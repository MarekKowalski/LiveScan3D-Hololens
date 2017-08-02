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

bool TransferSocket::GetFrame(Platform::Array<float>^* vertices, Platform::Array<uint8>^* colors, Platform::Array<int>^* triangles, Platform::Array<int>^* chunksVertices, Platform::Array<int>^* chunksTriangles)
{
	if (frameReceiver != nullptr && frameReceiver->Status == Windows::Foundation::AsyncStatus::Completed)
	{
		*vertices = this->vertices;
		*colors = this->colors;
		*triangles = this->triangles;
		*chunksVertices = this->chunksVertices;
		*chunksTriangles = this->chunksTriangles;

		return true;
	}

	return false;
}

void TransferSocket::ReceiveFrame()
{
	int nVertices = *(int*)ReadBytesBlocking(socket, 4).data();
	int nTriangles = *(int*)ReadBytesBlocking(socket, 4).data();
	int nChunks = *(int*)ReadBytesBlocking(socket, 4).data();


	chunksVertices = ref new Array<int>(nChunks);
	string res = ReadBytesBlocking(socket, sizeof(int) * nChunks);
	memcpy(chunksVertices->Data, res.data(), sizeof(int) * nChunks);

	chunksTriangles = ref new Array<int>(nChunks);
	res = ReadBytesBlocking(socket, sizeof(int) * nChunks);
	memcpy(chunksTriangles->Data, res.data(), sizeof(int) * nChunks);


	vertices = ref new Array<float>(nVertices * 3);
	//short *tempVertices = new short[nVertices * 3];
	


	res = ReadBytesBlocking(socket, sizeof(float) * nVertices * 3);
	memcpy(vertices->Data, res.data(), sizeof(float) * nVertices * 3);

	//for (int i = 0; i < nVertices * 3; i++)
	//{
	//	vertices[i] = tempVertices[i] / 1000.0f;
	//}

	//delete[] tempVertices;
	colors = ref new Array<unsigned char>(nVertices * 3);
	res = ReadBytesBlocking(socket, sizeof(unsigned char) * nVertices * 3);
	memcpy(colors->Data, res.data(), sizeof(unsigned char) * nVertices * 3);

	triangles = ref new Array<int>(3 * nTriangles);
	res = ReadBytesBlocking(socket, sizeof(int) * nTriangles * 3);
	memcpy(triangles->Data, res.data(), sizeof(int) * nTriangles * 3);

}