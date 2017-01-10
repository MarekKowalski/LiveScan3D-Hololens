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
		bool GetFrame(Platform::Array<float>^* vertices, Platform::Array<uint8>^* colors);
	private:
		~TransferSocket();
		void ReceiveFrame();

		SocketClient socket;
		Windows::Foundation::IAsyncAction^ frameReceiver;

		Platform::Array<float>^ vertices;
		Platform::Array<uint8>^ colors;
	};
}