using System.IO;
using Nautilus.Util;
using DtxCS.DataTypes;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Nautilus.LibForge.SongData
{
    public class SongDataReader : ReaderBase<SongData>
    {
        public static SongData ReadStream(Stream s)
        {
            return new SongDataReader(s).Read();
        }
        public SongDataReader(Stream s) : base(s) { }

        public override SongData Read()
        {
            return new SongData
            {
                Type = UInt(),
                SongId = UInt(),
                Version = Short(),
                GameOrigin = String(18),
                PreviewStart = Float(),
                PreviewEnd = Float(),
                Name = String(256),
                Artist = String(256),
                AlbumName = String(256),
                AlbumTrackNumber = Short().Then(Skip(2)),
                AlbumYear = Int(),
                OriginalYear = Int(),
                Genre = String(64),
                SongLength = Float(),
                GuitarRank = Float(),
                BassRank = Float(),
                VocalsRank = Float(),
                DrumRank = Float(),
                BandRank = Float(),
                KeysRank = Float(),
                RealKeysRank = Float(),
                Tutorial = Bool(),
                AlbumArt = Bool(),
                Cover = Bool(),
                VocalGender = Byte(),
                Medium = String(16),
                HasFreestyleVocals = Bool().Then(Skip(3)),
                VocalParts = Int(),
                Flags = Int(),
                Fake = Bool(),
                Shortname = String(256)
            };
        }
    }

    public class SongData
    {
        public uint Type;
        public uint SongId;
        public short Version;
        public string GameOrigin;
        public float PreviewStart;
        public float PreviewEnd;
        public string Name;
        public string Artist;
        public string AlbumName;
        public short AlbumTrackNumber;
        public int AlbumYear;
        public int OriginalYear;
        public string Genre;
        public float SongLength;
        public float GuitarRank;
        public float BassRank;
        public float VocalsRank;
        public float DrumRank;
        public float BandRank;
        public float KeysRank;
        public float RealKeysRank;
        public bool Tutorial;
        public bool AlbumArt;
        public bool Cover;
        public byte VocalGender;
        public string Medium;
        public bool HasFreestyleVocals;
        public int VocalParts;
        public int Flags;
        public bool Fake;
        public string Shortname;
    }

    public class SongDataConverter
    {
        public static SongData ToSongData(DataArray songDta)
        {
            var songId = songDta.Array("song_id");
            var art = songDta.Array("album_art").Any(1);
            var shortName = songDta.Array("song").Array("name").Any(1).Split('/').Reverse().First();
            var songIdNum = (shortName.GetHashCode() & 0xFFFFFF) + 90000000;
            return new SongData
            {
                AlbumArt = art == "1" || art == "TRUE",
                AlbumName = songDta.Array("album_name")?.String(1) ?? "",
                AlbumTrackNumber = (short)(songDta.Array("album_track_number")?.Int(1) ?? 0),
                AlbumYear = songDta.Array("year_released")?.Int(1) ?? 0,
                Artist = songDta.Array("artist").String(1),
                BandRank = songDta.Array("rank").Array("band").Int(1),
                BassRank = songDta.Array("rank").Array("bass").Int(1),
                DrumRank = songDta.Array("rank").Array("drum").Int(1),
                GuitarRank = songDta.Array("rank").Array("guitar").Int(1),
                KeysRank = songDta.Array("rank").Array("keys")?.Int(1) ?? 0,
                RealKeysRank = songDta.Array("rank").Array("real_keys")?.Int(1) ?? 0,
                VocalsRank = songDta.Array("rank").Array("vocals").Int(1),
                Cover = false,
                Fake = false,
                Flags = 0,
                GameOrigin = songDta.Array("game_origin")?.Any(1) ?? "ugc_plus",
                Genre = songDta.Array("genre").Symbol(1).ToString(),
                HasFreestyleVocals = false,
                Medium = "",
                Name = songDta.Array("name").String(1),
                OriginalYear = songDta.Array("year_released").Int(1),
                Tutorial = false,
                Type = 11,
                Version = -1,
                VocalGender = (byte)((songDta.Array("vocal_gender")?.Any(1) ?? "male") == "male" ? 1 : 2),
                VocalParts = songDta.Array("song").Array("vocal_parts")?.Int(1) ?? 1,
                Shortname = shortName.ToLowerInvariant(),
                SongId = (uint)songIdNum,
                SongLength = songDta.Array("song_length").Int(1),
                PreviewStart = songDta.Array("preview").Int(1),
                PreviewEnd = songDta.Array("preview").Int(2),
            };
        }
    }
}

namespace DtxCS.DataTypes
{
    /// <summary>
    /// Represents the possible types of values in a DataArray.
    /// </summary>
    public enum DataType : int
    {
        /// <summary>
        /// Integral value.
        /// </summary>
        INT = 0x00,
        /// <summary>
        /// Floating point value.
        /// </summary>
        FLOAT = 0x01,
        /// <summary>
        /// $-prefixed variable type
        /// </summary>
        VARIABLE = 0x02,
        /// <summary>
        /// Symbol value.
        /// </summary>
        SYMBOL = 0x05,
        /// <summary>
        /// '()
        /// </summary>
        EMPTY = 0x06,
        /// <summary>
        /// #ifdef directive
        /// </summary>
        IFDEF = 0x07,
        /// <summary>
        /// #else directive
        /// </summary>
        ELSE = 0x08,
        /// <summary>
        /// #endif directive
        /// </summary>
        ENDIF = 0x09,
        /// <summary>
        /// Array.
        /// </summary>
        ARRAY = 0x10,
        /// <summary>
        /// {}-enclosed function call
        /// </summary>
        COMMAND = 0x11,
        /// <summary>
        /// ""-enclosed String value.
        /// </summary>
        STRING = 0x12,
        /// <summary>
        /// []-enclosed macro definition
        /// </summary>
        MACRO = 0x13,
        /// <summary>
        /// #define directive
        /// </summary>
        DEFINE = 0x20,
        /// <summary>
        /// #include directive
        /// </summary>
        INCLUDE = 0x21,
        /// <summary>
        /// #merge directive
        /// </summary>
        MERGE = 0x22,
        /// <summary>
        /// #ifndef directive
        /// </summary>
        IFNDEF = 0x23,
        /// <summary>
        /// #autorun directive
        /// </summary>
        AUTORUN = 0x24,
        /// <summary>
        /// #undef directive
        /// </summary>
        UNDEF = 0x25
    };

    /// <summary>
    /// Represents the basic element of DTA, which could be an Atom or an Array.
    /// </summary>
    public abstract class DataNode
    {
        /// <summary>
        /// The parent of this data node. If null, this is a root node.
        /// </summary>
        public DataArray Parent { get; set; }

        /// <summary>
        /// The name of this data node.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The type of this data node.
        /// </summary>
        public abstract DataType Type { get; }

        /// <summary>
        /// Evaluate this node to get a value.
        /// </summary>
        /// <returns></returns>
        public abstract DataNode Evaluate();

        public override bool Equals(object obj)
        {
            if (!(obj is DataNode)) return false;
            if ((obj as DataNode).Type != this.Type) return false;
            if (obj.ToString() != this.ToString()) return false;
            return true;
        }

        public virtual string ToString(int depth) => ToString();

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

    /// <summary>
    /// Represents an array of DataNodes.
    /// </summary>
    public class DataArray : DataNode
    {
        public virtual char ClosingChar => ')';

        /// <summary>
        /// Returns DataType.ARRAY
        /// </summary>
        public override DataType Type => DataType.ARRAY;

        /// <summary>
        /// The children of this array.
        /// </summary>
        public List<DataNode> Children { get; }

        /// <summary>
        /// Default constructor for a Data Array.
        /// </summary>
        public DataArray()
        {
            this.Children = new List<DataNode>();
        }

        /// <summary>
        /// Add a node to this Array.
        /// </summary>
        /// <param name="node">Node to add</param>
        /// <returns>The added node.</returns>
        public T AddNode<T>(T node) where T : DataNode
        {
            Children.Add(node);
            node.Parent = this;
            return node;
        }

        /// <summary>
        /// Get or set the child of this array at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataNode this[int index]
        {
            get { return Children[index]; }
            set { Children[index] = value; }
        }

        /// <summary>
        /// Find the array in this array's children at the given index.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public DataArray Array(int idx)
        {
            if (Children[idx].Type == DataType.ARRAY)
            {
                return (DataArray)Children[idx];
            }
            throw new Exception("Element at index " + idx + " is not an Array. It is " + Children[idx].GetType().Name);
        }

        /// <summary>
        /// Find the integer in this array's children at the given index.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public int Int(int idx)
        {
            if (Children[idx].Type == DataType.INT)
            {
                return ((DataAtom)Children[idx]).Int;
            }
            throw new Exception("Element at index " + idx + " is not an integer.");
        }

        /// <summary>
        /// Find the float in this array's children at the given index.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public float Float(int idx)
        {
            if (Children[idx].Type == DataType.FLOAT)
            {
                return ((DataAtom)Children[idx]).Float;
            }
            throw new Exception("Element at index " + idx + " is not a float.");
        }

        public float Number(int idx)
        {
            if (Children[idx].Type == DataType.FLOAT)
            {
                return ((DataAtom)Children[idx]).Float;
            }
            else if (Children[idx].Type == DataType.INT)
            {
                return ((DataAtom)Children[idx]).Int;
            }
            throw new Exception("Element at index " + idx + " is not a number.");
        }

        public DataNode Node(int idx)
        {
            return Children[idx];
        }

        /// <summary>
        /// Find the string in this array's children at the given index.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public string String(int idx)
        {
            if (Children[idx].Type == DataType.STRING)
            {
                return ((DataAtom)Children[idx]).String;
            }
            throw new Exception("Element at index " + idx + " is not a string.");
        }

        /// <summary>
        /// Find the symbol in this array's children at the given index.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public DataSymbol Symbol(int idx)
        {
            if (Children[idx].Type == DataType.SYMBOL)
            {
                return (DataSymbol)Children[idx];
            }
            throw new Exception("Element at index " + idx + " is not a symbol.");
        }

        /// <summary>
        /// Find the variable in this array's children at the given index.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public DataVariable Var(int idx)
        {
            if (Children[idx].Type == DataType.VARIABLE)
            {
                return (DataVariable)Children[idx];
            }
            throw new Exception("Element at index " + idx + " is not a variable.");
        }

        /// <summary>
        /// Finds any node at the given index and returns it as a string.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public string Any(int idx)
        {
            return Children[idx].Name;
        }

        /// <summary>
        /// Find the first array in this array's children whose name matches.
        /// If none is found, returns null.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataArray Array(string name)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (!(Children[i] is DataArray)) continue;
                if (Children[i].Name == name)
                {
                    return ((DataArray)Children[i]);
                }
            }
            return null;
        }

        public override DataNode Evaluate()
        {
            var returnArray = new DataArray();
            foreach (var node in Children)
            {
                returnArray.AddNode(node.Evaluate());
            }
            return returnArray;
        }

        /// <summary>
        /// The string representation of the first element of the array, unless
        /// that element is another array, which would result in an empty string.
        /// </summary>
        public override string Name => Children[0].Type == DataType.ARRAY ?
                                 "" : Children[0].Name;

        /// <summary>
        /// The number of elements in this array.
        /// </summary>
        public int Count => Children.Count;

        /// <summary>
        /// The string representation of this Array, suitable for putting right
        /// back into a .dta file.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToString(0);

        public override string ToString(int depth)
        {
            string ret = new string(' ', depth * 3) + "(";
            for (int i = 0; i < Children.Count; i++)
            {
                var n = Children[i];
                ret += n is DataArray ? Environment.NewLine + n.ToString(depth + 1) : n.ToString(depth + 1);
                if (i + 1 != Children.Count) ret += " ";
            }
            ret += ")";
            return ret;
        }
    }

    public class DataCommand : DataArray
    {
        public override DataType Type => DataType.COMMAND;
        public override char ClosingChar => '}';

        public DataCommand() : base()
        {
        }

        public override string ToString() => ToString(0);

        public override string ToString(int depth)
        {
            string ret = new string(' ', depth * 3) + "{";
            foreach (DataNode n in Children)
            {
                ret += (n is DataArray ? Environment.NewLine + n.ToString(depth + 1) : n.ToString(depth + 1) + " ");
            }
            ret += "}";
            return ret;
        }

        /// <summary>
        /// Returns a copy of this datacommand with all children evaulated.
        /// </summary>
        public DataCommand EvalAll()
        {
            var ret = new DataCommand();
            for (var i = 0; i < Children.Count; i++)
            {
                ret.AddNode(Children[i].Evaluate());
            }
            return ret;
        }

        public override DataNode Evaluate()
        {
            Func<DataCommand, DataNode> f;
            if (Builtins.Funcs.TryGetValue(Symbol(0), out f))
            {
                return f.Invoke(this);
            }
            throw new Exception($"Func '{Any(0)}' is not defined.");
        }
    }

    // e.g., [RED 1 0 0] in a file, then (color RED) -> (color 1 0 0)
    // or, [TRUE 1] -> (happy TRUE) -> (happy 1)
    public class DataMacroDefinition : DataArray
    {
        public override DataType Type => DataType.MACRO;
        public override char ClosingChar => ']';
        public DataMacroDefinition() : base()
        {

        }
        public override string ToString() => ToString(0);

        public override DataNode Evaluate() => this;
        public override string ToString(int depth)
        {
            string ret = new string(' ', depth * 3) + "[";
            foreach (DataNode n in Children)
            {
                ret += (n is DataArray ? Environment.NewLine + n.ToString(depth + 1) : n.ToString(depth + 1) + " ");
            }
            ret += "]";
            return ret;
        }
    }

    /// <summary>
    /// The most basic element of dta.
    /// </summary>
    public class DataAtom : DataNode
    {
        DataType type;
        string sData;
        int iData;
        float fData;

        /// <summary>
        /// The type of this Atom.
        /// </summary>
        public override DataType Type => type;

        public override DataNode Evaluate() => this;

        /// <summary>
        /// The value of this Atom.
        /// </summary>
        public int Int
        {
            get
            {
                if (type == DataType.INT)
                    return iData;
                else throw new Exception("Data is not int");
            }
        }

        /// <summary>
        /// The value of this Atom.
        /// </summary>
        public float Float
        {
            get
            {
                if (type == DataType.FLOAT)
                    return fData;
                else throw new Exception("Data is not float");
            }
        }

        /// <summary>
        /// The value of this Atom.
        /// </summary>
        public string String
        {
            get
            {
                if (type == DataType.STRING)
                    return sData;
                else throw new Exception("Data is not string");
            }
        }

        /// <summary>
        /// Construct an Atom whose value is a string or symbol.
        /// </summary>
        /// <param name="data">The value assigned to this atom.</param>
        public DataAtom(string data)
        {
            type = DataType.STRING;
            sData = data.Replace("\\q", "\"");
        }

        /// <summary>
        /// Construct an Atom whose value is an integer.
        /// </summary>
        /// <param name="data"></param>
        public DataAtom(int data)
        {
            type = DataType.INT;
            iData = data;
        }

        /// <summary>
        /// Construct an Atom whose value is a floating-point value.
        /// </summary>
        /// <param name="data"></param>
        public DataAtom(float data)
        {
            type = DataType.FLOAT;
            fData = data;
        }

        /// <summary>
        /// The string representation of this Atom.
        /// </summary>
        public override string Name => ToString(true);

        private string ToString(bool name)
        {
            string ret = "";
            switch (type)
            {
                case DataType.STRING:
                    ret += name ? sData : "\"" + sData + "\"";
                    break;
                case DataType.INT:
                    ret += iData.ToString();
                    break;
                case DataType.FLOAT:
                    // Even though the format string uses a dot, it gets changed to a comma on some locales
                    // unless you give ToString the invariant culture.
                    ret += fData.ToString("0.0#", System.Globalization.NumberFormatInfo.InvariantInfo);
                    break;
            }
            return ret;
        }

        /// <summary>
        /// Returns the string representation of this Atom.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToString(false);
    }

    public class DataVariable : DataNode
    {
        static Dictionary<string, DataVariable> vars = new Dictionary<string, DataVariable>();

        public override string Name { get; }
        public override string ToString() => Name;

        public override DataType Type => DataType.VARIABLE;

        public DataNode Value { get; set; }

        public override DataNode Evaluate() => Value;

        public static DataVariable Var(string name)
        {
            DataVariable ret;
            if (!vars.TryGetValue(name, out ret))
                vars.Add(name, ret = new DataVariable(name, new DataAtom(0)));
            return ret;
        }

        /// <summary>
        /// Makes a data variable. Scoping not implemented because we're not an interpreter.
        /// Don't give this the $.
        /// </summary>
        /// <param name="name"></param>
        private DataVariable(string name, DataNode value)
        {
            Name = "$" + name;
            Value = value;
        }
    }

    public class DataSymbol : DataNode
    {
        static Dictionary<string, DataSymbol> symbols = new Dictionary<string, DataSymbol>();

        public static DataSymbol Symbol(string value)
        {
            DataSymbol ret;
            if (!symbols.TryGetValue(value, out ret))
                symbols.Add(value, ret = new DataSymbol(value));
            return ret;
        }

        public override string Name => value;
        public override DataNode Evaluate() => this;

        public override DataType Type => DataType.SYMBOL;

        private string value;
        private bool quote;

        private DataSymbol(string value)
        {
            this.value = value;
            foreach (var c in value)
            {
                // TODO: Is this right?
                if (c == ' ' || c == '\r' || c == '\n' || c == '\t'
                  || c == '(' || c == ')' || c == '{' || c == '}'
                  || c == '[' || c == ']')
                {
                    quote = true;
                    break;
                }
            }
        }

        public override string ToString() => quote ? $"'{Name}'" : Name;
    }

    /// <summary>
    /// Builtin functions, in case I end up writing an interpreter.
    /// </summary>
    static class Builtins
    {
        public static Dictionary<DataSymbol, Func<DataCommand, DataNode>> Funcs { get; }

        static Builtins()
        {
            Funcs = new Dictionary<DataSymbol, Func<DataCommand, DataNode>>();
            Funcs.Add(DataSymbol.Symbol("abs"), Abs);
            Funcs.Add(DataSymbol.Symbol("+"), Add);
            Funcs.Add(DataSymbol.Symbol("+="), AddEq);
            Funcs.Add(DataSymbol.Symbol("&"), BitAnd);
            Funcs.Add(DataSymbol.Symbol("append_str"), AppendStr);
            Funcs.Add(DataSymbol.Symbol("assign"), Assign);
            Funcs.Add(DataSymbol.Symbol("clamp"), Clamp);
            Funcs.Add(DataSymbol.Symbol("--"), Dec);
            Funcs.Add(DataSymbol.Symbol("/"), Divide);
            Funcs.Add(DataSymbol.Symbol("="), Eq);
            Funcs.Add(DataSymbol.Symbol("++"), Inc);
            Funcs.Add(DataSymbol.Symbol(">"), Gt);
            Funcs.Add(DataSymbol.Symbol("<"), Lt);
            Funcs.Add(DataSymbol.Symbol("-"), Subtract);
            Funcs.Add(DataSymbol.Symbol("if"), If);
        }
        static DataNode Abs(DataCommand input)
        {
            var args = input.EvalAll();
            return new DataAtom(Math.Abs(args.Number(1)));
        }
        static DataNode Add(DataCommand input)
        {
            var args = input.EvalAll();
            if (args.Node(1).Type == DataType.INT && args.Node(2).Type == DataType.INT)
            {
                return new DataAtom(args.Int(1) + args.Int(2));
            }
            return new DataAtom(args.Number(1) + args.Number(2));
        }
        static DataNode AddEq(DataCommand input)
        {
            var args = input.EvalAll();
            return new DataAtom(args.Number(1) + args.Number(2));
        }
        static DataNode BitAnd(DataCommand input)
        {
            var args = input.EvalAll();
            return new DataAtom(args.Int(1) & args.Int(2));
        }
        static DataNode AppendStr(DataCommand input)
        {
            var args = input.EvalAll();
            return new DataAtom(args.String(1) + args.String(2));
        }
        static DataNode Assign(DataCommand input)
        {
            input.Var(1).Value = input.Node(2).Evaluate();
            return input.Var(1).Value;
        }
        static DataNode Clamp(DataCommand input)
        {
            var args = input.EvalAll();
            float f1 = args.Number(1), f2 = args.Number(2), f3 = args.Number(3);
            return new DataAtom(f1 > f3 ? f3 : f1 < f2 ? f1 : f2);
        }
        static DataNode Dec(DataCommand input)
        {
            DataAtom a = input.Var(1).Value as DataAtom;
            input.Var(1).Value = new DataAtom(a.Int - 1);
            return input.Var(1).Value;
        }
        static DataNode Divide(DataCommand input)
        {
            var args = input.EvalAll();
            return new DataAtom(args.Number(1) / args.Number(2));
        }
        static DataNode Eq(DataCommand input)
        {
            var args = input.EvalAll();
            return new DataAtom(args.Node(1) == args.Node(2) ? 1 : 0);
        }
        static DataNode Gt(DataCommand input)
        {
            var args = input.EvalAll();
            return new DataAtom(args.Number(1) > args.Number(2) ? 1 : 0);
        }
        static DataNode Inc(DataCommand input)
        {
            DataAtom a = input.Var(1).Value as DataAtom;
            input.Var(1).Value = new DataAtom(a.Int + 1);
            return input.Var(1).Value;
        }
        static DataNode Lt(DataCommand input)
        {
            var args = input.EvalAll();
            return new DataAtom(args.Number(1) < args.Number(2) ? 1 : 0);
        }
        static DataNode Subtract(DataCommand input)
        {
            var args = input.EvalAll();
            if (args.Node(1).Type == DataType.INT && args.Node(2).Type == DataType.INT)
            {
                return new DataAtom(args.Int(1) - args.Int(2));
            }
            return new DataAtom(args.Number(1) - args.Number(2));
        }

        static DataNode If(DataCommand input)
        {
            if ((input.Children[1].Evaluate() as DataAtom).Int == 0)
            {
                return input.Children[3].Evaluate();
            }
            return input.Children[2].Evaluate();
        }
    }
}