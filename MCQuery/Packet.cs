using System;

namespace MCQuery
{
    internal class Packet
    {
        private byte[] buffer;
        private Int32 location;
        private Int32 length;

        internal Int32 Location
        {
            get
            {
                return this.location;
            }
        }

        internal Int32 Length
        {
            get
            {
                return this.length;
            }
        }

        internal Packet ()
        {
            this.buffer = new byte[65535];
            this.length = 0;
        }

        internal Packet (Int32 size)
        {
            this.buffer = new byte[size];
            this.length = size;
        }

        internal Packet (byte[] data)
        {
            this.buffer = data;
            this.length = data.Length;
        }

        internal byte[] Finalize ()
        {
            byte[] final = new byte[this.length];
            Buffer.BlockCopy(this.buffer, 0, final, 0, this.length);
            return final;
        }

        internal void Write (object value, Int32 offset = 0)
        {
            this.location = offset > 0 ? offset : this.location;

            Type T = value.GetType();

            if (T == typeof(byte))
            {
                if (this.CheckBounds(1, T.Name))
                {
                    byte o = Convert.ToByte(value);
                    this.buffer[this.location++] = (byte)o;
                    if (this.location >= this.length)
                        this.length += 1;
                }
            }

            else if (T == typeof(Char))
            {
                if (this.CheckBounds(2, T.Name))
                {
                    Char o = Convert.ToChar(value);
                    this.buffer[this.location++] = (byte)o;
                    this.buffer[this.location++] = (byte)((int)o >> 8);
                    if (this.location >= this.length)
                        this.length += 2;
                }
            }

            else if (T == typeof(Int16) || T == typeof(UInt16))
            {
                if (this.CheckBounds(2, T.Name))
                {
                    Int16 o = Convert.ToInt16(value);
                    this.buffer[this.location++] = (byte)o;
                    this.buffer[this.location++] = (byte)((int)o >> 8);
                    if (this.location >= this.length)
                        this.length += 2;
                }
            }

            else if (T == typeof(Int32) || T == typeof(UInt32) || T == typeof(Single))
            {
                if (this.CheckBounds(4, T.Name))
                {
                    Int32 o = Convert.ToInt32(value);
                    this.buffer[this.location++] = (byte)o;
                    this.buffer[this.location++] = (byte)((int)o >> 8);
                    this.buffer[this.location++] = (byte)((int)o >> 16);
                    this.buffer[this.location++] = (byte)((int)o >> 24);
                    if (this.location >= this.length)
                        this.length += 4;
                }
            }

            else if (T == typeof(Int64) || T == typeof(UInt64) || T == typeof(Double))
            {
                if (this.CheckBounds(8, T.Name))
                {
                    Int64 o = Convert.ToInt64(value);
                    this.buffer[this.location++] = (byte)o;
                    this.buffer[this.location++] = (byte)((int)o >> 8);
                    this.buffer[this.location++] = (byte)((int)o >> 16);
                    this.buffer[this.location++] = (byte)((int)o >> 24);
                    this.buffer[this.location++] = (byte)((int)o >> 32);
                    this.buffer[this.location++] = (byte)((int)o >> 40);
                    this.buffer[this.location++] = (byte)((int)o >> 48);
                    this.buffer[this.location++] = (byte)((int)o >> 56);
                    if (this.location >= this.length)
                        this.length += 8;
                }
            }

            else if (T == typeof(String))
            {
                byte[] string_bytes = System.Text.Encoding.UTF8.GetBytes(Convert.ToString(value));
                if (this.CheckBounds(string_bytes.Length, T.Name))
                {
                    Buffer.BlockCopy(string_bytes, 0, this.buffer, this.location, string_bytes.Length);
                    this.location += string_bytes.Length;
                    if (this.location >= this.length)
                        this.length += string_bytes.Length + 1;
                }
            }

            else throw new NotSupportedException("Unsupported type for writing: " + T.Name);
        }

        internal R Read<R> (Int32 offset = 0)
        {
            this.location = offset > 0 ? offset : this.location;

            Type T = typeof(R);

            if (T == typeof(byte))
            {
                if (this.CheckBounds(1, T.Name))
                {
                    R ret = (R)Convert.ChangeType(this.buffer[this.location], T);

                    this.location++;
                    return ret;
                }
            }

            else if (T == typeof(Char) || T == typeof(Int16) || T == typeof(UInt16))
            {
                if (this.CheckBounds(2, T.Name))
                {
                    R ret = (R)Convert.ChangeType(
                        (
                            (this.buffer[this.location + 1] << 8) +
                            this.buffer[this.location]
                        ), T);

                    this.location += 2;
                    return ret;
                }
            }

            else if (T == typeof(Single))
            {
                R ret = (R)Convert.ChangeType(System.BitConverter.ToSingle(this.buffer, this.location), T);
                this.location += 4;
                return ret;
            }

            else if (T == typeof(Int32) || T == typeof(UInt32))
            {
                if (this.CheckBounds(4, T.Name))
                {
                    R ret = (R)Convert.ChangeType(
                        (
                            (this.buffer[this.location + 3] << 24) +
                            (this.buffer[this.location + 2] << 16) +
                            (this.buffer[this.location + 1] << 8) +
                            this.buffer[this.location]
                        ), T);

                    this.location += 4;
                    return ret;
                }
            }

            else if (T == typeof(Double))
            {
                R ret = (R)Convert.ChangeType(System.BitConverter.ToDouble(this.buffer, this.location), T);
                this.location += 8;
                return ret;
            }

            else if (T == typeof(Int64) || T == typeof(UInt64))
            {
                if (this.CheckBounds(8, T.Name))
                {
                    R ret = (R)Convert.ChangeType(
                        (
                            (this.buffer[this.location + 7] << 56) +
                            (this.buffer[this.location + 6] << 48) +
                            (this.buffer[this.location + 5] << 40) +
                            (this.buffer[this.location + 4] << 32) +
                            (this.buffer[this.location + 3] << 24) +
                            (this.buffer[this.location + 2] << 16) +
                            (this.buffer[this.location + 1] << 8) +
                            this.buffer[this.location]
                        ), T);

                    this.location += 8;
                    return ret;
                }
            }

            else if (T == typeof(String))
            {
                int l = 0;

                for (int i = this.location; i < this.length; i++)
                    if (this.buffer[i] != 0)
                        l++;
                    else break;

                R ret = (R)Convert.ChangeType(System.Text.Encoding.UTF8.GetString(this.buffer, this.location, l), T);

                this.location += l + 1;
                return ret;
            }

            else throw new NotSupportedException("Unsupported type for reading: " + T.Name);

            return default(R);
        }

        internal void Skip (Int32 offset)
        {
            this.location += offset;
        }

        internal bool CheckBounds (Int32 offset, String type_name)
        {
            if (this.location + offset <= this.buffer.Length)
                return true;
            else
                throw new IndexOutOfRangeException("Data exceeded bounds. Type: " + type_name);
        }
    }
}
