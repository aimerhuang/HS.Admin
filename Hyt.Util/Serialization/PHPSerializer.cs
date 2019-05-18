using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Reflection;

namespace Hyt.Util.Serialization
{
    public interface Serializable
    {
        byte[] Serialize();
        void UnSerialize(byte[] ss);
    }
    public class UnSerializeException : Exception
    {
        public UnSerializeException() { }
        public UnSerializeException(string message) : base(message) { }
        public UnSerializeException(string message, Exception inner) : base(message, inner) { }
    }
    /// <summary>
    /// PHP序列化
    /// </summary>
    /// <remarks>2016-9-3 杨浩 添加</remarks>
    public class PHPSerializer
    {
        private static Hashtable __ns;
        private const byte __Quote = 34;
        private const byte __0 = 48;
        private const byte __1 = 49;
        private const byte __Colon = 58;
        private const byte __Semicolon = 59;
        private const byte __C = 67;
        private const byte __N = 78;
        private const byte __O = 79;
        private const byte __R = 82;
        private const byte __U = 85;
        private const byte __Slash = 92;
        private const byte __a = 97;
        private const byte __b = 98;
        private const byte __d = 100;
        private const byte __i = 105;
        private const byte __r = 114;
        private const byte __s = 115;
        private const byte __LeftB = 123;
        private const byte __RightB = 125;
        static PHPSerializer()
        {
            __ns = new Hashtable();
            Assembly[] assem = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assem.Length; i++)
            {
                Module[] m = assem[i].GetModules();
                for (int j = 0; j < m.Length; j++)
                {
                    try
                    {
                        Type[] t = m[j].GetTypes();
                        for (int k = 0; k < t.Length; k++)
                        {
                            if (t[k].Namespace != null && !__ns.ContainsKey(t[k].Namespace))
                            {
                                __ns[t[k].Namespace] = assem[i].FullName;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        private PHPSerializer()
        {
        }
        public static byte[] Serialize(object obj)
        {
            return Serialize(obj, Encoding.UTF8);
        }
        public static byte[] Serialize(object obj, Encoding encoding)
        {
            Hashtable ht = new Hashtable();
            int hv = 1;
            MemoryStream stream = new MemoryStream();
            stream.Seek(0, SeekOrigin.Begin);
            Serialize(stream, obj, ht, ref hv, encoding);
            byte[] result = stream.ToArray();
            stream.Close();
            return result;
        }
        private static void Serialize(MemoryStream stream, object obj, Hashtable ht, ref int hv, Encoding encoding)
        {
            if (obj == null)
            {
                hv++;
                WriteNull(stream);
            }
            else if (obj is Boolean)
            {
                hv++;
                WriteBoolean(stream, ((Boolean)obj) ? __1 : __0);
            }
            else if ((obj is Byte) || (obj is SByte) || (obj is Int16) || (obj is UInt16) || (obj is Int32))
            {
                hv++;
                WriteInteger(stream, Encoding.ASCII.GetBytes(obj.ToString()));
            }
            else if ((obj is UInt32) || (obj is Int64) || (obj is UInt64) || (obj is Decimal))
            {
                hv++;
                WriteDouble(stream, Encoding.ASCII.GetBytes(obj.ToString()));
            }
            else if (obj is Single)
            {
                hv++;
                if (Single.IsNaN((Single)obj))
                {
                    WriteDouble(stream, Encoding.ASCII.GetBytes("NAN"));
                }
                else if (Single.IsPositiveInfinity((Single)obj))
                {
                    WriteDouble(stream, Encoding.ASCII.GetBytes("INF"));
                }
                else if (Single.IsNegativeInfinity((Single)obj))
                {
                    WriteDouble(stream, Encoding.ASCII.GetBytes("-INF"));
                }
                else
                {
                    WriteDouble(stream, Encoding.ASCII.GetBytes(obj.ToString()));
                }
            }
            else if (obj is Double)
            {
                hv++;
                if (Double.IsNaN((Double)obj))
                {
                    WriteDouble(stream, Encoding.ASCII.GetBytes("NAN"));
                }
                else if (Double.IsPositiveInfinity((Double)obj))
                {
                    WriteDouble(stream, Encoding.ASCII.GetBytes("INF"));
                }
                else if (Double.IsNegativeInfinity((Double)obj))
                {
                    WriteDouble(stream, Encoding.ASCII.GetBytes("-INF"));
                }
                else
                {
                    WriteDouble(stream, Encoding.ASCII.GetBytes(obj.ToString()));
                }
            }
            else if ((obj is Char) || (obj is String))
            {
                hv++;
                WriteString(stream, encoding.GetBytes(obj.ToString()));
            }
            else if (obj is Array)
            {
                if (ht.ContainsKey(obj.GetHashCode()))
                {
                    WritePointRef(stream, Encoding.ASCII.GetBytes(ht[obj.GetHashCode()].ToString()));
                }
                else
                {
                    ht.Add(obj.GetHashCode(), hv++);
                    WriteArray(stream, obj as Array, ht, ref hv, encoding);
                }
            }
            else if (obj is ArrayList)
            {
                if (ht.ContainsKey(obj.GetHashCode()))
                {
                    WritePointRef(stream, Encoding.ASCII.GetBytes(ht[obj.GetHashCode()].ToString()));
                }
                else
                {
                    ht.Add(obj.GetHashCode(), hv++);
                    WriteArrayList(stream, obj as ArrayList, ht, ref hv, encoding);
                }
            }
            else if (obj is Hashtable)
            {
                if (ht.ContainsKey(obj.GetHashCode()))
                {
                    WritePointRef(stream, Encoding.ASCII.GetBytes(ht[obj.GetHashCode()].ToString()));
                }
                else
                {
                    ht.Add(obj.GetHashCode(), hv++);
                    WriteHashtable(stream, obj as Hashtable, ht, ref hv, encoding);
                }
            }
            else
            {
                if (ht.ContainsKey(obj.GetHashCode()))
                {
                    hv++;
                    WriteRef(stream, Encoding.ASCII.GetBytes(ht[obj.GetHashCode()].ToString()));
                }
                else
                {
                    ht.Add(obj.GetHashCode(), hv++);
                    WriteObject(stream, obj, ht, ref hv, encoding);
                }
            }
        }
        private static void WriteNull(MemoryStream stream)
        {
            stream.WriteByte(__N);
            stream.WriteByte(__Semicolon);
        }
        private static void WriteRef(MemoryStream stream, byte[] r)
        {
            stream.WriteByte(__r);
            stream.WriteByte(__Colon);
            stream.Write(r, 0, r.Length);
            stream.WriteByte(__Semicolon);
        }
        private static void WritePointRef(MemoryStream stream, byte[] p)
        {
            stream.WriteByte(__R);
            stream.WriteByte(__Colon);
            stream.Write(p, 0, p.Length);
            stream.WriteByte(__Semicolon);
        }
        private static void WriteBoolean(MemoryStream stream, byte b)
        {
            stream.WriteByte(__b);
            stream.WriteByte(__Colon);
            stream.WriteByte(b);
            stream.WriteByte(__Semicolon);
        }
        private static void WriteInteger(MemoryStream stream, byte[] i)
        {
            stream.WriteByte(__i);
            stream.WriteByte(__Colon);
            stream.Write(i, 0, i.Length);
            stream.WriteByte(__Semicolon);
        }
        private static void WriteDouble(MemoryStream stream, byte[] d)
        {
            stream.WriteByte(__d);
            stream.WriteByte(__Colon);
            stream.Write(d, 0, d.Length);
            stream.WriteByte(__Semicolon);
        }
        private static void WriteString(MemoryStream stream, byte[] s)
        {
            byte[] slen = Encoding.ASCII.GetBytes(s.Length.ToString());
            stream.WriteByte(__s);
            stream.WriteByte(__Colon);
            stream.Write(slen, 0, slen.Length);
            stream.WriteByte(__Colon);
            stream.WriteByte(__Quote);
            stream.Write(s, 0, s.Length);
            stream.WriteByte(__Quote);
            stream.WriteByte(__Semicolon);
        }
        private static void WriteArray(MemoryStream stream, Array a, Hashtable ht, ref int hv, Encoding encoding)
        {
            if (a.Rank == 1)
            {
                int len = a.GetLength(0);
                byte[] alen = Encoding.ASCII.GetBytes(len.ToString());
                int lb = a.GetLowerBound(0);
                int ub = a.GetUpperBound(0);
                stream.WriteByte(__a);
                stream.WriteByte(__Colon);
                stream.Write(alen, 0, alen.Length);
                stream.WriteByte(__Colon);
                stream.WriteByte(__LeftB);
                for (int i = lb; i <= ub; i++)
                {
                    WriteInteger(stream, Encoding.ASCII.GetBytes(i.ToString()));
                    Serialize(stream, a.GetValue(i), ht, ref hv, encoding);
                }
                stream.WriteByte(__RightB);
            }
            else
            {
                WriteArray(stream, a, new int[] { 0 }, ht, ref hv, encoding);
            }
        }
        private static void WriteArray(MemoryStream stream, Array a, int[] indices, Hashtable ht, ref int hv, Encoding encoding)
        {
            int n = indices.Length;
            int dimension = n - 1;
            int[] temp = new int[n + 1];
            indices.CopyTo(temp, 0);
            int len = a.GetLength(dimension);
            byte[] alen = Encoding.ASCII.GetBytes(len.ToString());
            int lb = a.GetLowerBound(dimension);
            int ub = a.GetUpperBound(dimension);
            stream.WriteByte(__a);
            stream.WriteByte(__Colon);
            stream.Write(alen, 0, alen.Length);
            stream.WriteByte(__Colon);
            stream.WriteByte(__LeftB);
            for (int i = lb; i <= ub; i++)
            {
                WriteInteger(stream, Encoding.ASCII.GetBytes(i.ToString()));
                if (a.Rank == n)
                {
                    indices[n - 1] = i;
                    Serialize(stream, a.GetValue(indices), ht, ref hv, encoding);
                }
                else
                {
                    temp[n - 1] = i;
                    WriteArray(stream, a, temp, ht, ref hv, encoding);
                }
            }
            stream.WriteByte(__RightB);
        }
        private static void WriteArrayList(MemoryStream stream, ArrayList a, Hashtable ht, ref int hv, Encoding encoding)
        {
            int len = a.Count;
            byte[] alen = Encoding.ASCII.GetBytes(len.ToString());
            stream.WriteByte(__a);
            stream.WriteByte(__Colon);
            stream.Write(alen, 0, alen.Length);
            stream.WriteByte(__Colon);
            stream.WriteByte(__LeftB);
            for (int i = 0; i < len; i++)
            {
                WriteInteger(stream, Encoding.ASCII.GetBytes(i.ToString()));
                Serialize(stream, a[i], ht, ref hv, encoding);
            }
            stream.WriteByte(__RightB);
        }
        private static void WriteHashtable(MemoryStream stream, Hashtable h, Hashtable ht, ref int hv, Encoding encoding)
        {
            int len = h.Count;
            byte[] hlen = Encoding.ASCII.GetBytes(len.ToString());
            stream.WriteByte(__a);
            stream.WriteByte(__Colon);
            stream.Write(hlen, 0, hlen.Length);
            stream.WriteByte(__Colon);
            stream.WriteByte(__LeftB);
            foreach (DictionaryEntry entry in h)
            {
                if ((entry.Key is Byte) || (entry.Key is SByte) || (entry.Key is Int16) || (entry.Key is UInt16) || (entry.Key is Int32))
                {
                    WriteInteger(stream, Encoding.ASCII.GetBytes(entry.Key.ToString()));
                }
                else if (entry.Key is Boolean)
                {
                    WriteInteger(stream, new byte[] { ((Boolean)entry.Key) ? __1 : __0 });
                }
                else
                {
                    WriteString(stream, encoding.GetBytes(entry.Key.ToString()));
                }
                Serialize(stream, entry.Value, ht, ref hv, encoding);
            }
            stream.WriteByte(__RightB);
        }
        private static void WriteObject(MemoryStream stream, object obj, Hashtable ht, ref int hv, Encoding encoding)
        {
            Type type = obj.GetType();
            if (type.IsSerializable)
            {
                byte[] typename = encoding.GetBytes(type.Name);
                byte[] typenamelen = Encoding.ASCII.GetBytes(typename.Length.ToString());
                if (obj is Serializable)
                {
                    byte[] cs = ((Serializable)obj).Serialize();
                    byte[] cslen = Encoding.ASCII.GetBytes(cs.Length.ToString());
                    stream.WriteByte(__C);
                    stream.WriteByte(__Colon);
                    stream.Write(typenamelen, 0, typenamelen.Length);
                    stream.WriteByte(__Colon);
                    stream.WriteByte(__Quote);
                    stream.Write(typename, 0, typename.Length);
                    stream.WriteByte(__Quote);
                    stream.WriteByte(__Colon);
                    stream.Write(cslen, 0, cslen.Length);
                    stream.WriteByte(__Colon);
                    stream.WriteByte(__LeftB);
                    stream.Write(cs, 0, cs.Length);
                    stream.WriteByte(__RightB);
                }
                else
                {
                    MethodInfo __sleep = null;
                    try
                    {
                        __sleep = type.GetMethod("__sleep", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase, null, new Type[0], new ParameterModifier[0]);
                    }
                    catch { }
                    int fl = 0;
                    FieldInfo[] f;
                    if (__sleep != null)
                    {
                        string[] fns = (string[])__sleep.Invoke(obj, null);
                        f = new FieldInfo[fns.Length];
                        for (int i = 0, len = f.Length; i < len; i++)
                        {
                            f[i] = type.GetField(fns[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        }
                    }
                    else
                    {
                        f = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    }
                    for (int i = 0, len = f.Length; i < len; i++)
                    {
                        if (f[i] != null && !(f[i].IsNotSerialized || f[i].IsInitOnly || f[i].IsLiteral))
                        {
                            fl++;
                        }
                    }
                    byte[] flen = Encoding.ASCII.GetBytes(fl.ToString());
                    stream.WriteByte(__O);
                    stream.WriteByte(__Colon);
                    stream.Write(typenamelen, 0, typenamelen.Length);
                    stream.WriteByte(__Colon);
                    stream.WriteByte(__Quote);
                    stream.Write(typename, 0, typename.Length);
                    stream.WriteByte(__Quote);
                    stream.WriteByte(__Colon);
                    stream.Write(flen, 0, flen.Length);
                    stream.WriteByte(__Colon);
                    stream.WriteByte(__LeftB);
                    for (int i = 0, len = f.Length; i < len; i++)
                    {
                        if (f[i] != null && !(f[i].IsNotSerialized || f[i].IsInitOnly || f[i].IsLiteral))
                        {
                            if (f[i].IsPublic)
                            {
                                WriteString(stream, encoding.GetBytes(f[i].Name));
                            }
                            else if (f[i].IsPrivate)
                            {
                                WriteString(stream, encoding.GetBytes("\0" + f[i].DeclaringType.Name + "\0" + f[i].Name));
                            }
                            else
                            {
                                WriteString(stream, encoding.GetBytes("\0*\0" + f[i].Name));
                            }
                            Serialize(stream, f[i].GetValue(obj), ht, ref hv, encoding);
                        }
                    }
                    stream.WriteByte(__RightB);
                }
            }
            else
            {
                WriteNull(stream);
            }
        }
        public static object ChangeType(object obj, Type destType)
        {
            if (obj == null)
            {
                return null;
            }

            Type sourceType = obj.GetType();

            if (sourceType == destType)
            {
                return obj;
            }

            if (obj is ArrayList && destType.IsArray)
            {
                return ArrayListToArray(obj as ArrayList, destType.GetArrayRank(), destType.GetElementType());
            }
            else if (obj is ArrayList && destType == typeof(Hashtable))
            {
                return ArrayListToHashtable(obj as ArrayList);
            }
            else if ((!sourceType.IsByRef && destType.IsByRef) || (!sourceType.IsPointer && destType.IsPointer))
            {
                return Convert.ChangeType(obj, destType.GetElementType());
            }
            else
            {
                return Convert.ChangeType(obj, destType);
            }
        }
        private static Hashtable ArrayListToHashtable(ArrayList a)
        {
            int n = a.Count;
            Hashtable h = new Hashtable(n);
            for (int i = 0; i < n; i++)
            {
                h[i] = a[i];
            }
            return h;
        }
        private static Array ArrayListToArray(ArrayList obj, int rank, Type elementType)
        {
            int[] lengths = new int[rank];
            ArrayList al = obj;
            for (int i = 0; i < rank; i++)
            {
                lengths[i] = al.Count;
                if (al.Count > 0)
                {
                    al = al[0] as ArrayList;
                }
                else
                {
                    break;
                }
            }
            Array result = Array.CreateInstance(elementType, lengths);
            if (lengths[0] > 0)
            {
                ArrayListToArray(obj as ArrayList, result, new int[] { 0 }, elementType);
            }
            return result;
        }
        private static void ArrayListToArray(ArrayList source, Array dest, int[] indices, Type elementType)
        {
            int n = indices.Length;
            int dimension = n - 1;
            int[] temp = new int[n + 1];
            indices.CopyTo(temp, 0);
            int len = dest.GetLength(dimension);
            for (int i = 0; i < len; i++)
            {
                if (dest.Rank == n)
                {
                    indices[n - 1] = i;
                    if (source[i] == null || elementType == typeof(object) || elementType == source[i].GetType())
                    {
                        dest.SetValue(source[i], indices);
                    }
                    else
                    {
                        dest.SetValue(ChangeType(source[i], elementType), indices);
                    }
                }
                else
                {
                    temp[n - 1] = i;
                    ArrayListToArray(source[i] as ArrayList, dest, temp, elementType);
                }
            }
        }
        private static object CreateInstance(Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch
            {
            }
            try
            {
                return Activator.CreateInstance(type, new object[] { 0 });
            }
            catch
            {
            }
            try
            {
                return Activator.CreateInstance(type, new object[] { false });
            }
            catch
            {
            }
            try
            {
                return Activator.CreateInstance(type, new object[] { "" });
            }
            catch
            {
            }
            try
            {
                FieldInfo[] f = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                for (int i = 0, n = f.Length; i < n; i++)
                {
                    if (f[i].FieldType == type)
                    {
                        return f[i].GetValue(null);
                    }
                }
            }
            catch
            {
            }
            MethodInfo[] m = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0, n = m.Length; i < n; i++)
            {
                if (m[i].ReturnType == type)
                {
                    try
                    {

                        return m[i].Invoke(null, null);
                    }
                    catch
                    {
                    }
                    try
                    {
                        return m[i].Invoke(null, new object[] { 0 });
                    }
                    catch
                    {
                    }
                    try
                    {
                        return m[i].Invoke(null, new object[] { false });
                    }
                    catch
                    {
                    }
                    try
                    {
                        return m[i].Invoke(null, new object[] { "" });
                    }
                    catch
                    {
                    }
                }
            }
            ThrowError("Can't create the instance of " + type.FullName);
            return null;
        }
        public static object UnSerialize(byte[] ss)
        {
            return UnSerialize(ss, null, Encoding.UTF8);
        }
        public static object UnSerialize(byte[] ss, Encoding encoding)
        {
            return UnSerialize(ss, null, encoding);

        }
        public static object UnSerialize(byte[] ss, Type type)
        {
            return UnSerialize(ss, type, Encoding.UTF8);
        }
        public static object UnSerialize(byte[] ss, Type type, Encoding encoding)
        {
            int hv = 1;
            MemoryStream stream = new MemoryStream(ss);
            stream.Seek(0, SeekOrigin.Begin);
            object result = UnSerialize(stream, new Hashtable(), ref hv, new Hashtable(), encoding);
            stream.Close();
            if (type != null && result != null)
            {
                result = ChangeType(result, type);
            }
            return result;
        }
        private static object UnSerialize(MemoryStream stream, Hashtable ht, ref int hv, Hashtable rt, Encoding encoding)
        {
            switch (stream.ReadByte())
            {
                case __N: return ht[hv++] = ReadNull(stream);
                case __b: return ht[hv++] = ReadBoolean(stream);
                case __i: return ht[hv++] = ReadInteger(stream);
                case __d: return ht[hv++] = ReadDouble(stream);
                case __s: return ht[hv++] = ReadString(stream, encoding);
                case __U: return ht[hv++] = ReadUnicodeString(stream);
                case __r: return ht[hv++] = ReadRef(stream, ht, rt);
                case __a: return ReadArray(stream, ht, ref hv, rt, encoding);
                case __O: return ReadObject(stream, ht, ref hv, rt, encoding);
                case __C: return ReadCustomObject(stream, ht, ref hv, encoding);
                case __R: return ReadRef(stream, ht, rt);
                default: ThrowError("Wrong Serialize Stream!"); return null;
            }
        }
        private static string ReadNumber(MemoryStream stream)
        {
            StringBuilder sb = new StringBuilder();
            int i = stream.ReadByte();
            while (i != __Semicolon && i != __Colon)
            {
                sb.Append((char)i);
                i = stream.ReadByte();
            }
            return sb.ToString();
        }
        private static object ReadNull(MemoryStream stream)
        {
            stream.Seek(1, SeekOrigin.Current);
            return null;
        }
        private static bool ReadBoolean(MemoryStream stream)
        {
            stream.Seek(1, SeekOrigin.Current);
            bool b = (stream.ReadByte() == __1);
            stream.Seek(1, SeekOrigin.Current);
            return b;
        }
        private static object ReadInteger(MemoryStream stream)
        {
            stream.Seek(1, SeekOrigin.Current);
            string i = ReadNumber(stream);
            bool neg = i.StartsWith("-");
            try
            {
                return (neg ? (object)SByte.Parse(i) : (object)Byte.Parse(i));
            }
            catch
            {
                try
                {
                    return (neg ? (object)Int16.Parse(i) : (object)UInt16.Parse(i));
                }
                catch
                {
                    return Int32.Parse(i);
                }
            }
        }
        private static object ReadDouble(MemoryStream stream)
        {
            stream.Seek(1, SeekOrigin.Current);
            string d = ReadNumber(stream);
            if (d == "NAN") return Double.NaN;
            if (d == "INF") return Double.PositiveInfinity;
            if (d == "-INF") return Double.NegativeInfinity;
            bool neg = d.StartsWith("-");
            try
            {
                return UInt32.Parse(d);
            }
            catch
            {
                try
                {
                    return (neg ? (object)Int64.Parse(d) : (object)UInt64.Parse(d));
                }
                catch
                {
                    try
                    {
                        return Decimal.Parse(d);
                    }
                    catch
                    {
                        try
                        {
                            return Single.Parse(d);
                        }
                        catch
                        {
                            return Double.Parse(d);
                        }
                    }
                }
            }
        }
        private static string ReadString(MemoryStream stream, Encoding encoding)
        {
            stream.Seek(1, SeekOrigin.Current);
            int len = Int32.Parse(ReadNumber(stream));
            stream.Seek(1, SeekOrigin.Current);
            byte[] buf = new byte[len];
            stream.Read(buf, 0, len);
            string s = encoding.GetString(buf);
            stream.Seek(2, SeekOrigin.Current);
            return s;
        }
        private static string ReadUnicodeString(MemoryStream stream)
        {
            stream.Seek(1, SeekOrigin.Current);
            int l = Int32.Parse(ReadNumber(stream));
            stream.Seek(1, SeekOrigin.Current);
            StringBuilder sb = new StringBuilder(l);
            int c;
            for (int i = 0; i < l; i++)
            {
                if ((c = stream.ReadByte()) == __Slash)
                {
                    char c1 = (char)stream.ReadByte();
                    char c2 = (char)stream.ReadByte();
                    char c3 = (char)stream.ReadByte();
                    char c4 = (char)stream.ReadByte();
                    sb.Append((char)Int32.Parse(String.Concat(c1, c2, c3, c4), System.Globalization.NumberStyles.HexNumber));
                }
                else
                {
                    sb.Append((char)c);
                }
            }
            stream.Seek(2, SeekOrigin.Current);
            return sb.ToString();
        }
        private static object ReadRef(MemoryStream stream, Hashtable ht, Hashtable rt)
        {
            stream.Seek(1, SeekOrigin.Current);
            int r = Int32.Parse(ReadNumber(stream));
            if (rt.ContainsKey(r))
            {
                rt[r] = true;
            }
            return ht[r];
        }
        private static object ReadArray(MemoryStream stream, Hashtable ht, ref int hv, Hashtable rt, Encoding encoding)
        {
            stream.Seek(1, SeekOrigin.Current);
            int n = Int32.Parse(ReadNumber(stream));
            stream.Seek(1, SeekOrigin.Current);
            Hashtable h = new Hashtable(n);
            ArrayList al = new ArrayList(n);
            int r = hv;
            rt.Add(r, false);
            long p = stream.Position;
            ht[hv++] = h;
            for (int i = 0; i < n; i++)
            {
                object key, value;
                switch (stream.ReadByte())
                {
                    case __i: key = Convert.ToInt32(ReadInteger(stream)); break;
                    case __s: key = ReadString(stream, encoding); break;
                    case __U: key = ReadUnicodeString(stream); break;
                    default: ThrowError("Wrong Serialize Stream!"); return null;
                }
                value = UnSerialize(stream, ht, ref hv, rt, encoding);
                if (al != null)
                {
                    if ((key is Int32) && (int)key == i)
                    {
                        al.Add(value);
                    }
                    else
                    {
                        al = null;
                    }
                }
                h[key] = value;
            }
            if (al != null)
            {
                ht[r] = al;
                if ((bool)rt[r])
                {
                    hv = r + 1;
                    stream.Position = p;
                    for (int i = 0; i < n; i++)
                    {
                        int key;
                        switch (stream.ReadByte())
                        {
                            case __i: key = Convert.ToInt32(ReadInteger(stream)); break;
                            default: ThrowError("Wrong Serialize Stream!"); return null;
                        }
                        al[key] = UnSerialize(stream, ht, ref hv, rt, encoding);
                    }
                }
            }
            rt.Remove(r);
            stream.Seek(1, SeekOrigin.Current);
            return ht[r];
        }
        private static object ReadObject(MemoryStream stream, Hashtable ht, ref int hv, Hashtable rt, Encoding encoding)
        {
            stream.Seek(1, SeekOrigin.Current);
            int len = Int32.Parse(ReadNumber(stream));
            stream.Seek(1, SeekOrigin.Current);
            byte[] buf = new byte[len];
            stream.Read(buf, 0, len);
            string cn = encoding.GetString(buf);
            stream.Seek(2, SeekOrigin.Current);
            int n = Int32.Parse(ReadNumber(stream));
            stream.Seek(1, SeekOrigin.Current);
            Type type = GetType(cn);
            object o;
            if (type != null)
            {
                try
                {
                    o = CreateInstance(type);
                }
                catch
                {
                    o = new Hashtable(n);
                }
            }
            else
            {
                o = new Hashtable(n);
            }
            ht[hv++] = o;
            for (int i = 0; i < n; i++)
            {
                string key;
                switch (stream.ReadByte())
                {
                    case __s: key = ReadString(stream, encoding); break;
                    case __U: key = ReadUnicodeString(stream); break;
                    default: ThrowError("Wrong Serialize Stream!"); return null;
                }
                if (key.Substring(0, 1) == "\0")
                {
                    key = key.Substring(key.IndexOf("\0", 1) + 1);
                }
                if (o is Hashtable)
                {
                    ((Hashtable)o)[key] = UnSerialize(stream, ht, ref hv, rt, encoding);
                }
                else
                {
                    type.InvokeMember(key, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetField, null, o, new object[] { UnSerialize(stream, ht, ref hv, rt, encoding) });
                }
            }
            stream.Seek(1, SeekOrigin.Current);
            MethodInfo __wakeup = null;
            try
            {
                __wakeup = o.GetType().GetMethod("__wakeup", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase, null, new Type[0], new ParameterModifier[0]);
            }
            catch { }
            if (__wakeup != null)
            {
                __wakeup.Invoke(o, null);
            }
            return o;
        }
        private static object ReadCustomObject(MemoryStream stream, Hashtable ht, ref int hv, Encoding encoding)
        {
            stream.Seek(1, SeekOrigin.Current);
            int len = Int32.Parse(ReadNumber(stream));
            stream.Seek(1, SeekOrigin.Current);
            byte[] buf = new byte[len];
            stream.Read(buf, 0, len);
            string cn = encoding.GetString(buf);
            stream.Seek(2, SeekOrigin.Current);
            int n = Int32.Parse(ReadNumber(stream));
            stream.Seek(1, SeekOrigin.Current);
            Type type = GetType(cn);
            object o;
            if (type != null)
            {
                o = CreateInstance(type);
            }
            else
            {
                ThrowError(String.Concat("Type ", cn, " is undefined!"));
                return null;
            }
            ht[hv++] = o;
            if (o is Serializable)
            {
                byte[] b = new byte[n];
                stream.Read(b, 0, n);
                ((Serializable)o).UnSerialize(b);
            }
            else
            {
                stream.Seek(n, SeekOrigin.Current);
            }
            stream.Seek(1, SeekOrigin.Current);
            return o;
        }
        private static void ThrowError(string message)
        {
            throw new UnSerializeException(message);
        }
        private static Type GetType(string typeName)
        {
            Type type = Type.GetType(typeName, false, true);
            if (type != null) return type;
            foreach (DictionaryEntry e in __ns)
            {
                type = Type.GetType(String.Concat((string)e.Key, ".", typeName, ", ", (string)e.Value), false, true);
                if (type != null) return type;
            }
            return null;
        }
    }
    
}
