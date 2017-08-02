#pragma once
#include <string>
#include <vector>

#include "socketCS.h"


namespace NetworkCommunication
{
	public ref class TransferSocket sealed
	{
	public:
		TransferSocket(Platform::String^ IP, int port);
		
		void RequestFrame();
		void ReceiveFrameAsync();
		bool GetFrame(Platform::Array<float>^* vertices, Platform::Array<uint8>^* colors, Platform::Array<int>^* triangles, Platform::Array<int>^* chunksVertices, Platform::Array<int>^* chunksTriangles);
	private:
		~TransferSocket();
		void ReceiveFrame();

		SocketClient socket;
		Windows::Foundation::IAsyncAction^ frameReceiver;

		Platform::Array<float>^ vertices;
		Platform::Array<uint8>^ colors;
		Platform::Array<int>^ triangles;
		Platform::Array<int>^ chunksVertices;
		Platform::Array<int>^ chunksTriangles;
	};
}