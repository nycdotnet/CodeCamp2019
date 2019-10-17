using Codecamp.Sessions;
using Google.Protobuf;
using System;
using System.IO;
using System.Security.Cryptography;

namespace CodeCamp.Demo.ProtoSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creates an instance of the `Session` class which implements the `Session` message from the `Sessions` proto.
            var session = new Session();

            // Assign values to a few fields
            session.Abstract = "fun session";
            session.SpeakerName = "Steve Ognibene";

            // notice how this is a CamelCase enum name and option?  It was converted to the C# idiom by protoc.
            // if you follow the [Protobuf style guide](https://developers.google.com/protocol-buffers/docs/style),
            // protoc will convert your protos to the sort of code you expect to see in the language of your choice.
            session.SpeakerStatus = SpeakerStatus.Returning;
            

            // Use the `.ToByteArray()` extension method from the Protobuf library to get the serialized array of bytes representing the object's value.
            var content = session.ToByteArray();

            // calculate a SHA-256 hash for this byte array.
            var previousHash = CalculateSha256HashString(content);

            // write out the raw bytes of the proto to a file so you can look at them in a text editor if you want.
            var filePath = Path.Join(Environment.ExpandEnvironmentVariables("%TEMP%"), "SessionProto.bin");
            File.WriteAllBytes(filePath, content);
            Console.WriteLine($"Wrote {content.Length} bytes to {filePath}.");
            
            // we will null out the original object.  Just to "prove there is no funny business happening here".
            session = null;

            // we read the bytes from disk and call the parser on the `Session` class.  (This parser is generated)
            // this should give us a `Session` object that is identical to the one we previously saved (but nulled-out).
            var rehydratedSession = Session.Parser.ParseFrom(File.ReadAllBytes(filePath));


            // we calculate the SHA-256 hash of the new object and compare to the previous hash.
            if (previousHash == CalculateSha256HashString(rehydratedSession.ToByteArray()))
            {
                // they should be identical.
                Console.WriteLine("After reading the proto bytes back from disk and rehydrating an object from them, the hash codes were equal.");
            }
            else
            {
                // this should not happen.
                Console.WriteLine("Something unexpected has occurred, and the hash codes were not equal!!!!");
            }
        }

        static string CalculateSha256HashString(byte[] bytes)
        {
            using (var sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(bytes)).ToString().Replace("-","");
            }
        }
    }
}

