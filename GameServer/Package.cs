using System;
using System.Collections.Generic;
using System.IO;

public class Package
{
    private int readPosition;
    public List<byte> myBytes = new List<byte>();
    public bool endConnection = false;
    public byte[] toReadBytes;

    public int fromId = -1;
    public int toId = -1;

    #region Write
    public void Write(byte[] toWrite)
    {
        foreach (byte b in toWrite)
        {
            myBytes.Add(b);
        }
    }

    public void WriteInt(int i)
    {
        byte[] toWrite = BitConverter.GetBytes(i);
        Write(toWrite);
    }

    public void WriteString(string input)
    {
        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        byte[] toWrite = enc.GetBytes(input);
        WriteInt(toWrite.Length);
        Write(toWrite);
    }
    public void WriteBool(bool b)
    {
        byte[] toWrite = BitConverter.GetBytes(b);
        Write(toWrite);
    }
    public void WriteFile(string path)
    {
        byte[] toWrite = File.ReadAllBytes(path);
        WriteInt(toWrite.Length);
        Write(toWrite);
    }
    #endregion

    #region Read
    public void ReadFile(string path)
    {
        int lenght = ReadInt();
        List<byte> fileBytes = new List<byte>();
        int i = 0;
        foreach (byte b in toReadBytes)
        {
            if (i > readPosition - 1 && i < lenght + readPosition)
            {
                fileBytes.Add(b);
            }
            i++;
        }
        File.WriteAllBytes(path, fileBytes.ToArray());
        readPosition += fileBytes.ToArray().Length;
        //Console.WriteLine("Size of File: " + lenght + " / " + fileBytes.ToArray().Length.ToString() + " Bytes");
    }
    public bool ReadBool()
    {
        bool result = false;

        result = BitConverter.ToBoolean(toReadBytes, readPosition);
        readPosition += 1;

        return result;
    }
    public int ReadInt(bool changeReadpos = true)
    {
        int result = BitConverter.ToInt32(myBytes.ToArray(), readPosition);

        if (changeReadpos)
            readPosition += 4;

        return result;
    }

    public string ReadString()
    {
        string result = "(empty)";
        int size = ReadInt();
        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        result = enc.GetString(toReadBytes, readPosition, size);
        readPosition += size;
        return result;
    }
    #endregion

    public Package EndConnection()
    {
        Package package = new Package();
        package.endConnection = true;
        return package;
    }
    public byte[] GetBytePart(int lenght)
    {
        List<byte> part = new List<byte>();

        int i = 0;
        foreach (byte b in myBytes)
        {
            if (i > readPosition - 1 && i < lenght + readPosition)
            {
                part.Add(b);
            }
            i++;
        }

        return part.ToArray();
    }

    public int GetLenght()
    {
        return myBytes.Count;
    }
    public Package()
    {
        readPosition = 0;
    }
    public Package(byte[] bytes)
    {
        readPosition = 0;

        toReadBytes = bytes;

        foreach (byte b in bytes)
        {
            myBytes.Add(b);
        }
    }
    public byte[] GetAllBytes()
    {
        return myBytes.ToArray();
    }

    public bool CanRead()
    {
        if (readPosition >= toReadBytes.Length)
            return false;
        else
            return true;
    }
}

