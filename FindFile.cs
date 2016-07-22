using System;

namespace azLib
{
	/// <summary>
	/// Summary description for FindFile.
	/// </summary>
	public class FindFile: IDisposable
	{
		public Kernel32.WIN32_FIND_DATA data;
		private IntPtr Handle;
		
		public FindFile() {
			data = new azLib.Kernel32.WIN32_FIND_DATA();
			Handle = Kernel32.INVALID_HANDLE_VALUE; 
		}

		public bool FindFirst(String fileName)
		{
			if (Found) {
				FindClose();
			}
			Handle = Kernel32.FindFirstFile(fileName , data);
			return Found; 
		}

		~FindFile() {
			Free();
		}

		public bool Found {
			get {return Handle != Kernel32.INVALID_HANDLE_VALUE;}
		}

		public bool FindNext() {
			return Kernel32.FindNextFile(Handle,data); 
		}

		public void FindClose() {
			   Dispose(); 
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Free();
		}

		private void Free() {
			lock(this) {
				if (Found) {
					Kernel32.FindClose(Handle);
					Handle = Kernel32.INVALID_HANDLE_VALUE; 
				}
			}
		}

	}
}
