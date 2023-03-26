using System;
using System.Collections;
using SpalkTechTestWebApp.ApplicationService.Interfaces;
using SpalkTechTestWebApp.ApplicationService.Models;

namespace SpalkTechTestWebApp.ApplicationService.Classes
{
    public class StreamFileValidator : IStreamFileValidator
    {
        public StreamFileValidator(){}

        public ValidationResponse ValidateUploadedStream(Stream stream)
        {
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            byte[] byteStream = memoryStream.ToArray();

            int offset = 0;
            int packetIndex = 0;
            var pids = new List<int>();

            if (byteStream.Length % 188 != 0) {
                if (byteStream[0] == 0x47) {

                }
            }

            while (offset < byteStream.Length)
            {
                byte syncByte = byteStream[offset];

                if (syncByte == 0x47)
                {
                    int pid = ParseAndRecordPid(byteStream[offset + 1], byteStream[offset + 2]);
                    pids.Add(pid);

                    offset += 188; // 188 packet fixed size
                    packetIndex++;
                }
                else if (offset == 0)
                {
                    int bytesSkipped = 0;
                    bool hasError = false;

                    do
                    {
                        if (byteStream[0] == 0x47) {
                            byteStream = byteStream.Skip(1).ToArray();
                        }

                        int nextSyncByteIndex = Array.IndexOf(byteStream, (byte)0x47);
                        byteStream = byteStream.Skip(nextSyncByteIndex).ToArray();
                        bytesSkipped += nextSyncByteIndex;
                        if (bytesSkipped > 188)
                        {
                            hasError = true;
                            break;
                        }
                        if (byteStream.Length <= 188) {
                            break;
                        }
                    } while (byteStream[188] != 0x47);


                    if (hasError) {
                        return new ValidationResponse
                        {
                            IsStreamValid = false,
                            ErrorMessage = $"No sync byte present in packet 1, offset 188",
                            SuccessMessage = null
                        };
                    }
                }
                else
                {
                    return new ValidationResponse
                    {
                        IsStreamValid = false,
                        ErrorMessage = $"No sync byte present in packet {packetIndex}, offset {offset}",
                        SuccessMessage = null
                    };
                }
            }


            string successMessage = "";

            foreach (var pid in pids.Distinct().OrderBy(p => p).ToList())
            {
                successMessage += "0x" + pid.ToString("X") + "\n";
            }

            return new ValidationResponse
            {
                IsStreamValid = true,
                ErrorMessage = null,
                SuccessMessage = successMessage
            };
        }

        private static int ParseAndRecordPid(byte secondByte, byte thirdByte)
        {
            var bitsInSecondByte = new BitArray(new[] { secondByte });
            var bitsInThirdByte = new BitArray(new[] { thirdByte });

            var boolArray = new bool[13];
            int index = 0;

            for (int j = 0; j < bitsInThirdByte.Length; j++)
            {
                boolArray[index++] = bitsInThirdByte[j];
            }

            for (int i = 0; i < 5; i++) // actually they are the first 5 bits
            {
                boolArray[index++] = bitsInSecondByte[i];
            }

            var pidBits = new BitArray(boolArray);

            int[] intPresentation = new int[1];

            pidBits.CopyTo(intPresentation, 0); // convert PID bits presentation to int presentation

            return intPresentation[0];
        }
    }
}

