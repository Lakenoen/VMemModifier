#include "Windows.h"
#include <iostream>
#include <cstddef>

struct MemInfo {
	long long baseAddr = 0;
	long long protect = 0;
	long long regionSize = 0;
	unsigned long state = 0;
	long long minAppAddr = 0;
	long long maxAppAddr = 0;
};

extern "C" {
	void __cdecl readMemory(int id, long long addr, long long size, std::byte* buffer, size_t& readed, unsigned long long& error) {
		HANDLE handle = OpenProcess(PROCESS_ALL_ACCESS | PROCESS_QUERY_INFORMATION, false, id);

		if (handle == INVALID_HANDLE_VALUE)
			return;

		DWORD old = 0;
		if (!ReadProcessMemory(handle, (void*)addr, buffer, size, &readed))
			error = GetLastError();
		CloseHandle(handle);
	}

	void __cdecl getMemInfo(int id, long long addr, MemInfo& memInfo, unsigned long long& error) {
		HANDLE handle = OpenProcess(PROCESS_ALL_ACCESS | PROCESS_QUERY_INFORMATION, false, id);
		if (handle == INVALID_HANDLE_VALUE || handle == NULL)
			return;

		MEMORY_BASIC_INFORMATION info{ 0 };
		if (VirtualQueryEx(handle, (void*)addr, &info, sizeof(MEMORY_BASIC_INFORMATION)) == 0) {
			error = GetLastError();
			CloseHandle(handle);
			return;
		}
		memInfo.regionSize = info.RegionSize;
		memInfo.baseAddr = (long long)info.BaseAddress;
		memInfo.state = info.State;
		memInfo.protect = info.Protect;
		SYSTEM_INFO sysInfo{ 0 };
		GetSystemInfo(&sysInfo);
		memInfo.minAppAddr = (long long)sysInfo.lpMinimumApplicationAddress;
		memInfo.maxAppAddr = (long long)sysInfo.lpMaximumApplicationAddress;
		CloseHandle(handle);
	}

	void __cdecl writeMemory(int id, long long addr, std::byte* data, long long size, size_t& written, unsigned long long& error) {

		HANDLE handle = OpenProcess(PROCESS_ALL_ACCESS | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION, false, id);

		if (handle == INVALID_HANDLE_VALUE)
			return;

		DWORD old = 0;
		DebugActiveProcess(id);
		VirtualProtectEx(handle, (void*)addr, size, PAGE_READWRITE, &old);
		if(!WriteProcessMemory(handle, (void*)addr, data, size, &written))
			error = GetLastError();
		VirtualProtectEx(handle, (void*)addr, size, old, &old);
		DebugActiveProcessStop(id);

		CloseHandle(handle);
	}

	void __cdecl inject(int id, const wchar_t* dllname, short& status) {
		size_t dllNameSize = wcslen(dllname);

		HANDLE handle = OpenProcess(PROCESS_ALL_ACCESS, false, id);
		if (handle == NULL || handle == INVALID_HANDLE_VALUE) {
			status = -1;
			return;
		}

		HMODULE loadLibModule = GetModuleHandle("kernel32.dll");
		if (loadLibModule == NULL || loadLibModule == INVALID_HANDLE_VALUE) {
			status = -2;
			goto finally;
		}

		void* fp = (void*)GetProcAddress(loadLibModule, "LoadLibraryW");
		if (fp == nullptr) {
			status = -3;
			goto finally;
		}

		void* dllNameAddr = VirtualAllocEx(handle, NULL, dllNameSize * sizeof(wchar_t), MEM_RESERVE | MEM_COMMIT, PAGE_EXECUTE_READWRITE);
		if (dllNameAddr == nullptr) {
			status = -4;
			goto finally;
		}

		size_t written = 0;
		WriteProcessMemory(handle, dllNameAddr, dllname, dllNameSize * sizeof(wchar_t), &written);
		if (written < dllNameSize) {
			status = -5;
			goto finally;
		}

		HANDLE thread = CreateRemoteThread(handle, NULL, 0, (LPTHREAD_START_ROUTINE)fp, dllNameAddr, 0, 0);
		if (thread == NULL || thread == INVALID_HANDLE_VALUE) {
			status = -6;
			goto finally;
		}

		finally:
		CloseHandle(handle);
	}

}
#ifdef DEBUG

int main() {
	return 0;
}

#endif // DEBUG