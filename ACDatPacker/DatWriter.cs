using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDatPacker
{
    class DatWriter
    {

        public DatWriter()
        {

        }


        public void Write(List<string> files, string path, uint blockSize = 0x400)
        {
            // Cell block size is 0x100, all others are 0x400 (Portal, Language, HiRes)

            var fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

            // First, we write all the DatFiles
            fileStream.Seek(blockSize, SeekOrigin.Begin);

            foreach(var file in files)
            {
                var fileBuffer = ReadFile(file);
            }

        }

        private void WriteFile(FileStream stream, uint blockSize, byte[] buffer)
        {
            // Thanks @MagNus for this function!

            // Dat "file" is broken up into sectors that are not neccessarily congruous. Next address is stored in first four bytes of each sector.
            uint nextAddress;

            int bufferOffset = 0;

            while (bufferOffset < buffer.Length)
            {
                if (buffer.Length - bufferOffset <= blockSize - 4)
                {
                    stream.Write(new byte[4], 0, 4);
                    stream.Write(buffer, bufferOffset, buffer.Length - bufferOffset);
                    bufferOffset += (buffer.Length - bufferOffset);
                }
                else
                {
                    nextAddress = Convert.ToUInt32(stream.Position + blockSize);
                    stream.Write(new byte[4] { (byte)nextAddress, (byte)(nextAddress >> 8), (byte)(nextAddress >> 16), (byte)(nextAddress >> 24) }, 0, 4);
                    stream.Write(buffer, bufferOffset, Convert.ToInt32(blockSize) - 4); // Write our sector from the buffer[]
                    bufferOffset += Convert.ToInt32(blockSize) - 4; // Adjust this so we know where in our buffer[] the next sector gets written from
                }
            }

            // Make sure we're on a Header.BlockSize boundary
            if (stream.Position % blockSize != 0)
                stream.Position += (stream.Position % blockSize);
        }

        /// <summary>
        /// Loads a file into a byte array
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private byte[] ReadFile(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

    }
}
