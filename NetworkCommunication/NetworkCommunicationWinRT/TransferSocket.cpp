#include "TransferSocket.h"
#include "socketCS.h"
#include <thread>

using namespace NetworkCommunication;
using namespace std;
using namespace Platform;
using namespace Windows::System::Threading;


string ReadBytesBlocking(SocketClient &socket, int nBytesToRead)
{
	string ret;

	while (ret.size() < nBytesToRead)
	{
		ret += socket.ReceiveBytes(nBytesToRead - ret.size());
	}

	return ret;
}

TransferSocket::TransferSocket(String^ IP, int port)
{
	string strIP = std::string(IP->Begin(), IP->End());
	socket = SocketClient(strIP, port);

	frameReceiver = nullptr;
}

TransferSocket::~TransferSocket()
{
	socket.Close();
}

void TransferSocket::RequestFrame()
{
	char buffer[1];
	buffer[0] = 0;

	socket.SendBytes(buffer, 1);
}

void TransferSocket::ReceiveFrameAsync()
{
	frameReceiver = ThreadPool::RunAsync(ref new WorkItemHandler([this](Windows::Foundation::IAsyncAction^ spAction)
	{
		ReceiveFrame();
	}	
	, Platform::CallbackContext::Any));

}

bool TransferSocket::GetFrame(Platform::Array<float>^* vertices, Platform::Array<uint8>^* colors)
{
	if (frameReceiver != nullptr && frameReceiver->Status == Windows::Foundation::AsyncStatus::Completed)
	{
		*vertices = this->vertices;
		*colors = this->colors;

		return true;
	}

	return false;
}

void TransferSocket::ReceiveFrame()
{
	int nVertices = *(int*)ReadBytesBlocking(socket, 4).data();

	vertices = ref new Array<float>(nVertices * 3);
	short *tempVertices = new short[nVertices * 3];
	colors = ref new Array<unsigned char>(nVertices * 3);


	string res = ReadBytesBlocking(socket, sizeof(short) * nVertices * 3);
	memcpy(tempVertices, res.data(), sizeof(short) * nVertices * 3);

	for (int i = 0; i < nVertices * 3; i++)
	{
		vertices[i] = tempVertices[i] / 1000.0f;
	}

	delete[] tempVertices;

	res = ReadBytesBlocking(socket, sizeof(unsigned char) * nVertices * 3);
	memcpy(colors->Data, res.data(), sizeof(unsigned char) * nVertices * 3);
}