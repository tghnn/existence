//for Them, who my death and my life
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace shard0
{
    public class FinishException : System.Exception
    {
        public FinishException()
        {
        }

        public FinishException(string message)
            : base(message)
        {
        }
    }
    public class Exps_n
    {
        public SortedDictionary<Complex, Complex> data;
        public Complex val;
        public Exps_n(Complex v)
        {
            data = new SortedDictionary<Complex, Complex>();
            val = v;
            data.Add(new Complex(0), new Complex(1));
            data.Add(new Complex(1), v);
        }
        public void save(BinaryWriter file)
        {
            val.save(file);
            file.Write((Int32)(data.Count - 2));
            foreach (KeyValuePair<Complex, Complex> v in data.Where(v => ((!v.Key.isint(0, 0)) && (!v.Key.isint(1, 0)))))
            {
                v.Key.save(file);
                v.Value.save(file);
            }
        }
        public static Exps_n load(BinaryReader file)
        {
            Exps_n ret = new Exps_n(Complex.load(file));
            Complex k, v;
            int i = 0, cnt = file.ReadInt32();
            while (i < cnt)
            {
                k = Complex.load(file);
                v = Complex.load(file);
                ret.data.Add(k, v);
                i++;
            }
            return ret;
        }


        public Complex exp(Complex e)
        {
            if (!data.ContainsKey(e))
            {
                Complex v = new Complex(val); v.exp(e);
                data.Add(e, v);
            }
            return data[e];
        }
        public Complex exp()
        {
            return val;
        }
    }

    public class Vals
    {
        public static int _ind;
        public static Vals[] inds;
        public Int32 ind;
        public Exps_n val;
        public Vars var;
        public int deep;
        public Vals(Vars vr, Exps_n vl, int d)
        {
            val = vl; var = vr; deep = d; ind = Vals._ind++;
            if (Vals.inds.Length <= ind) Array.Resize<Vals>(ref Vals.inds, Vals.inds.Length + 100);
            Vals.inds[ind] = this;
        }
        public static void saves(BinaryWriter file)
        {
            file.Write((Int32)(Vals._ind));
            int i = 0; while (i < Vals._ind)
            {
                file.Write((Int32)(Vals.inds[i].var.ind));
                Vals.inds[i].val.save(file);
                file.Write((Int16)(Vals.inds[i].deep));
                i++;
            }
        }
        public static void loads(BinaryReader file)
        {
            int i = 0, cnt = file.ReadInt32();
            Vals._ind = 0;
            int[] vnum = new int[Vars._ind];
            int vr;
            while (i < cnt)
            {
                vr = file.ReadInt32();
                Vars.inds[vr].vals[vnum[vr]++] = new Vals(Vars.inds[vr], Exps_n.load(file), file.ReadInt16());
                i++;
            }
        }
        public static Vals load(BinaryReader file)
        {
            Vals ret = null;
            int n = file.ReadInt32();
            if (n >= 0) ret = Vals.inds[n];
            return ret;
        }

        public static void save(BinaryWriter file, Vals v)
        {
            file.Write((Int32)(v == null ? -1 : v.ind));
        }
        public Exps_n get_exp()
        {
            if (var.stat != IDS.root.stat_calc)
            {
                if (var.stat == IDS.root.stat_uncalc)
                {
                    if (deep == 0) IDS.root.sys.error(var.name + " recursion");
                }
                else
                {
                    if (var.var == null) IDS.root.sys.error(var.name + " var: non is non");
                    var.stat = IDS.root.stat_uncalc;
                    var.set_now(var.var.calc());
                }
            }
            return val;
        }
        public Complex get_val(Complex e)
        {
            return get_exp().exp(e);
        }
        public Complex get_val()
        {
            return get_exp().exp();
        }
        public string get_name()
        {
            return new String('\'', deep) + var.name;
        }

    }
    public class Vars : IComparable
    {
        public static int _ind;
        public static Vars[] inds;
        public int ind;
        public Vals[] vals;
        public Func var;
        public int stat;
        public string name, desc;
        void set_ind()
        {
            ind = Vars._ind++;
            if (Vars.inds.Length <= ind) Array.Resize<Vars>(ref Vars.inds, Vars.inds.Length + 100);
            Vars.inds[ind] = this;
        }
        public Vars(string n, int valn, Complex vl)
        {
            desc = n;  name = n; stat = 0; var = null; vals = new Vals[valn + 1];
            int i = 0; while (i <= valn)
            {
                vals[i] = new Vals(this, new Exps_n(vl), i); i++;
            }
            set_ind();
        }
        public Vars(string n, Complex[] vl)
        {
            desc = n;  name = n; stat = 0; var = null; vals = new Vals[vl.Length + 1];
            int i = 0; while (i <= vl.Length)
            {
                vals[i] = new Vals(this, new Exps_n(vl[i]), i); i++;
            }
            set_ind();
        }
        public Vars(string n, int valn, int _stat)
        {
            desc = n;  name = n; stat = _stat; var = null; vals = new Vals[valn + 1];
            set_ind();
        }
        public Vars(string n, int valn, int _stat, bool isset)
        {
            name = n; stat = _stat; var = null; vals = new Vals[valn + 1];
            if (isset) set_ind();
        }
        public static int saves(BinaryWriter file)
        {
            int ret = 0;
            file.Write((Int32)(Vars._ind));
            int i = 0; while (i < Vars._ind)
            {
                file.Write(Vars.inds[i].name);
                file.Write((Int32)(Vars.inds[i].vals.Length - 1));
                file.Write((Int32)(Vars.inds[i].stat));
                file.Write(Vars.inds[i].desc);
                if (Vars.inds[i].var != null) ret++;
                i++;
            }
            return ret;
        }
        public static void loads(BinaryReader file)
        {
            int i = 0; 
            Vars._ind = file.ReadInt32();
            Array.Resize<Vars>(ref Vars.inds, Vars._ind + 11);
            while (i < Vars._ind)
            {
                Vars.inds[i] = new Vars(file.ReadString(), file.ReadInt32(), file.ReadInt32(),false);
                Vars.inds[i].desc = file.ReadString();
                i++;
            }
        }
        public void set_now(Complex v0)
        {
            if (stat != IDS.root.stat_calc)
            {
                int i = vals.Length - 1;
                while (i > 0)
                {
                    vals[i].val = vals[i - 1].val;
                    i--;
                }
                stat = IDS.root.stat_calc;
            }
            vals[0].val = new Exps_n(v0);
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Vars v = obj as Vars;
            if (ind == v.ind) return 0;
            if (ind < v.ind) return -1; else return +1;
        }
    }
    public class IDS
    {
        public const int znums = 100;
        public static Num ln_prec, n_e_full, n_e, n_pi_full, n_pi, n_ln2_full, n_ln2;
        public static IDS root;
        public static BigInteger exp_max;
        public static int prec2, prec10, digit10, sqr_steps, sqr_exp_bits, v_res, time, now_time;
        public static int[] h_to_l2;
        public static BigInteger[] e10, e2;
        public static Num[] sqr2, nums;
        public static SortedDictionary<Num, Num> ln;
        public static Func now_func = null;
        public BinaryReader fload = null;
        public Fileio sys;
        public Flow flow;
        public Parse par;
        public System.Drawing.Bitmap pic;
        public int stat_uncalc, stat_calc, n_step, steps;
        public SortedDictionary<string, Vars> var;
        public Vars v_e, v_pi, v_ln2, v_x, v_n;
        public string[] funcs_name = { "", "", "", "ln", "fact", "int", "sign", "row", "" };
        public SortedDictionary<string, int> fnames;
        public Num[] fact;
        static int _l2(int i)
        {
            int i0 = 0;
            while (i != 0) { i >>= 1; i0++; }
            return i0;
        }
        public string flag()
        {
            return pic.Width.ToString() + "," + pic.Height.ToString() + "," +
                    digit10.ToString() + "," + sqr_steps.ToString() + "," + exp_max.ToString() + "," +
                    time.ToString() + "," + n_step.ToString() + "," + steps.ToString() + ",";
        }
        public IDS(Fileio f, Parse p)
        {
            int i, sx = 0, sy = 0, exp = 0; string parm;
            sys = f; par = p; stat_uncalc = 1; stat_calc = 2;
            fnames = new SortedDictionary<string, int>();
            var = new SortedDictionary<string, Vars>();
            IDS.ln = new SortedDictionary<Num, Num>();
            IDS.nums = new Num[IDS.znums * 2];
            i = 0; while (i < IDS.znums * 2) { IDS.nums[i] = new Num(i - IDS.znums); i++; }
            Complex._0 = new Complex(0);
            Complex._r1 = new Complex(1);
            Complex._i1 = new Complex(nums[IDS.znums], nums[IDS.znums + 1]);
            Complex._r_1 = new Complex(-1);
            IDS.root = this;
            One.zero = new One();
            Func.zero = new Func(new Complex(0));
            if (sys.flag == "")
            {
                par.lnext(); parm = par.val + ",0,0,";
                File.Delete(par.sys.name + ".png");
            }
            else
            {
                if ((!File.Exists(sys.name + ".bin")) || (!File.Exists(sys.name + ".png"))) sys.error("fatal: save lost");
                parm = sys.flag;
            }
            String[] parms = parm.Split(new char[] { ',' }); if (parms.Length < 8) sys.error("wrong parms");
            if (parms[8] == "init") sys.error("fatal: init");
            if (parms[8] == "==") sys.error("fatal: time quant");
            if (!(Int32.TryParse(parms[0], out sx) && Int32.TryParse(parms[1], out sy) &&
                Int32.TryParse(parms[2], out digit10) && Int32.TryParse(parms[3], out sqr_steps) && Int32.TryParse(parms[4], out exp) &&
                Int32.TryParse(parms[5], out time) && Int32.TryParse(parms[6], out n_step) && Int32.TryParse(parms[7], out steps))) sys.error("wrong parms");
            now_time = (parms[8] == "" ? time : time / 2);
            sys.file_flag.WriteLine(parm + (sys.flag == "" ? "init" : "=")); sys.file_flag.Flush();
            if ((sx < 100) || (sx > 2000) || (sy < 100) || (sy > 2000) || (sqr_steps < 4) || (sqr_steps > 11) ||
                (exp < 11) || (exp > 6666) || (digit10 < 11) || (digit10 > 2000) ||
                (time < 6)) sys.error("wrong head");
            if (File.Exists(sys.name + ".png"))
            {
                Bitmap tb = new Bitmap(par.sys.name + ".png");
                pic = new Bitmap(tb);
                if ((sx != pic.Width) || (sy != pic.Height)) sys.error("wrong pic");
            }
            else
            {
                pic = new System.Drawing.Bitmap(sx, sy);
                for (int i0 = 0; i0 < sx; i0++) for (int i1 = 0; i1 < sy; i1++) pic.SetPixel(i0, i1, Color.FromArgb(0, 0, 0));
            }
            IDS.exp_max = new BigInteger(exp);
            BigInteger b, c;
            IDS.e10 = new BigInteger[IDS.digit10 + 2]; b = 1; i = 0;
            while (i < e10.Count())
            {
                IDS.e10[i] = b; b *= 10; i++;
            }
            IDS.e2 = new BigInteger[IDS.digit10 * 4 * 4]; b = 1; i = 0;
            while (i < e2.Count())
            {
                IDS.e2[i] = b; b *= 2; i++;
            }

            //for ^1/2: 4 = 10^380, 5 = 10^760, 6 = 10^1500; 
            //for ^x: 4 = 2^60, 5 = 2^121, 6 = 2^242 (72d), 7 = 2^500 (150d)
            sqr_exp_bits = (int)(e2[sqr_steps - 4]) * 60;
            prec2 = (sqr_exp_bits * 11) / 10;
            prec10 = (prec2 * 3) / 10;

            if (prec10 > digit10) sys.error("wrong prec");

            h_to_l2 = new int[256]; //0-7; 8-15; ..
            i = 0; while (i < h_to_l2.Length)
            {
                h_to_l2[i] = _l2(i) - 1;
                i++;
            }

            n_pi_full = new Num("3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196442881097566593344612847564823378678316527120190914564856692346034861045432664821339360726024914127372458700660631558817488152092096282925409171536436789259036001133053054882046652138414695194151160943305727036575959195309218611738193261179310511854807446237996274956735188575272489122793818301194912983367336244065664308602139494639522473719070217986094370277053921717629317675238467481846766940513200056812714526356082778577134275778960917363717872146844090122495343014654958537105079227968925892354201995611212902196086403441815981362977477130996051870721134999999837297804995105973173281609631859502445945534690830264252230825334468503526193118817101000313783875288658753320838142061717766914730359825349042875546873115956286388235378759375195778185778053217122680661300192787661119590921642019893809525720106548586327886593615338182796823030195203530185296899577362259941389124972177528347913151557485724245415069595082953311686172785588907509838175463746493931925506040092770167113900984882401285836160356370766010471018194295559619894676783744944825537977472684710404753464620804668425906949129331367702898915210475216205696602405803815019351125338243003558764024749647326391419927260426992279678235478163600934172164121992458631503028618297455570674983850549458858692699569092721079750930295532116534498720275596023648066549911988183479775356636980742654252786255181841757467289097777279380008164706001614524919217321721477235014144197356854816136115735255213347574184946843852332390739414333454776241686251898356948556209921922218427255025425688767179049460165346680498862723279178608578438382796797668145410095388378636095068006422512520511739298489608412848862694560424196528502221066118630674427862203919494504712371378696095636437191728".Substring(0, IDS.digit10 - 2));
            n_e_full = new Num("2.71828182845904523536028747135266249775724709369995957496696762772407663035354759457138217852516642742746639193200305992181741359662904357290033429526059563073813232862794349076323382988075319525101901157383418793070215408914993488416750924476146066808226480016847741185374234544243710753907774499206955170276183860626133138458300075204493382656029760673711320070932870912744374704723069697720931014169283681902551510865746377211125238978442505695369677078544996996794686445490598793163688923009879312773617821542499922957635148220826989519366803318252886939849646510582093923982948879332036250944311730123819706841614039701983767932068328237646480429531180232878250981945581530175671736133206981125099618188159304169035159888851934580727386673858942287922849989208680582574927961048419844436346324496848756023362482704197862320900216099023530436994184914631409343173814364054625315209618369088870701676839642437814059271456354906130310720851038375051011574770417189861068739696552126715468895703503540212340784981933432106817012100562788023519303322474501585390473041995777709350366041699732972508868769664035557071622684471625607988265178713419512466520103059212366771943252786753985589448969709640975459185695638023637016211204774272283648961342251644507818244235294863637214174023889344124796357437026375529444833799801612549227850925778256209262264832627793338656648162772516401910590049164499828931505660472580277863186415519565324425869829469593080191529872117255634754639644791014590409058629849679128740687050489585867174798546677575732056812884592054133405392200011378630094556068816674001698420558040336379537645203040243225661352783695117788386387443966253224985065499588623428189970773327617178392803494650143455889707194258639877275471096295374152111513683506275260232648472870392076431005958411661205452970302364725492966693811513732275364509888903136020572481765851180630364428123149655070475102544650117272115551948668508003685322818".Substring(0, IDS.digit10 - 2));
            n_ln2_full = new Num("0.69314718055994530941723212145817656807550013436025525412068000949339362196969471560586332699641868754200148102057068573368552023575813055703267075163507596193072757082837143519030703862389167347112335011536449795523912047517268157493206515552473413952588295045300709532636664265410423915781495204374043038550080194417064167151864471283996817178454695702627163106454615025720740248163777338963855069526066834113727387372292895649354702576265209885969320196505855476470330679365443254763274495125040606943814710468994650622016772042452452961268794654619316517468139267250410380254625965686914419287160829380317271436778265487756648508567407764845146443994046142260319309673540257444607030809608504748663852313818167675143866747664789088143714198549423151997354880375165861275352916610007105355824987941472950929311389715599820565439287170007218085761025236889213244971389320378439353088774825970171559107088236836275898425891853530243634214367061189236789192372314672321720534016492568727477823445353476481149418642386776774406069562657379600867076257199184734022651462837904883062033061144630073719489002743643965002580936519443041191150608094879306786515887090060520346842973619384128965255653968602219412292420757432175748909770675268711581705113700915894266547859596489065305846025866838294002283300538207400567705304678700184162404418833232798386349001563121889560650553151272199398332030751408426091479001265168243443893572472788205486271552741877243002489794540196187233980860831664811490930667519339312890431641370681397776498176974868903887789991296503619270710889264105230924783917373501229842420499568935992206602204654941510613918788574424557751020683703086661948089641218680779020818158858000168811597305618667619918739520076671921459223672060253959543654165531129517598994005600036651356756905124592682574394648316833262490180382424082423145230614096380570070255138770268178516306902551370323405380214501901537402950994226299577964742713".Substring(0, IDS.digit10 - 2));

            ln_prec = new Num((IDS.prec2 / 4) * 2 + 1); ln_prec.div();
            n_pi = new Num(IDS.n_pi_full); IDS.n_pi.prec_this();
            n_e = new Num(IDS.n_e_full); IDS.n_e.prec_this();
            n_ln2 = new Num(IDS.n_ln2_full); IDS.n_ln2.prec_this();

            v_pi = findadd_var("pi"); v_pi.var = new Func(new Complex(IDS.n_pi));
            v_e = findadd_var("e"); v_e.var = new Func(new Complex(IDS.n_e));
            v_ln2 = findadd_var("ln2"); v_ln2.var = new Func(new Complex(IDS.n_ln2));
            v_ln2 = findadd_var("i"); v_ln2.var = new Func(new Complex(Complex._i1));
            v_x = findadd_var("x"); v_n = findadd_var("n");
            v_res = 6;

            sqr2 = new Num[IDS.digit10]; //2 = (2:3)
            sqr2[0] = new Num(1);
            sqr2[1] = new Num("1.4");
            Num _n = new Num(2);
            Num _2 = new Num(2);
            Num __t;
            i = 2; while (i < IDS.sqr2.Length - 1)
            {
                __t = Num.mul(_n, IDS.sqr2[1]);
                sqr2[i++] = Num.div(Num.add(_n, __t), _2).simple();
                _n.mul(_2);
                sqr2[i++] = Num.div(Num.add(__t, _n), _2).simple();
            }
            i = 2; while (i < 8) { fnames.Add(funcs_name[i], i); i++; }
            fact = new Num[1000];
            b = new BigInteger(1); c = new BigInteger(1); fact[0] = new Num(b);
            while (b < fact.Count())
            {
                c *= b; fact[(int)b] = new Num(c); b++;
            }
            root = this;
            if (sys.flag == "")
            {
                flow = new Flow(11, now_time);
                par.init();
                save(sys.name + ".bin").Close();
                sys.flag = flag();
                sys.finish();
            }
            else
            {
                load(sys.name + ".bin");
            }
        }
        public BinaryWriter save(string name)
        {
            BinaryWriter file = new BinaryWriter(File.Open(name, FileMode.Create));
            file.Write(par.flag_out_exptomul);
            file.Write(par.flag_out_desc);
            file.Write((Int32)(par.opers));
            file.Write((Int32)(par.body_num));
            file.Write((Int32)(par.body.Count));
            foreach (string s in par.body) file.Write(s);
            int funcs = Vars.saves(file);
            Vals.saves(file);
            file.Write((Int32)funcs);
            int i = 0; while (i < Vars._ind)
            {
                if (Vars.inds[i].var != null)
                {
                    file.Write((Int32)i);
                    Vars.inds[i].var.save(file);
                }
                i++;
            }
            flow.save(file);
            file.Flush();
            return file;
        }
        public void load(string name)
        {
            fload = new BinaryReader(File.Open(name, FileMode.Open));
            par.flag_out_exptomul = fload.ReadBoolean();
            par.flag_out_desc = fload.ReadBoolean();
            par.opers = fload.ReadInt32();
            par.body_num = fload.ReadInt32();
            int i = 0, cnt = fload.ReadInt32(); while (i < cnt)
            {
                par.body.Add(fload.ReadString());
                i++;
            }
            par.body_num--; par.bnext();
            Vars.loads(fload);
            Vals.loads(fload);
            i = 0; cnt = fload.ReadInt32(); while (i < cnt)
            {
                Vars.inds[fload.ReadInt32()].var = Func.load(fload);
                i++;
            }
            var.Clear();
            i = 0; while (i < Vars._ind)
            {
                var.Add(Vars.inds[i].name, Vars.inds[i]);
                i++;
            }
            flow = Flow.load(fload, now_time);
        }
        int deep(string n)
        {
            int i = 0; while ((i < n.Length) && (n[i] == '\'')) i++;
            return i;
        }
        public Vals find_val(string n)
        {
            int i = deep(n);
            Vars f = find_var(n.Substring(i));
            if (f.vals.Length <= i) sys.error(n + " too deep");
            return f.vals[i];
        }
        public Vars find_var(string n0)
        {
            if (!var.ContainsKey(n0)) sys.error(n0 + " var not found");
            return var[n0];
        }
        public Vars find_var_fill(string n0)
        {
            if (!var.ContainsKey(n0)) sys.error(n0 + " var not found");
            if (var[n0].var == null) par.sys.error(n0 + " empty name");
            return var[n0];
        }

        public Vars findadd_var(string n)
        {
            int i = deep(n); string n0 = n.Substring(i);
            if (var.ContainsKey(n0))
            {
                if (var[n0].vals.Length != i + 1) sys.error(n0 + " wrong deep");
            }
            else
            {
                var.Add(n0, new Vars(n0, i, new Complex(0)));
            }
            return var[n0];
        }
        public void uncalc() { stat_uncalc += 2; stat_calc += 2; }
        //          val           var
        //old    uncalc,calc  uncalc,calc
        //uncalc  get           recurs
        //calc    get             get
    }

    public interface IPower
    {
        void exp2();
    }

    public abstract class Power<T> where T : IPower, new()
    {
        public abstract void set(T s);
        public abstract void set0();
        public abstract void set1();
        public abstract void copy(ref T s);
        public abstract void mul(T m);
        public abstract void div();
        public void exp2()
        {
            T m0 = new T(); copy(ref m0); mul(m0);
        }
        public void exp(BigInteger ex)
        {
            if (ex > IDS.exp_max) IDS.root.sys.error("exp: too");
            _exp((int)ex);
        }
        public void exp(int ex)
        {
            if (ex > IDS.exp_max) IDS.root.sys.error("exp: too");
            _exp(ex);
        }
        public void _exp(int ex)
        {
            int _e;
            _e = ((ex < 0) ? -ex : ex);
            if (_e == 0) { set1(); return; }
            if (_e > 1)
            {
                T t = new T(); copy(ref t);
                int i0 = _e - 1; while (i0 > 0)
                {
                    if ((i0 & 1) != 0) mul(t);
                    t.exp2();
                    i0 >>= 1;
                }
            }
            if (ex < 0) div();
        }
        public void exp(int ex, T[] lex)
        {

            int _e;
            _e = ((ex < 0) ? -ex : ex);
            if (_e == 0) { set1(); return; }
            if (_e > 1)
            {
                T t = new T(); copy(ref t);
                int i0 = _e - 1; while (i0 > 0)
                {
                    if ((i0 & 1) != 0) mul(t);





                    i0 >>= 1;
                }
            }
            if (ex < 0) div();
        }
    }

    public class Num : Power<Num>, IPower, IComparable
    {
        public BigInteger up, down;
        public int sign;
        public BigInteger get_sup() { return up * sign; }
        public Num()
        {
            init(0, 0);
        }
        public static Num load(BinaryReader file)
        {
            Num ret;
            Int16 cnt = file.ReadInt16();
            if (cnt < 0)
            {
                ret = IDS.nums[-cnt];
            }
            else
            {
                ret = new Num();
                ret.up = new BigInteger(file.ReadBytes(cnt));
                cnt = file.ReadInt16();
                ret.down = new BigInteger(file.ReadBytes(cnt));
                ret.sign = (ret.up > 0 ? 1 : 0);
                if (ret.up < 0) { ret.up = -ret.up; ret.sign = -1; }
            }
            return ret;
        }
        public void save(BinaryWriter file)
        {
            if ((down == 1) && (up < IDS.znums))
            {
                file.Write((Int16)(-(int)(up) * sign - IDS.znums));
            }
            else
            {
                byte[] tmp = (up * sign).ToByteArray();
                file.Write((Int16)(tmp.Length));
                file.Write(tmp);
                tmp = down.ToByteArray();
                file.Write((Int16)(tmp.Length));
                file.Write(tmp);
            }
        }
        public Num(Num n)
        {
            set(n);
        }
        public Num(Num n, int s)
        {
            set(n, s);
        }
        static public Num get(int r)
        {
            if (Math.Abs(r) < IDS.znums) return IDS.nums[IDS.znums + r]; else return new Num(r);
        }
        static public Num add(Num a0, Num a1, int s)
        {
            Num r = new Num(a0); r.add(a1, s);
            return r;
        }
        static public Num add(Num a0, Num a1)
        {
            Num r = new Num(a0); r.add(a1, 1);
            return r;
        }
        static public Num sub(Num s0, Num s1)
        {
            Num r = new Num(s0); r.add(s1, -1);
            return r;
        }
        static public Num neg(Num m0)
        {
            return new Num(-m0.sign, m0.up, m0.down);
        }
        static public Num _div(Num d0)
        {
            return new Num(d0.sign, d0.down, d0.up);
        }
        static public Num mul(Num m0, Num m1)
        {
            return new Num(m0.sign * m1.sign, m0.up * m1.up, m0.down * m1.down);
        }
        static public Num div(Num d0, Num d1)
        {
            return new Num(d0.sign * d1.sign, d0.up * d1.down, d0.down * d1.up);
        }
        static public Num max(Num m0, Num m1)//no new 
        {
            if (m0.great(m1)) return m1; else return m0;
        }
        static public Num min(Num m0, Num m1) //no new
        {
            if (m0.great(m1)) return m0; else return m1;
        }
        static public Num common(Num n0, Num n1)//no new
        {
            if (n0.sign != n1.sign) return IDS.nums[IDS.znums];
            if (n0.up * n1.down > n1.up * n0.down) return n1; else return n0;

        }
        static public Num extract(Num n0, Num n1)
        {
            Num ret = new Num(n0); ret.extract(n1); return ret;
        }
        public Num(BigInteger u)
        {
            set(u);
        }
        public Num(int s, BigInteger u, BigInteger d)
        {
            sign = s; up = u; down = d;
        }
        public Num(string s)
        {
            set(s);
        }
        public void set(int n)
        {
            init(n, 1);
        }
        public override void copy(ref Num n)
        {
            n.sign = sign;
            n.up = BigInteger.Abs(up);
            n.down = BigInteger.Abs(down);
        }
        public override void set(Num n)
        {
            sign = n.sign;
            up = BigInteger.Abs(n.up);
            down = BigInteger.Abs(n.down);
        }
        public void set(Num n, int s)
        {
            sign = n.sign * s;
            up = BigInteger.Abs(n.up);
            down = BigInteger.Abs(n.down);
        }
        static char[] pnt = { '.' };
        public void set(string s)
        {
            if (s.Length > IDS.e10.Length) IDS.root.sys.error("num: too long");
            string[] ss = s.Split(Num.pnt);
            if (ss.Length > 2) IDS.root.sys.error("num: parse");
            if (ss.Length == 2)
            {
                ss[0] += ss[1];
                down = BigInteger.Abs(IDS.e10[ss[1].Length]);
            }
            else down = 1;
            BigInteger.TryParse(ss[0], out up);
            sign = (up > 0 ? 1 : 0);
        }

        public void set(BigInteger _u)
        {
            if (_u < 0) { sign = -1; up = -_u; } else { sign = (_u == 0 ? 0 : 1); up = _u; }
            down = 1;
        }
        public Num(int a, int b)
        {
            init(a, b);
        }
        public Num(int a)
        {
            init(a, 1);
        }
        public override void set0()
        {
            init(0, 1);
        }
        public override void set1()
        {
            init(1, 1);
        }
        void init(int a, int b)
        {
            up = BigInteger.Abs(a);
            down = BigInteger.Abs(b);
            sign = (a < 0 ? -1 : (a > 0 ? 1 : 0));
        }
        public void neg()
        {
            sign *= -1;
        }
        public bool great(Num a)
        {
            if (sign == a.sign)
                return (a.up * down * sign > up * a.down * sign);
            else return (a.sign > 0);
        }
        public void max(Num a)
        {
            if (great(a)) set(a);
        }
        public void min(Num a)
        {
            if (a.great(this)) set(a);
        }

        public bool isint()
        {
            return (down == 1);
        }
        public static bool isint(Num n, int i)
        {
            if (n == null) return false; else return n.isint(i);
        }
        public bool isint(int n)
        {
            return (down == 1) && (sign * n >= 0) && (up == BigInteger.Abs(n));
        }
        public Num simple() //immutable
        {
            BigInteger a, _u, _d;
            if ((up > 1) && (down > 1))
            {
                bool t = false; _u = BigInteger.Abs(up); _d = BigInteger.Abs(down);
                do
                {
                    a = BigInteger.GreatestCommonDivisor(_u, _d);
                    if (a > 1) t = true; else break;
                    _u = BigInteger.Divide(_u, a);
                    _d = BigInteger.Divide(_d, a);
                } while (true);
                if (t)
                {
                    if ((_d == 1) && (_u < IDS.znums)) return IDS.nums[IDS.znums + (int)(_u) * sign];
                    else return new Num(sign, _u, _d);
                }
            }
            else if (down == 0) IDS.root.sys.error("div0");
            if (up == 0) down = 1;
            if ((down == 1) && (up < IDS.znums)) return IDS.nums[IDS.znums + (int)(up) * sign];
            else return this;
        }
        public void simple_this()
        {
            BigInteger a;
            if ((up > 1) && (down > 1))
            {
                do
                {
                    a = BigInteger.GreatestCommonDivisor(up, down);
                    if (a < 2) return;
                    up = BigInteger.Divide(up, a);
                    down = BigInteger.Divide(down, a);
                } while (true);
            }
        }
        public void common(Num n)
        {
            if (sign != n.sign) set0();
            else
            {
                if (up * n.down > n.up * down) set(n);
            }
        }
        public void extract(Num n)
        {
            if (n.sign == 0) set0();
            else
            {
                sign = ((sign < 0) && (n.sign < 0) ? -1 : 1);
                up = BigInteger.GreatestCommonDivisor(up, n.up);
                down = BigInteger.GreatestCommonDivisor(down, n.down);
            }
        }
        public override void div()
        {
            BigInteger t = up;
            up = down; down = t;
        }
        public new void exp2()
        {
            sign *= sign;
            up *= up;
            down *= down;
        }

        public void mul(int a)
        {
            if (a < 0)
            {
                sign = -sign;
                up *= -a;
            }
            else if (a > 0) up *= a;
            else set0();
        }
        public override void mul(Num a)
        {
            sign *= a.sign;
            up *= a.up;
            down *= a.down;
        }
        public void div(Num a)
        {
            sign *= a.sign;
            up *= a.down;
            down *= a.up;
        }
        public void mul(BigInteger u, BigInteger d)
        {
            up *= u; down *= d;
        }
        public void mul(Num a, int e)
        {
            if (e > 0) mul(a); else div(a);
        }
        public void add_up(BigInteger a)
        {
            up += a;
        }
        public void add(int a)
        {
            add(new Num(a), 1);
        }
        public void add(Num a, int s)
        { // 1/6 + 1/15 : (3) : (15/3 + 6/3) / (6 * (15/3))
            if (sign == 0) set(a, s);
            else
            {
                BigInteger c = BigInteger.GreatestCommonDivisor(down, a.down);
                BigInteger d = a.down / c;
                if (sign * (a.sign * s) < 0)
                {
                    up = d * up - (down / c) * a.up;
                }
                else
                {
                    up = d * up + (down / c) * a.up;
                }
                down *= d;
                if (up < 0) { up = -up; sign = -sign; } else if (up == 0) { sign = 0; down = 1; }
            }
        }
        public void add(Num a) { add(a, 1); }
        public void sub(Num a) { add(a, -1); }
        public void prec_this()
        {
            prec_this(IDS.prec2);
        }
        public void prec_this(int l)
        {
            int lu, ld; int _l;
            if ((up < 2) || (down < 2)) return;
            lu = Num._l2(up); ld = Num._l2(down);
            if (lu > ld) lu = ld;
            _l = lu - l; if (_l > l)
            {
                BigInteger _d, _au, _ad, _bu, _bd, _cu, _cd;
                _d = IDS.e2[_l];
                _au = up / _d; _bu = up % _d; _cu = _bu / _au;
                _ad = down / _d; _bd = down % _d; _cd = _bd / _ad;
                _d += (_cu + _cd) / 2;
                up /= _d; down /= _d;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _l2(BigInteger n)
        {
            if (n < 2) return 0;
            byte[] tb = n.ToByteArray();
            int _l = tb.Length - 1;
            return (_l << 3) + IDS.h_to_l2[tb[_l]];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //r - (this - r^e)/e/r^(e-1) = r - this/e/r^(e-1) + r/e
        void _sq2_n(Num to, Num fr) // (r+this/r)/2
        {
            BigInteger _d = down * fr.up;
            BigInteger _c = BigInteger.GreatestCommonDivisor(fr.down, _d);
            _d /= _c;
            to.up = _d * fr.up + (fr.down / _c) * up * fr.down;
            to.down = fr.down * _d;
            if (to.up.IsEven) to.up >>= 1; else to.down <<= 1;
            to.prec_this();
        }
        Num sq_exp(Num sq) //up > down
        {
            int i0, i1 = IDS.sqr_exp_bits;
            if (sq.down.IsEven && (sign < 0)) IDS.root.sys.error("exp: neg sqr");
            Num res = new Num(1), _sq = new Num(sq), tmp = new Num(1), _t = new Num(this); // (5/11-1/2)*2 = ((5*2-11)/(11*2))*2
            BigInteger _ud = up / down;
            if (_ud > 3)
            {
                int l2 = Num._l2(_ud);
                while (l2 > 1)
                {
                    _t._sq2_n(tmp, IDS.sqr2[l2]); i0 = IDS.sqr_steps; while (i0-- > 0) _t._sq2_n(tmp, tmp);
                    _sq.up <<= 1; if (_sq.up >= _sq.down)
                    {
                        _sq.up -= _sq.down;
                        res.mul(tmp);
                    }
                    if (_sq.up.IsZero || (i1 < 0)) return res.simple();
                    _t.up = tmp.up; _t.down = tmp.down; l2 >>= 1; i1--;
                }
            }
            while ((!_sq.up.IsZero) && (i1 > 0))
            {
                tmp.up = _t.up + _t.down; if (tmp.up.IsEven) { tmp.up >>= 1; tmp.down = BigInteger.Abs(_t.down); } else tmp.down = _t.down << 1;
                i0 = IDS.sqr_steps; while (i0-- > 0) _t._sq2_n(tmp, tmp);
                _sq.up <<= 1; if (_sq.up >= _sq.down)
                {
                    _sq.up -= _sq.down;
                    res.mul(tmp); res.prec_this();
                }
                _t.up = tmp.up; _t.down = tmp.down; i1--;
            }
            return res.simple();
        }
        public void exp(Num ex)
        {
            int se; bool dv = (down > up);
            if ((sign == 0) || isint(1) || ex.isint(1)) return;
            if (ex.sign == 0) { set1(); return; }
            Num _e = new Num(ex), r0 = (dv ? new Num(sign, down, up) : new Num(this));
            se = _e.sign; _e.sign = 1;
            BigInteger _ei = _e.toint();
            if (_ei > 0)
            {
                _e.up -= _ei * _e.down;
                Num r1 = new Num(r0);
                r0.exp(_ei);
                r0.mul(r1.sq_exp(_e));
            }
            else r0 = r0.sq_exp(_e);
            sign = r0.sign;
            if (dv ^ (se < 0)) { up = r0.down; down = r0.up; } else { up = r0.up; down = r0.down; }
        }
        public void ln()
        {
            if (sign < 1) IDS.root.sys.error("ln: not pos");
            bool dv = down > up;
            if (dv) div();
            Num r = null;
            if (IDS.ln.ContainsKey(this)) set(IDS.ln[this]);
            else
            {
                int l2 = Num._l2(toint());
                Num t = new Num(this);
                t.down *= IDS.e2[l2];
                t.simple_this(); t.prec_this();
                //x = (1+y)/(1-y); 1+y = x-xy; 1-x = -y-xy; x-1 = y(1+x); y = (x-1)/(x+1)
                Num y = Num.div(Num.sub(t, IDS.nums[IDS.znums + 1]), Num.add(t, IDS.nums[IDS.znums + 1])).simple();
                r = Num.mul(y, IDS.nums[IDS.znums + 2]);
                Num z = new Num(IDS.ln_prec);
                Num l = new Num(z);
                y = Num.mul(y, y);
                while (z.down > 1)
                {
                    z.down -= 2;
                    l = Num.add(z, Num.mul(y, l));
                }
                r.mul(l); r.simple_this(); r.prec_this();
                r.add(Num.mul(IDS.n_ln2, IDS.nums[IDS.znums + l2]));
                IDS.ln.Add(new Num(this), new Num(r)); set(r);
            }
            if (dv) sign = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        BigInteger _div(BigInteger d0, BigInteger d1)
        {
            BigInteger t;
            BigInteger r = BigInteger.DivRem(d0, d1, out t);
            if (r > 1000000000) return new BigInteger(1000000000);
            if (t * 2 > d0) r++;
            return r;
        }
        //6: +0.0000000000000000000000000000000000000000000000000000013631000934856663598194709551475972339538149682
        //6: +0.0000000000000000000000000000000000000000000000000000070006026538925265359943634345869258627656933747
        //6: +0.0000000000000000000000000000000000000000000000000000995320760599319535732067627950329184983090196596
        //n1 = [(x > n0) n0+n0/i; (n0 > x > n0/2) n0-n0/i; (n0/2 > x) n0/i]
        public Num uf(Num n)
        {
            BigInteger nu, xu, t0, i;
            xu = up * n.down; nu = n.up * down;
            if (xu > nu)
            {
                //n/(x-n) = i
                //(nu/nd)/(nd*xu/xd*nd-xd*nu/nd*xd)
                //(xd*nd*nu/nd)/(nd*xu-xd*nu)
                //(xd*nu)/(nd*xu-xd*nu)
                t0 = xu - nu; i = _div(nu, t0);
                return new Num(1, n.up * i + n.up, n.down * i);
            }
            else if (xu * 2 > nu)
            {
                //n/(n-x)
                t0 = nu - xu; i = _div(nu, t0);
                return new Num(1, n.up * i - n.up, n.down * i);
            }
            else
            {
                //n/x
                i = _div(nu, xu);
                return new Num(1, n.up, n.down * i);
            }
        }

        public BigInteger ufu(Num n, Num s)
        {
            //d = n-x; i = s/d + 1; s /= i; n1 = n-s;
            BigInteger dnx, unx, t0, i;
            dnx = down * n.down; unx = n.up * down - up * n.down;
            t0 = s.down * unx; i = s.up * dnx / t0 + 1;
            s.down *= i; n.sub(s);
            return i;
        }
        public static Num exp(Num ex, Num p)
        {
            Num r = new Num(ex); r.exp(p); return r;
        }
        public static Num exp(Num ex, int p)
        {
            Num r = new Num(ex); r.exp(p); return r;
        }

        public BigInteger toint()
        {
            return sign * up / down;
        }
        public double todouble()
        {
            Num tmp = new Num(this);
            tmp.prec_this(300);
            return (double)(sign * tmp.up) / (double)(tmp.down);
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Num k = obj as Num;
            if (sign != k.sign) return (sign < k.sign ? -1 : +1);
            BigInteger u0 = up * k.down, u1 = k.up * down;
            if (u0 == u1) return 0;
            return (u0 < u1 ? -1 : 1);
        }
    }
    public class Complex : Power<Complex>, IPower, IComparable
    {
        public static Complex _0, _r1, _i1, _r_1;
        public Num r, i;
        Num s;
        public static Complex exp(Complex ex, Complex p)
        {
            Complex rt = new Complex(ex);
            rt.exp(p);
            return rt;
        }
        public static Complex exp(Complex ex, int p)
        {
            Complex rt = new Complex(ex);
            rt.exp(p);
            return rt;
        }
        static public Complex max(Complex m0, Complex m1)//no new 
        {
            if (m0.great(m1)) return m1; else return m0;
        }
        static public Complex min(Complex m0, Complex m1) //no new
        {
            if (m0.great(m1)) return m0; else return m1;
        }

        static public Complex common(Complex n0, Complex n1)
        {
            return new Complex(Num.common(n0.r, n1.r), Num.common(n0.r, n1.r));
        }

        static public Complex neg(Complex a)
        {
            Complex r = new Complex(a); r.neg();
            return r;
        }
        static public Complex add(Complex a0, Complex a1)
        {
            Complex r = new Complex(a0); r.add(a1);
            return r;
        }
        static public Complex sub(Complex s0, Complex s1)
        {
            Complex r = new Complex(s0); r.sub(s1);
            return r;
        }
        static public Complex _div(Complex d0)
        {
            Complex r = new Complex(d0); r.div();
            return r;
        }
        static public Complex mul(Complex m0, Complex m1)
        {
            Complex r = new Complex(m0); r.mul(m1);
            return r;
        }
        static public Complex div(Complex d0, Complex d1)
        {
            Complex r = new Complex(d0); r.div(d1);
            return r;
        }
        public static Complex load(BinaryReader file)
        {
            if (file.ReadBoolean()) return new Complex(Num.load(file), Num.load(file)); else return null;
        }
        public static Complex[] load_a(BinaryReader file)
        {
            Complex[] ret = new Complex[file.ReadInt32()];
            int i = 0; while (i < ret.Length)
            {
                ret[i] = Complex.load(file);
                i++;
            }
            return ret;
        }
        public void save(BinaryWriter file)
        {
            file.Write(true);
            r.save(file);
            i.save(file);
        }
        public static void save(BinaryWriter file, Complex _c)
        {
            if (_c == null) file.Write(false); else _c.save(file);
        }
        public static void save_a(BinaryWriter file, Complex[] _a)
        {
            file.Write((Int32)_a.Length);
            foreach (Complex c in _a) Complex.save(file, c);
        }
        public Complex(Num _r, Num _i)
        {
            r = _r;
            i = _i;
            s = null;
        }
        public Complex(int _r, string _s)
        {
            r = Num.get(_r);
            i = new Num(_s);
            s = null;
        }
        public Complex(string _s)
        {
            r = new Num(_s);
            i = IDS.nums[IDS.znums];
            s = r;
        }
        public Complex(int _r)
        {
            r = Num.get(_r);
            i = IDS.nums[IDS.znums];
            s = r;
        }
        public Complex(Num _r)
        {
            r = new Num(_r);
            i = IDS.nums[IDS.znums];
            s = r;
        }
        public Complex()
        {
            r = IDS.nums[IDS.znums];
            i = IDS.nums[IDS.znums];
            s = IDS.nums[IDS.znums];
        }
        public Complex(Complex _k)
        {
            r = _k.r;
            i = _k.i;
            s = _k.s;
        }
        public override void set0()
        {
            r = IDS.nums[IDS.znums];
            i = IDS.nums[IDS.znums];
            s = IDS.nums[IDS.znums];
        }
        public override void set1()
        {
            r = IDS.nums[IDS.znums + 1];
            i = IDS.nums[IDS.znums];
            s = IDS.nums[IDS.znums + 1];
        }
        public override void set(Complex a)
        {
            r = a.r;
            i = a.i;
            s = a.s;
        }
        public override void copy(ref Complex a)
        {
            a.r = r;
            a.i = i;
            a.s = s;
        }
        public int sign()
        {
            return (r.sign != 0 ? r.sign : i.sign);
        }
        public void max(Complex a)
        {
            if (great(a)) set(a);
        }
        public void min(Complex a)
        {
            if (a.great(this)) set(a);
        }

        public void extract(Complex n)
        {
            r = Num.extract(r, n.r);
            i = Num.extract(i, n.i);
        }

        public bool isint()
        {
            return (i.sign == 0) && r.isint();
        }
        public bool isint(int _r)
        {
            return (i.sign == 0) && r.isint(_r);
        }
        public static bool isint(Complex n, int _i)
        {
            if (n == null) return false; else return n.isint(_i);
        }

        public void neg()
        {
            r = Num.neg(r); i = Num.neg(i); if (s != null) s = Num.neg(s);
        }
        public Num mod()
        {
            BigInteger id = i.down * i.down, rd = r.down * r.down;
            Num _r = new Num(1, r.up * r.up * id + i.up * i.up * rd, id * rd);
            _r.exp(new Num(1, 1, 2));
            return _r;
        }
        public void add(Complex a)
        {
            r = Num.add(r, a.r); i = Num.add(i, a.i); s = null;
        }
        public void sub(Complex a)
        {
            r = Num.sub(r, a.r); i = Num.sub(i, a.i); s = null;
        }
        public override void div()
        {
            //k/(k*k+i*i) : -i/(k*k+i*i)
            Num x0, x1;
            x0 = new Num(r); x0.exp2();
            x1 = new Num(i); x1.exp2();
            x0.add(x1); x0.div();
            r = Num.mul(r, x0); i = Num.mul(i, x0);
            s = null;
        }
        //ku/kd : iu/id  | * kd*id:kd*id
        //ku/kd * kd*id - iu/id * kd*id : ku/kd * kd*id + iu/id * kd*id
        //ku*id - iu*kd : ku*id + iu*kd
        public override void mul(Complex a)
        {
            //k*a.k - i*a.i : k*a.i + i*a.k
            Num x, _r;
            _r = new Num(r); _r.mul(a.r);
            x = new Num(i); x.mul(a.i);
            x.neg(); _r.add(x);
            i = Num.mul(i, a.r); i.add(Num.mul(r, a.i));
            r = _r;
            s = null;
        }
        public void div(Complex a)
        {
            Complex _r = new Complex(a);
            _r.div();
            mul(_r);
        }
        public new void exp2()
        {
            //k^2-i^2 : 2*k*i
            Num r2, i2;
            r2 = new Num(r); r2.exp2();
            i2 = new Num(i); i2.exp2();
            r2.add(i2, -1);
            i = Num.mul(i, r); i.mul(2);
            r = r2;
            s = null;
        }
        public void exp(Complex pow)
        {
            if (pow.i.sign != 0) IDS.root.sys.error("not yet");
            exp(pow.r);
        }
        public void ln()
        {
            if (i.sign == 0)
            {
                r = new Num(r); r.ln();
            }
            else
            {


            }
        }
        public void exp(Num pow)
        {
            if (pow.isint()) exp(pow.toint());
            else
            {
                if (i.sign == 0) r = Num.exp(r, pow);
                else
                {
                    IDS.root.sys.error("not yet");
                }
            }
        }
        public bool iszero()
        {
            return (r.sign == 0) && (i.sign == 0);
        }
        public bool isequ(Complex a)
        {
            return ((r.CompareTo(a.r) == 0) && (i.CompareTo(a.i) == 0));
        }
        public bool isint(int _r, int _i)
        {
            return r.isint(_r) && i.isint(_i);
        }
        public void simple()
        {
            r = r.simple(); i = i.simple();
        }
        public Complex get_simple()
        {
            return new Complex(r.simple(), i.simple());
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Complex c = obj as Complex;
            if (s == null) s = Num.add(r, i, 1);
            if (c.s == null) c.s = Num.add(c.r, c.i, 1);
            int _r = s.CompareTo(c.s); if (_r != 0) return _r;
            return (r.CompareTo(c.r));
        }
        public bool great(Complex a)
        {
            return CompareTo(a) < 0;
        }
        public BigInteger toint()
        {
            return (i.sign == 0 ? r.toint() : mod().toint());
        }
        public double todouble()
        {
            return (i.sign == 0 ? r.todouble() : mod().todouble());
        }

    }
    public class Row_simple
    {


        public Row_simple(Many2 p)
        {
        }
        public Complex calc(Complex p)
        {
            Complex r = new Complex();



            return r;
        }
    }

    public class One : IComparable
    {
        public static One zero;
        public SortedDictionary<Func, Func> exps;
        public One()
        {
            exps = new SortedDictionary<Func, Func>();
        }
        public One(Func f)
        {
            exps = new SortedDictionary<Func, Func>();
            exps.Add(f, new Func(new Complex(1)));
        }
        public One(Func fe, Func fp)
        {
            exps = new SortedDictionary<Func, Func>();
            exps.Add(fe, fp);
        }
        public One(One o)
        {
            exps = new SortedDictionary<Func, Func>();
            set(o);
        }
        public static One load(BinaryReader file)
        {
            One ret = new One();
            int i = 0, cnt = file.ReadInt32();
            while (i < cnt)
            {
                ret.exps.Add(Func.load(file), Func.load(file));
                i++;
            }
            return ret;
        }
        public void save(BinaryWriter file)
        {
            file.Write((Int32)(exps.Count));
            foreach (KeyValuePair<Func, Func> m in exps)
            {
                m.Key.save(file);
                m.Value.save(file);
            }
        }
        public void set(One o)
        {
            exps.Clear();
            foreach (KeyValuePair<Func, Func> m in o.exps) exps.Add(new Func(m.Key), new Func(m.Value));
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            One o = obj as One;
            SortedDictionary<Func, Func>.Enumerator e0 = exps.GetEnumerator();
            SortedDictionary<Func, Func>.Enumerator e1 = o.exps.GetEnumerator();
            bool n0, n1;
            int rk, rv;
            while (true)
            {
                do n0 = e0.MoveNext(); while (n0 && (e0.Current.Value.isconst(0, 0)));
                do n1 = e1.MoveNext(); while (n1 && (e1.Current.Value.isconst(0, 0)));
                if (n0 ^ n1) return (n0 ? e0.Current.Value.CompareTo(Func.zero) : Func.zero.CompareTo(e1.Current.Value));
                else
                {
                    if (!n0) return 0;
                    if ((rk = e0.Current.Key.CompareTo(e1.Current.Key)) == 0)
                    {
                        if ((rv = e0.Current.Value.CompareTo(e1.Current.Value)) != 0) return rv;
                    }
                    else
                    {
                        if (rk < 0) return e0.Current.Value.CompareTo(Func.zero);
                        else return Func.zero.CompareTo(e1.Current.Value);
                    }
                }
            }
        }

        public void addto(Func val, Func exp) //no new
        {
            if (exps.ContainsKey(val)) exps[val].add(exp); else exps.Add(val, exp);
        }
        public void addto(Func val)
        {
            if (exps.ContainsKey(val)) exps[val].add(new Func(new Complex(1))); else exps.Add(new Func(val), new Func(new Complex(1)));
        }
        public void mul(One o)
        {
            foreach (KeyValuePair<Func, Func> m in o.exps)
            {
                if (exps.ContainsKey(m.Key))
                {
                    exps[m.Key].add(m.Value);
                    if (exps[m.Key].isconst(0, 0)) exps.Remove(m.Key);
                }
                else exps.Add(new Func(m.Key), new Func(m.Value));
            }
        }
        public void div()
        {
            foreach (KeyValuePair<Func, Func> m in exps) m.Value.neg();
        }
        public void exp(int e)
        {
            exp(new Complex(e));
        }
        public void exp(Complex e)
        {
            foreach (KeyValuePair<Func, Func> f in exps) f.Value.mul(e);
        }
        public void exp(Func e)
        {
            foreach (KeyValuePair<Func, Func> f in exps) f.Value.mul(e);
        }
        public Func get_Func(Complex n)
        {
            Func r = null;
            if (exps.Count == 0) r = new Func(n);
            if ((exps.Count == 1) && (n.CompareTo(Complex._r1) == 0) && exps.ElementAt(0).Value.isconst(1, 0))
                r = exps.ElementAt(0).Key;
            return r;
        }

        public void extract(One from) //common multi
        {
            SortedDictionary<Func, Func> r = new SortedDictionary<Func, Func>();
            foreach (KeyValuePair<Func, Func> m in exps)
                if (from.exps.ContainsKey(m.Key))
                {
                    m.Value.common(from.exps[m.Key]);
                    r.Add(m.Key, m.Value);
                }
            exps = r;
        }
        public Many2 expand(Complex n)
        {
            Many2 r = new Many2(1), t; One o = new One();
            foreach (KeyValuePair<Func, Func> m in exps) if ((m.Key.type == Func.t_many2) && (m.Value.type_pow() == 0))
                {
                    t = new Many2((Many2)(m.Key.data));
                    t.exp(m.Value); r.mul(t);
                }
                else o.addto(new Func(m.Key), new Func(m.Value));
            r.mul(o, n);
            return r;
        }
        public bool expand_p0(Func val, Exps_f exu, Exps_f exd)
        {
            bool rt = false;
            foreach (KeyValuePair<Func, Func> m in exps) if (m.Value.CompareTo(val) == 0)
                {
                    rt = true; m.Value.type = Func.t_many2; m.Value.data = new Many2(new Many(exu.mvar), new Many(exd.mvar));
                }
            return rt;
        }
        public bool expand_2(Func val, Exps_f exu, Exps_f exd)
        {
            bool fv = false, fk = false;
            foreach (KeyValuePair<Func, Func> m in exps)
            {
                fk = m.Key.expand(val, exu, exd) || fk;
                fv = m.Value.expand(val, exu, exd) || fv;
            }
            if (fk)
            {
                One r = new One();
                foreach (KeyValuePair<Func, Func> m in exps)
                    if ((!m.Value.isconst(0, 0)) && (!m.Key.isconst(1, 0))) r.addto(m.Key, m.Value);
                exps = r.exps;
            }
            return fv || fk;
        }

        public void replace(Vals v, Func f)
        {
            foreach (KeyValuePair<Func, Func> ff in exps)
            {
                ff.Key.replace(v, f); ff.Value.replace(v, f);
            }
        }

        public Complex simple()
        {
            Complex nt, rt = new Complex(1); Func p, e;
            One r = new One();
            foreach (KeyValuePair<Func, Func> ff in exps)
            {
                ff.Value.simple();
                if (!ff.Value.isconst(0, 0))
                {
                    ff.Key.simple();
                    if ((ff.Key.type == Func.t_num) && (ff.Value.type_pow() == 0)) rt.mul(Complex.exp((Complex)(ff.Key.data), (Complex)(ff.Value.data)));
                    else
                    {
                        if ((ff.Key.type == Func.t_val) && (((Vals)(ff.Key.data)).var == IDS.root.v_e) && (ff.Value.type == Func.t_ln))
                        { p = new Func((Many2)(ff.Value.data)); e = new Func(new Complex(1)); }
                        else { p = ff.Key; e = ff.Value; }
                        if ((p.type == Func.t_many2) && (((Many2)(p.data)).type_exp() < 2))
                        {
                            ((Many2)(p.data)).up.data.ElementAt(0).Key.exp(e);
                            ((Many2)(p.data)).down.data.ElementAt(0).Key.exp(e);
                            ((Many2)(p.data)).down.data.ElementAt(0).Key.div();
                            nt = Complex.mul(((Many2)(p.data)).up.data.ElementAt(0).Value, Complex._div(((Many2)(p.data)).down.data.ElementAt(0).Value));
                            r.mul(((Many2)(p.data)).up.data.ElementAt(0).Key);
                            r.mul(((Many2)(p.data)).down.data.ElementAt(0).Key);
                            if (e.type_pow() < 1) rt.mul(Complex.exp(nt, (Complex)(e.data)));
                            else r.addto(new Func(nt), e);
                        }
                        else r.addto(p, e);
                    }
                }
            }
            exps = r.exps;
            return rt;
        }
        public void deeper(int deep)
        {
            SortedDictionary<Func, Func> r = new SortedDictionary<Func, Func>();
            foreach (KeyValuePair<Func, Func> ff in exps) r.Add(Func.deeper(ff.Key, deep), Func.deeper(ff.Value, deep));
            exps = r;
        }
        public Complex calc()
        {
            Complex rt = new Complex(1);
            foreach (KeyValuePair<Func, Func> f in exps) rt.mul(f.Key.calc(f.Value.calc()));
            return rt;
        }
        public void findvals(One o)
        {
            foreach (KeyValuePair<Func, Func> f in exps) { f.Key.findvals(o); f.Value.findvals(o); }
        }
        public Many diff_down(Vals at)
        {
            Many r = new Many(); One a;
            List<Func> d = new List<Func>(), p = new List<Func>(), e = new List<Func>();
            One f_d0 = new One();
            Func t;
            foreach (KeyValuePair<Func, Func> ff in exps)
            {
                t = ff.Key.diff_down(ff.Value, at);
                if (t.isconst(0, 0)) f_d0.addto(ff.Key, ff.Value);
                else
                {
                    p.Add(ff.Key); e.Add(ff.Value); d.Add(t);
                }
            }
            int i1, i0 = 0; while (i0 < d.Count)
            {
                a = new One(f_d0);
                i1 = 0; while (i1 < d.Count)
                {
                    if (i0 == i1)
                    {
                        a.addto(d[i1]);
                    }
                    else
                    {
                        a.addto(new Func(p[i1]), new Func(e[i1]));
                    }
                    i1++;
                }
                r.add(a);
                i0++;
            }
            r.simple(); return r;
        }
        public Many diff_up(Vals at)
        {
            Many r;
            One o = new One();
            Func fa = new Func(at), ex = new Func(new Complex(0));
            foreach (KeyValuePair<Func, Func> ff in exps)
            {
                if ((ff.Value.type > Func.t_num) || (ff.Key.type > Func.t_num)) IDS.root.sys.error("S: too complex");
                if (ff.Value.type == Func.t_val)
                {
                    if (ff.Value.data == at)
                    {
                        if (ff.Value.CompareTo(ff.Key) == 0) IDS.root.sys.error("S: x^x");
                        IDS.root.sys.error("now not implemented");
                    }
                    else
                    {
                        if (ff.Key.CompareTo(fa) == 0)
                        {
                            if (!ex.isconst(0, 0)) IDS.root.sys.error("now not implemented");
                            ex.add(ff.Value);
                        }
                        else
                        {
                            o.addto(new Func(ff.Key), new Func(ff.Value));
                        }
                    }
                }
                else
                {
                    if (ff.Key.CompareTo(fa) == 0)
                    {
                        if (!ex.isconst(0, 0)) IDS.root.sys.error("now not implemented");
                        ex.add(ff.Value);
                    }
                    else
                    {
                        o.addto(new Func(ff.Key), new Func(ff.Value));
                    }
                }
            }
            if (ex.isconst(-1, 0))
            {
                o.addto(new Func(Func.t_ln, new Many2(fa)));
            }
            else
            {
                ex.add(new Func(new Complex(1)));
                o.addto(fa, new Func(ex)); o.addto(ex, new Func(new Complex(-1)));
            }
            r = new Many(o); r.simple(); return r;
        }

    }

    public class Many : Power<Many>, IPower, IComparable
    {
        public SortedDictionary<One, Complex> data;
        public Many()
        {
            data = new SortedDictionary<One, Complex>();
        }
        public Many(One o, Complex n)
        {
            data = new SortedDictionary<One, Complex>();
            data.Add(o, n);
        }
        public Many(Func f)
        {
            data = new SortedDictionary<One, Complex>();
            data.Add(new One(f), new Complex(1));
        }
        public Many(One o)
        {
            data = new SortedDictionary<One, Complex>();
            data.Add(o, new Complex(1));
        }
        public Many(Complex n)
        {
            data = new SortedDictionary<One, Complex>();
            data.Add(new One(), n);
        }
        public Many(Many m)
        {
            set(m);
        }
        public static Many load(BinaryReader file)
        {
            Many ret = new Many();
            int i = 0, cnt = file.ReadInt32();
            while (i < cnt)
            {
                ret.data.Add(One.load(file), Complex.load(file));
                i++;
            }
            return ret;
        }
        public void save(BinaryWriter file)
        {
            file.Write((Int32)(data.Count));
            foreach (KeyValuePair<One, Complex> o in data)
            {
                o.Key.save(file);
                o.Value.save(file);
            }
        }

        public static SortedDictionary<One, Complex> copy(SortedDictionary<One, Complex> c)
        {
            SortedDictionary<One, Complex> r = new SortedDictionary<One, Complex>();
            foreach (KeyValuePair<One, Complex> o in c) r.Add(new One(o.Key), new Complex(o.Value));
            return r;
        }
        public override void set(Many s)
        {
            data = Many.copy(s.data);
        }
        public override void copy(ref Many s)
        {
            s.set(this);
        }
        public override void set0()
        {
            data.Clear();
            data.Add(new One(), new Complex(0));
        }
        public override void set1()
        {
            data.Clear();
            data.Add(new One(), new Complex(1));
        }
        public override void div()
        {
            if (data.Count != 1) IDS.root.sys.error("cant divide many");
            data[data.ElementAt(0).Key].div();
            data.ElementAt(0).Key.div();
        }

        public int sign()
        {
            bool p = false, m = false; int cm;
            foreach (KeyValuePair<One, Complex> o in data)
            {
                cm = o.Value.CompareTo(Complex._0);
                if (cm > 0)
                {
                    p = true; if (m) return 0;
                }
                if (cm < 0)
                {
                    m = true; if (p) return 0;
                }
            }
            if (p) return 1;
            if (m) return -1;
            return 0;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) { int s = sign(); return (s != 0 ? s : data.ElementAt(0).Value.CompareTo(Complex._0)); }
            Many m = obj as Many;
            SortedDictionary<One, Complex>.Enumerator o0 = data.GetEnumerator();
            SortedDictionary<One, Complex>.Enumerator o1 = m.data.GetEnumerator();
            bool n0, n1;
            int r;
            while (true)
            {
                do n0 = o0.MoveNext(); while (n0 && (o0.Current.Value.iszero()));
                do n1 = o1.MoveNext(); while (n1 && (o1.Current.Value.iszero()));
                if (n0 ^ n1) return (n0 ? o0.Current.Value.CompareTo(Complex._0) : Complex._0.CompareTo(o1.Current.Value));
                else
                {
                    if (!n0) return 0;
                    if ((r = o0.Current.Key.CompareTo(o1.Current.Key)) == 0)
                    {
                        if ((r = o0.Current.Value.CompareTo(o1.Current.Value)) != 0) return r;
                    }
                    else
                    {
                        if (r > 0) return o0.Current.Value.CompareTo(Complex._0);
                        else return Complex._0.CompareTo(o1.Current.Value);
                    }
                }
            }
        }
        public Func get_Func()
        {
            Func r = null;
            if (data.Count == 0) r = new Func(new Complex(0));
            if (data.Count == 1) r = data.ElementAt(0).Key.get_Func(data.ElementAt(0).Value);
            return r;
        }
        public Complex get_Num()
        {
            Complex r = null;
            if (data.Count == 0) r = new Complex(0);
            if ((data.Count == 1) && (data.ElementAt(0).Key.CompareTo(One.zero) == 0))
                r = new Complex(data.ElementAt(0).Value);
            return r;
        }
        public BigInteger get_mult_for_int()
        {
            BigInteger r = 1, b;
            foreach (KeyValuePair<One, Complex> on in data)
            {
                b = BigInteger.GreatestCommonDivisor(r, on.Value.r.down);
                r *= on.Value.r.down / b;
                b = BigInteger.GreatestCommonDivisor(r, on.Value.i.down);
                r *= on.Value.i.down / b;
            }
            return r;
        }
        public bool isint(int r, int i)
        {
            Complex n = get_Num();
            if (n != null) return n.isint(r, i);
            return false;
        }

        public void add(One o, Complex n)
        {
            if (data.ContainsKey(o))
            {
                data[o].add(n);
            }
            else data.Add(new One(o), new Complex(n));
        }
        public void sub(One o, Complex n)
        {
            if (data.ContainsKey(o))
            {
                data[o].sub(n);
            }
            else data.Add(new One(o), Complex.neg(n));
        }
        public void add(One o)
        {
            add(o, Complex._r1);
        }
        public void add(Many from)
        {
            foreach (KeyValuePair<One, Complex> o in from.data) add(o.Key, o.Value);
        }
        public void sub(Many from)
        {
            foreach (KeyValuePair<One, Complex> o in from.data) sub(o.Key, o.Value);
        }

        public void mul(One o, Complex n)
        {
            SortedDictionary<One, Complex> r = new SortedDictionary<One, Complex>();
            One _o; Complex _n;
            foreach (KeyValuePair<One, Complex> m0 in data)
            {
                _o = new One(m0.Key);
                _o.mul(o);
                _n = Complex.mul(m0.Value, n);
                if (r.ContainsKey(_o)) r[_o].add(_n); //in r any Num is new
                else r.Add(_o, _n);
            }
            data = r;
        }
        public override void mul(Many _m)
        {
            SortedDictionary<One, Complex> r = new SortedDictionary<One, Complex>();
            One o; Complex n;
            foreach (KeyValuePair<One, Complex> m0 in data)
            {
                foreach (KeyValuePair<One, Complex> m1 in _m.data)
                {
                    o = new One(m0.Key);
                    o.mul(m1.Key);
                    n = Complex.mul(m0.Value, m1.Value);
                    if (r.ContainsKey(o)) r[o].add(n);
                    else r.Add(o, n);
                }
            }
            data = r;
        }
        public void mul(Complex n)
        {
            for (int _i = 0; _i < data.Count; _i++) (data[data.ElementAt(_i).Key] = Complex.mul(data.ElementAt(_i).Value, n)).simple();
        }
        public void mul_simple(Complex n)
        {
            for (int _i = 0; _i < data.Count; _i++) (data[data.ElementAt(_i).Key] = Complex.mul(data.ElementAt(_i).Value, n)).simple();
        }

        public Many expand()
        {
            Many ret = new Many(new Complex(1)), r = new Many(); Many2 tmp;
            foreach (KeyValuePair<One, Complex> o in data)
            {
                tmp = o.Key.expand(o.Value);
                r.mul(tmp.down); //div to prev
                tmp.up.mul(ret); //accum div to now
                r.add(tmp.up); //
                ret.mul(tmp.down); //accum
            }
            data = r.data;
            return ret;
        }

        public void expand(Many from, One o, Complex n, Func val)
        {
            One to = new One(o); to.exps[val].set0();
            One _o; Complex _n;
            foreach (KeyValuePair<One, Complex> on in from.data)
            {
                _o = new One(to); _o.mul(on.Key);
                _n = Complex.mul(n, on.Value);
                add(_o, _n);
            }
        }
        public bool expand_p0(Func val, Exps_f exu, Exps_f exd)
        {
            bool rt = false;
            foreach (KeyValuePair<One, Complex> o in data) rt = o.Key.expand_p0(val, exu, exd) || rt;
            return rt;
        }
        public bool expand_2(Func val, Exps_f exu, Exps_f exd)
        {
            bool rt = false;
            foreach (KeyValuePair<One, Complex> o in data) rt = o.Key.expand_2(val, exu, exd) || rt;
            return rt;
        }
        //e0,e2,p0,p2
        public bool expand_e0(Func val, Exps_f exu, Exps_f exd)
        {
            bool rt = false;
            SortedDictionary<Func, Many> ml = new SortedDictionary<Func, Many>();
            Many res = new Many();
            exu.add(this, val, +1); exu.calc();
            exd.add(this, val, -1); exd.calc();
            int type = (exu.type > exd.type ? exu.type : exd.type);
            Func eu, ed, e;
            foreach (KeyValuePair<One, Complex> o in data)
            {
                //[+1]/[-1]
                //[-up] *= [+1]^min*[-1]^max 
                //[+1]^(up + (-min))*[-1]^(max - up)
                if (o.Key.exps.ContainsKey(val))
                {
                    if (o.Key.exps[val].type_pow() + type < 3)
                    {
                        e = o.Key.exps[val];
                        eu = (exu.type > 1 ? new Func(Complex.add(exu.min, (Complex)(e.data))) : e);
                        ed = (exd.type > 1 ? new Func(Complex.sub(exd.max, (Complex)(e.data))) : e);
                        if (exu.data.ContainsKey(eu) && exd.data.ContainsKey(ed))
                        {
                            if (!ml.ContainsKey(e))
                            {
                                ml.Add(new Func(e), new Many(exu.data[eu]));
                                ml[e].mul(exd.data[ed]);
                            }
                            res.expand(ml[e], o.Key, o.Value, val);
                        }
                        else res.add(o.Key, o.Value);
                    }
                    else res.add(o.Key, o.Value);
                }
                else res.add(o.Key, o.Value);
            }
            data = res.data;
            return rt;
        }
        public void replace(Vals v, Func f)
        {
            foreach (KeyValuePair<One, Complex> o in data) o.Key.replace(v, f);
        }

        public int type_exp()
        {
            if ((data.Count == 1) && (data.ElementAt(0).Value.i.sign == 0))
            {
                if (data.ElementAt(0).Value.isint()) return 0; else return 1;
            }
            else return 2;
        }
        public void exp(Func e)
        {
            int te = type_exp(), tp = e.type_pow();
            if (te + tp > 2) IDS.root.sys.error("exp: cant Many to many");
            if (te > 1) exp((int)(((Complex)(e.data)).r.up));
            else
            {
                One to = data.ElementAt(0).Key;
                if (tp < 2)
                {
                    data[to] = Complex.exp(data[to], (Complex)(e.data));
                    to.exp((Complex)(e.data));
                }
                else to.exp(e);
            }
        }

        public new void exp(int e)
        {
            if (data.Count > 1) base.exp(e);
            else if (data.Count == 1)
            {
                One to = data.ElementAt(0).Key;
                data[to] = Complex.exp(data[to], e);
                to.exp(e);
            }
        }
        public KeyValuePair<One, Complex> extract()
        {
            if (data.Count == 0) return new KeyValuePair<One, Complex>(new One(), new Complex(1));
            else
            {
                One ro = null; Complex rn = null;
                foreach (KeyValuePair<One, Complex> m in data)
                {
                    if (ro == null)
                    {
                        ro = new One(m.Key); rn = new Complex(m.Value);
                    }
                    else
                    {
                        ro.extract(m.Key);
                        rn.extract(m.Value);
                        if (ro.exps.Count < 1) new KeyValuePair<One, Complex>(ro, new Complex(0));
                        if (rn.iszero()) new KeyValuePair<One, Complex>(new One(), rn);
                    }
                }
                return new KeyValuePair<One, Complex>(ro, rn);
            }
        }

        public void neg()
        {
            for (int i = 0; i < data.Count; i++) data[data.ElementAt(i).Key] = Complex.neg(data.ElementAt(i).Value);
        }

        public void deeper(int deep)
        {
            SortedDictionary<One, Complex> r = new SortedDictionary<One, Complex>();
            One o;
            foreach (KeyValuePair<One, Complex> m in data)
            {
                o = new One(m.Key); o.deeper(deep);
                r.Add(o, m.Value); //deeper dont remove val from one
            }
            data = r;
        }

        public void add_toexp(Func val, Func _e)
        {
            SortedDictionary<One, Complex> r = new SortedDictionary<One, Complex>();
            One o;
            foreach (KeyValuePair<One, Complex> m in data)
            {
                o = new One(m.Key); o.addto(val, _e);
                if (r.ContainsKey(o)) r[o].add(m.Value); //in r any Num is new
                else r.Add(o, new Complex(m.Value));
            }
            data = r;
        }

        public Complex find_minexp(Func val) //_val^(-x) -> /_val^(x)
        {
            Complex _min = new Complex(0), t;
            foreach (KeyValuePair<One, Complex> o in data)
                if (o.Key.exps.ContainsKey(val))
                {
                    t = o.Key.exps[val].get_num_part();
                    if (t.CompareTo(_min) < 0) _min.set(t);
                }
            return _min;
        }

        public void common(Many m)
        {
            bool ch = false; KeyValuePair<One, Complex> on; Complex n;
            for (int i = 0; i < data.Count; i++)
            {
                on = data.ElementAt(i);
                if (m.data.ContainsKey(on.Key))
                {
                    n = Complex.common(on.Value, m.data[on.Key]);
                    if (n.iszero()) ch = true;
                    data[on.Key] = n;
                }
                else { ch = true; data[on.Key] = new Complex(0); }
            }
            if (ch)
            {
                SortedDictionary<One, Complex> r = new SortedDictionary<One, Complex>();
                foreach (KeyValuePair<One, Complex> _on in data) if (!_on.Value.iszero()) r.Add(_on.Key, _on.Value);
                data = r;
            }
        }
        /* ?????? common ?????
                public void extract(Many m)
                {
                    SortedDictionary<One,Complex> r = new SortedDictionary<One,Complex>();
                    foreach (KeyValuePair<One,Complex> d in data)
                    {
                        if (m.data.ContainsKey(d.Key)) {
                            if (d.Value.sign == m.data[d.Key].sign)
                                r.Add(new One(d.Key),Num.mul(d.Value,m.data[d.Key]));
                        }
                    }
                    data = r;
                }
        */
        public void simple()
        {
            if (data.Count < 1) return;
            One k; Complex v;
            SortedDictionary<One, Complex> r = new SortedDictionary<One, Complex>();
            foreach (KeyValuePair<One, Complex> d in data)
            {
                if (!d.Value.iszero())
                {
                    k = new One(d.Key); v = Complex.mul(k.simple(), d.Value);
                    if (r.ContainsKey(k)) r[k] = Complex.add(r[k], v); else r.Add(k, v.get_simple());
                }
            }
            data = r;
        }
        public Complex calc()
        {
            Complex rt = new Complex(0);
            foreach (KeyValuePair<One, Complex> o in data) rt.add(Complex.mul(o.Key.calc(), o.Value));
            return rt;
        }
        public void findvals(One o)
        {
            foreach (KeyValuePair<One, Complex> on in data) on.Key.findvals(o);
        }

        public Many diff_down(Vals at)
        {
            Many m, r = new Many(new Complex(0));
            foreach (KeyValuePair<One, Complex> on in data)
            {
                m = on.Key.diff_down(at); m.mul(on.Value);
                r.add(m);
            }
            return r;
        }
        public Many diff_up(Vals at)
        {
            Many m, r = new Many(new Complex(0));
            foreach (KeyValuePair<One, Complex> on in data)
            {
                m = on.Key.diff_up(at); m.mul(on.Value);
                r.add(m);
            }
            return r;
        }
    }

    public class Many2 : Power<Many2>, IPower, IComparable
    {
        public Many up, down;
        public Many2()
        {
            up = new Many(); down = new Many();
        }
        public Many2(Many u, Many d) //no new 
        {
            up = u; down = d;
        }
        public Many2(Many u) //no new 
        {
            up = u;
            down = new Many();
            down.data.Add(new One(), new Complex(1));
        }
        public Many2(One u) //no new 
        {
            up = new Many(u);
            down = new Many();
            down.data.Add(new One(), new Complex(1));
        }
        public Many2(Func u) //no new 
        {
            up = new Many(new One(u));
            down = new Many();
            down.data.Add(new One(), new Complex(1));
        }
        public Many2(int n)
        {
            up = new Many(); down = new Many();
            up.data.Add(new One(), new Complex(n));
            down.data.Add(new One(), new Complex(1));
        }
        public Many2(Complex n)
        {
            up = new Many(); down = new Many();
            up.data.Add(new One(), n);
            down.data.Add(new One(), new Complex(1));
        }
        public Many2(One o, Complex n)
        {
            up = new Many(); down = new Many();
            up.data.Add(new One(o), new Complex(n));
            down.data.Add(new One(), new Complex(1));
        }

        public Many2(Many2 m)
        {
            up = new Many(m.up); down = new Many(m.down);
        }
        public static Many2 load(BinaryReader file)
        {
            return new Many2(Many.load(file), Many.load(file)); ;
        }
        public void save(BinaryWriter file)
        {
            up.save(file);
            down.save(file);
        }

        public override void set(Many2 s)
        {
            up.set(s.up); down.set(s.down);
        }
        public override void copy(ref Many2 s)
        {
            s.set(this);
        }
        public override void set0()
        {
            up.data.Clear(); down.data.Clear();
            up.data.Add(new One(), new Complex(0));
            down.data.Add(new One(), new Complex(1));
        }
        public override void set1()
        {
            up.data.Clear(); down.data.Clear();
            up.data.Add(new One(), new Complex(1));
            down.data.Add(new One(), new Complex(1));
        }
        public override void div()
        {
            Many t;
            if ((up.data.Count == 1) && (up.data.ElementAt(0).Value.iszero())) IDS.root.sys.error("div0");
            t = up; up = down; down = t;
        }

        public int sign()
        {
            return up.sign() * down.sign();
        }


        public int CompareTo(object obj)
        {
            if (obj == null) { int s = sign(); return (s != 0 ? s : down.data.ElementAt(0).Value.CompareTo(Complex._0)); }
            Many2 m = obj as Many2;
            int r = up.CompareTo(m.up);
            if (r != 0) return r;
            return -down.CompareTo(m.down);
        }

        public void add(Complex n)
        {
            Many t = new Many(n); t.mul(down); up.add(t);
        }

        public void add(Many2 m)
        {
            if (down.CompareTo(m.down) == 0) up.add(m.up);
            else
            {
                Many t = new Many(m.up);
                t.mul(down); up.mul(m.down); up.add(t);
                down.mul(m.down);
            }
        }

        public override void mul(Many2 m)
        {
            up.mul(m.up); down.mul(m.down);
        }
        public void mul(Complex n)
        {
            up.mul(n);
        }
        public void mul(One o, Complex n)
        {
            up.mul(o, n);
        }

        public void revert(Func val) //_val^(-x) -> /_val^(x)
        {
            Complex min = Complex.min(up.find_minexp(val), down.find_minexp(val));
            if (min.iszero()) return;
            min.neg(); Func f = new Func(min);
            up.add_toexp(val, f);
            down.add_toexp(val, f);
        }

        public bool expand(Func val, Exps_f exu, Exps_f exd)
        {
            bool rt0, rt02;
            rt02 = up.expand_2(val, exu, exd);
            rt02 = down.expand_2(val, exu, exd) || rt02;

            rt02 = up.expand_p0(val, exu, exd) || rt02;
            rt02 = down.expand_p0(val, exu, exd) || rt02;

            rt0 = up.expand_e0(val, exu, exd);
            Complex minup = new Complex(exu.min), maxdw = new Complex(exd.max);
            rt0 = down.expand_e0(val, exu, exd) || rt0;
            if (rt0)
            {
                up.mul(exu.data[new Func(exu.min)]);
                up.mul(exd.data[new Func(exd.max)]);
                down.mul(exu.data[new Func(minup)]);
                down.mul(exd.data[new Func(maxdw)]);
            }
            if (rt0 || rt02) simple();
            return rt0 || rt02;
        }
        public void expand()
        {
            Many tod = up.expand(); Many tou = down.expand();
            up.mul(tou); down.mul(tod);
        }
        public void replace(Vals v, Func f)
        {
            up.replace(v, f);
            down.replace(v, f);
        }

        public int type_exp()
        {
            int u = up.type_exp();
            int d = down.type_exp();
            return (u > d ? u : d);
        }
        public void exp(Func e)
        {
            if (e.type_pow() < 2)
            {
                Func _e = new Func(e);
                if (((Complex)(_e.data)).r.sign < 0)
                {
                    ((Complex)(_e.data)).neg();
                    up.exp(_e); down.exp(_e);
                    Many t = up; up = down; down = t;
                }
                else
                {
                    up.exp(_e); down.exp(_e);
                }
            }
            else { up.exp(e); down.exp(e); }
        }

        public void neg()
        {
            up.neg();
        }

        public void deeper(int deep)
        {
            up.deeper(deep);
            down.deeper(deep);
        }
        public Func get_Func()
        {
            if (down.isint(1, 0)) return up.get_Func(); else return null;
        }
        public Complex get_Num()
        {
            Complex u = up.get_Num();
            if (u == null) return null;
            Complex d = down.get_Num();
            if (d == null) return null;
            d.div(); u.mul(d); return u;
        }
        public Complex get_num_part() //up[num_one]/down(Num)
        {
            Complex d = down.get_Num();
            if ((d == null) || (!up.data.ContainsKey(One.zero))) return new Complex(0);
            d.div(); d.mul(up.data[One.zero]); return d;
        }
        public void common(Many2 m)
        {
            if (down.CompareTo(m.down) == 0) up.common(m.up);
            else
            {
                Many t = new Many(m.up);
                t.mul(down); up.mul(m.down); down.mul(m.down);
                up.common(t);
            }
        }
        public void toint()
        {
            BigInteger a = up.get_mult_for_int() * down.get_mult_for_int();
            Complex m = new Complex(new Num(a));
            up.mul_simple(m); down.mul_simple(m);
        }
        public Complex simple()
        {
            up.simple();
            down.simple();

            if ((up.data.Count > 1) || (down.data.Count > 1))
            {
                KeyValuePair<One, Complex> f_up = up.extract(), f_down = down.extract();
                if ((f_up.Key.exps.Count + f_down.Key.exps.Count > 0))
                {
                    One ou = new One(f_up.Key); ou.extract(f_down.Key); ou.div();
                    Complex cu = new Complex(f_up.Value); cu.extract(f_down.Value); cu.div();
                    up.mul(ou, cu); f_up.Key.mul(ou);
                    down.mul(ou, cu); f_down.Key.mul(ou);
/*
                        f_up.Value.div(); up.mul(f_up.Value);
                        f_down.Value.div(); down.mul(f_down.Value);
                        f_up.Value.div(); f_up.Value.mul(f_down.Value);
                        ou.addto(new Func(new Many2(up, down)), new Func(new Complex(1)));
                        down = new Many(new Complex(1));
                        up = new Many(ou, f_up.Value);
                    */
                    foreach (KeyValuePair<Func, Func> u in f_up.Key.exps) if (u.Value.sign() >= 0) u.Value.set0();
                    f_up.Value.set(f_up.Key.simple());
                    foreach (KeyValuePair<Func, Func> d in f_down.Key.exps) if (d.Value.sign() >= 0) d.Value.set0();
                    f_down.Value.set(f_down.Key.simple());
                    f_up.Key.div();
                    down.mul(f_up.Key, Complex._r1);
                    up.mul(f_up.Key, Complex._r1);
                    f_down.Key.div();
                    up.mul(f_down.Key, Complex._r1);
                    down.mul(f_down.Key, Complex._r1);
                }
            }
            else
            {
                if (!down.isint(1, 0))
                {
                    down.div(); up.mul(down); down = new Many(new Complex(1));
                }
                Func t = up.get_Func();
                if ((t != null) && (t.type == Func.t_many2) && ((Many2)(t.data)).down.isint(1, 0)) up = ((Many2)(t.data)).up;
                t = down.get_Func();
                if ((t != null) && (t.type == Func.t_many2) && ((Many2)(t.data)).down.isint(1, 0)) down = ((Many2)(t.data)).up;
                Complex nd = down.get_Num();
                if (nd == null) return null;
                Complex nu = up.get_Num();
                if (nu != null)
                {
                    nd.div(); nu.mul(nd); nu.simple(); return nu;
                }
            }
            return null;
        }
        public Complex calc()
        {
            Complex d = down.calc();
            if (d.iszero()) IDS.root.sys.error("div0");
            d.div(); d.mul(up.calc()); d.simple(); return d;
        }
        public void findvals(One o)
        {
            up.findvals(o); down.findvals(o);
        }
        public bool hasval(Vals a)
        {
            One t = new One(); findvals(t);
            return t.exps.ContainsKey(new Func(a));
        }

        public Many2 diff_down(Vals at)
        {
            Many fu = up.diff_down(at), fd = down.diff_down(at);
            Many2 r = new Many2(new Many(), new Many());
            fu.mul(down); fd.mul(up); fu.sub(fd); r.up = fu;
            r.down = new Many(down); r.down.mul(down);
            r.simple(); return r;
        }
        public Many2 diff_up(Vals at)
        {
            Many _u = new Many(up); Many _d = new Many(down); _d.div(); _u.mul(_d);
            Many2 r = new Many2(_u.diff_up(at));
            r.simple(); return r;
        }
    }
    public class Exps_f
    {
        public Complex max, min;
        int deep;
        public int type;
        //0 - 1*one, 1 - n*one, 2 - many
        //0,1 - sign, 2 - no sign
        public SortedDictionary<Func, Many> data;
        public Many mvar;
        Func e1;
        public Exps_f(Many m, int _deep)
        {
            min = new Complex(0); max = new Complex(0);
            data = new SortedDictionary<Func, Many>();
            Func e0 = new Func(new Complex(0));
            data.Add(e0, new Many());
            data[e0].data.Add(new One(), new Complex(1));
            deep = _deep;
            e1 = new Func(new Complex(1));
            mvar = new Many(m); if (deep > 0) mvar.deeper(deep);
            type = mvar.type_exp();
            data.Add(e1, mvar);
        }
        void add(Func e)
        {
            if (!data.ContainsKey(e)) data.Add(e, new Many());
        }

        public void add(Many m, Func val, int updown)
        {
            Complex t;
            min.set0(); max.set0();
            Func te;
            if (type > 1)
            {
                foreach (KeyValuePair<One, Complex> on in m.data)
                {
                    if (on.Key.exps.ContainsKey(val))
                    {
                        te = on.Key.exps[val];
                        if (te.type_pow() + type < 3)
                        {
                            t = (Complex)(te.data);
                            min.min(t); max.max(t);
                        }
                    }
                }
                min.neg();
                add(new Func(min)); add(new Func(max));
            }
            t = new Complex(0);
            /*
            [+1]^(pow - min)*[-1]^(max - pow)
            (a+b)/(c+d)
             -1, -2, +1
             (a+b)^2*(c+d)^1
             [+1]^(-1 - (-2))*[-1]^(+1 - (-1)) (+1) (+2)
             [+1]^(-2 - (-2))*[-1]^(+1 - (-2)) (0) (+3)
             [+1]^(+1 - (-2))*[-1]^(+1 - (+1)) (+3) (0)
            */
            foreach (KeyValuePair<One, Complex> on in m.data)
            {
                if (on.Key.exps.ContainsKey(val))
                {
                    te = on.Key.exps[val];
                    if (te.type_pow() + type < 3)
                    {
                        if (type < 2) add(new Func(te));
                        else
                        {
                            if (updown == +1)
                            { //pow + (-min)
                                t.set((Complex)(te.data)); t.add(min);
                                add(new Func(t));
                            }
                            if (updown == -1)
                            {//max - pow
                                t.set(max); t.sub((Complex)(te.data));
                                add(new Func(t));
                            }
                        }
                    }
                }
            }
        }
        public void calc()
        {
            Func dl = new Func(new Complex(0));
            foreach (KeyValuePair<Func, Many> d in data)
            {
                /*                if (d.Value.data.Count == 0) {
                                    if (d.Key.type == 0) {
                                        dl.data = new Num((Num)(d.Key.data));
                                        Num c = new Num(1);
                                        while (((Num)(dl.data)).up != 0) {
                                            c.up <<= 1; ((Num)(dl.data)).up >>= 1;
                                            if (data.ContainsKey(dl)) {
                                                c.up--; c.up &= ((Num)(d.Key.data)).up;

                                                d.Value.set(data[dl]);
                                                d.Value.exp(new Func(c));
                                                break;
                                            }
                                        }
                */


                if (d.Value.data.Count == 0)
                {
                    d.Value.set(data[e1]);
                    d.Value.exp(d.Key);
                }
                //                }
            }
        }
    }

    public class Row : IComparable
    {
        public int point;
        public SortedDictionary<int, Many2> data;
        public Row()
        {
            point = 0;
            data = new SortedDictionary<int, Many2>();
        }
        public Row(Row r)
        {
            point = r.point;
            data = new SortedDictionary<int, Many2>();
            foreach (KeyValuePair<int, Many2> m in r.data) data.Add(m.Key, new Many2(m.Value));
        }

        public static Row load(BinaryReader file)
        {
            Row ret = new Row();
            ret.point = file.ReadInt32();
            int i = 0, cnt = file.ReadInt32();
            while (i < cnt)
            {
                ret.data.Add(file.ReadInt32(), Many2.load(file));
                i++;
            }
            return ret;
        }
        public void save(BinaryWriter file)
        {
            file.Write((Int32)(point));
            file.Write((Int32)(data.Count));
            foreach (KeyValuePair<int, Many2> m in data)
            {
                file.Write((Int32)(m.Key));
                m.Value.save(file);
            }
        }

        public void set(Row r)
        {
            point = r.point;
            data.Clear();
            foreach (KeyValuePair<int, Many2> m in r.data) data.Add(m.Key, new Many2(m.Value));
        }
        Many2 step(Many2 ind, int exp, Vals val)
        {
            Many2 r = new Many2(ind);
            r.replace(IDS.root.v_x.vals[0], new Func(val));
            r.replace(IDS.root.v_n.vals[0], new Func(new Complex(exp)));
            Complex n = r.simple();
            if (n != null) return new Many2(n); else return r;
        }
        public Many2 prep_calc(Vals val, int steps)
        {
            int len = data.Count - point;
            if (len < 1) IDS.root.sys.error("row: empty");
            Many2 calc = new Many2(new Complex(0)), t;
            int i0 = 0; while (i0 < point) { calc.add(step(data[i0], i0, val)); i0++; }
            int e = point, i1 = 0; while (i1 < steps)
            {
                i0 = 0; while (i0 < len)
                {
                    t = step(data[point + i0], e, val);
                    calc.add(t);
                    i0++; e++;
                }
                i1++;
            }
            calc.simple();
            calc.toint();
            return calc;
        }
        public bool expand(Func val, Exps_f exu, Exps_f exd)
        {
            bool r = false;
            foreach (KeyValuePair<int, Many2> m in data) r = m.Value.expand(val, exu, exd) || r;
            return r;
        }
        public void expand()
        {
            foreach (KeyValuePair<int, Many2> m in data) m.Value.expand();
        }
        public void simple()
        {
            foreach (KeyValuePair<int, Many2> m in data) m.Value.simple();
        }

        public void deeper(int d)
        {
            foreach (KeyValuePair<int, Many2> m in data) m.Value.deeper(d);
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return data.ElementAt(0).Value.CompareTo(null);
            Row r = obj as Row; int t = 0;
            if (data.Count != r.data.Count) return (data.Count < r.data.Count ? -1 : +1);
            foreach (KeyValuePair<int, Many2> m in data)
            {
                t = m.Value.CompareTo(r.data[m.Key]);
                if (t != 0) return t;
            }
            return t;
        }
        public void findvals(One o)
        {
            foreach (KeyValuePair<int, Many2> m in data) m.Value.findvals(o);
        }
    }
    public class Matrix : IComparable
    {
        public Func[,] data;
        public int y, x;
        public Matrix(int _y, int _x)
        {
            y = _y; x = _x;
            if ((y == 0) || (x == 0)) IDS.root.sys.error("matr: 0 size");
            data = new Func[y, x];
        }
        public Matrix(Matrix m)
        {
            y = m.y; x = m.x;
            data = new Func[y, x];
            int ix, iy = 0; while (iy < y)
            {
                ix = 0; while (ix < x)
                {
                    data[iy, ix] = new Func(m.data[iy, ix]);
                    ix++;
                }
                iy++;
            }
        }
        public Matrix(Matrix m, int _y)
        {
            y = m.y - 1; x = m.x - 1;
            data = new Func[y, x];
            int ix, iy = 0; while (iy < y)
            {
                ix = 0; while (ix < x)
                {
                    data[iy, ix] = new Func(m.data[(iy < _y ? iy : iy + 1), ix + 1]);
                    ix++;
                }
                iy++;
            }
        }
        public Matrix(List<List<Func>> tm)
        {
            y = tm.Count; x = tm[0].Count;
            if ((x == 0) || (y == 0)) IDS.root.sys.error("matr: 0 size");
            data = new Func[y, x];
            int ix, iy = 0; while (iy < y)
            {
                ix = 0; while (ix < x)
                {
                    data[iy, ix] = tm[iy][ix];
                    ix++;
                }
                iy++;
            }
        }
        public static Matrix load(BinaryReader file)
        {
            Matrix ret = new Matrix(file.ReadInt32(), file.ReadInt32());
            int ix, iy = 0; while (iy < ret.y)
            {
                ix = 0; while (iy < ret.x)
                {
                    ret.data[iy, ix] = Func.load(file);
                    ix++;
                }
                iy++;
            }
            return ret;
        }
        public void save(BinaryWriter file)
        {
            file.Write((Int32)y);
            file.Write((Int32)x);
            foreach (Func _f in data) _f.save(file);
        }
        public void add(Func m)
        {
            foreach (Func _f in data) _f.add(m);
        }
        public void mul(Func m)
        {
            foreach (Func _f in data) _f.mul(m);
        }
        public void mul(Complex m)
        {
            foreach (Func _f in data) _f.mul(m);
        }
        public void common(Func m)
        {
            foreach (Func _f in data) _f.common(m);
        }
        public void add(Matrix m)
        {
            if ((y != m.y) || (x != m.x)) IDS.root.sys.error("matr: wrong add");
            int ix, iy = 0; while (iy < y)
            {
                ix = 0; while (ix < m.x)
                {
                    data[iy, ix].add(m.data[iy, ix]);
                    ix++;
                }
                iy++;
            }
        }
        public void mul(Matrix m)
        {
            if (y != m.x) IDS.root.sys.error("matr: wrong mul");
            Func[,] res = new Func[y, m.x];
            Func tmp;
            int n, ix, iy = 0; while (iy < y)
            {
                ix = 0; while (ix < m.x)
                {
                    res[iy, ix] = new Func(data[iy, 0]);
                    res[iy, ix].mul(m.data[0, ix]);
                    n = 1; while (n < x)
                    {
                        tmp = new Func(data[iy, n]);
                        tmp.mul(m.data[n, ix]);
                        res[iy, ix].add(tmp);
                        n++;
                    }
                    iy++;
                }
                ix++;
            }
        }
        public void neg()
        {
            foreach (Func _f in data) _f.neg();
        }
        public bool expand(Func val, Exps_f exu, Exps_f exd)
        {
            bool ret = false;
            foreach (Func _f in data) ret = _f.expand(val, exu, exd) || ret;
            return ret;
        }
        public void expand()
        {
            foreach (Func _f in data) _f.expand();
        }
        public void deeper(int d)
        {
            foreach (Func _f in data) _f.deeper(d);
        }
        public void findvals(One o)
        {
            foreach (Func _f in data) _f.findvals(o);
        }
        static public Func<Matrix, Many>[] _det = {
              (Matrix t) => {return new Many();},
              (Matrix t) => {return new Many(new One(new Func(t.data[0,0])));},
              (Matrix t) => {
                  One _o = new One(new Func(t.data[1,0]));
                  _o.addto(new Func(t.data[0,1]));
                  Many ret = new Many(_o,new Complex(-1));
                  _o = new One(new Func(t.data[0,0]));
                  _o.addto(new Func(t.data[1,1]));
                  ret.add(_o);
                  return ret;
              },
              (Matrix t) => {
                  Many ret = new Many(), tmp;
                  int i = 0; while (i < t.x) {
                      tmp = new Matrix(t,i).det();
                      tmp.mul(new One(new Func(t.data[i,0])),new Complex((i & 1) == 0 ? 1 : -1));
                      ret.add(tmp);
                      i++;
                  }
                  return ret;
              }
        };

        public Many det()
        {
            if (x != y) IDS.root.sys.error("matr: wrong det");
            return Matrix._det[x < 4 ? x : 3](this);
        }
        public void replace(Vals v, Func f)
        {
            foreach (Func _f in data) _f.replace(v, f);
        }
        public void simple()
        {
            foreach (Func _f in data) _f.simple();
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return data[0, 0].CompareTo(null);
            Matrix m = obj as Matrix;
            int my = (y > m.y ? y : m.y);
            int mx = (x > m.x ? x : m.x);
            int ret, ix, iy;
            iy = 0; while (iy < my)
            {
                ix = 0; while (ix < mx)
                {
                    if ((iy < y) && (ix < x)) ret = data[y, x].CompareTo(((iy < m.y) && (ix < m.x)) ? m.data[y, x] : null);
                    else ret = -m.data[y, x].CompareTo(((iy < y) && (ix < x)) ? data[y, x] : null);
                    if (ret != 0) return ret;
                    ix++;
                }
                iy++;
            }
            return 0;
        }
    }
    public class Func : Power<Func>, IPower, IComparable
    {
        public const int types = 9, t_val = 0, t_num = 1, t_many2 = 2, t_ln = 3, t_fact = 4, t_int = 5, t_sign = 6, t_row = 7, t_matr = 8;
        public static Func zero;
        public Object data;
        public int type; //0: &val, 1: &Complex, 2: &many2, 3: ln(Many2), 4: fact(Many2), 5: int(Many2), 6: sign(Many2), 7: row(Row), 8: matrix(func[][])
        public Func()
        {
            type = -1; data = null;
        }
        public Func(BinaryReader file)
        {
            load(file);
        }
        static public Action<Func, BinaryReader>[] load_func = {
              (Func t, BinaryReader f) => t.data = Vals.inds[f.ReadInt32()],
              (Func t, BinaryReader f) => t.data = Complex.load(f),
              (Func t, BinaryReader f) => t.data = Many2.load(f),

              (Func t, BinaryReader f) => t.data = Many2.load(f),
              (Func t, BinaryReader f) => t.data = Many2.load(f),
              (Func t, BinaryReader f) => t.data = Many2.load(f),
              (Func t, BinaryReader f) => t.data = Many2.load(f),
              (Func t, BinaryReader f) => t.data = Row.load(f),
              (Func t, BinaryReader f) => t.data = Matrix.load(f)
        };
        public static Func load(BinaryReader file)
        {
            Func ret = new Func();
            ret.type = file.ReadByte();
            Func.load_func[ret.type](ret, file);
            return ret;
        }
        static public Action<Func, BinaryWriter>[] save_func = {
              (Func t, BinaryWriter f) => f.Write(((Vals)(t.data)).ind),
              (Func t, BinaryWriter f) => ((Complex)(t.data)).save(f),
              (Func t, BinaryWriter f) => ((Many2)(t.data)).save(f),
              (Func t, BinaryWriter f) => ((Many2)(t.data)).save(f),
              (Func t, BinaryWriter f) => ((Many2)(t.data)).save(f),
              (Func t, BinaryWriter f) => ((Many2)(t.data)).save(f),
              (Func t, BinaryWriter f) => ((Many2)(t.data)).save(f),
              (Func t, BinaryWriter f) => ((Row)(t.data)).save(f),
              (Func t, BinaryWriter f) => ((Matrix)(t.data)).save(f)
        };
        public void save(BinaryWriter file)
        {
            file.Write((byte)type);
            Func.save_func[type](this, file);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Vals v)
        {
            type = Func.t_val; data = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Complex n)
        {
            type = Func.t_num; data = n;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(One o, Complex n)
        {
            type = Func.t_many2; data = new Many2(o, n);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Row r)
        {
            type = Func.t_row; data = r;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Matrix m)
        {
            type = Func.t_matr; data = m;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Many2 m)
        {
            type = Func.t_many2; data = m;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(int t, Many2 m)
        {
            type = t; data = m;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Func f)
        {
            type = f.type;
            Func.set_func[f.type](this, f);
        }
        static public Action<Func, Func>[] set_func = {
              (Func t, Func f) => {t.data = f.data;},
              (Func t, Func f) => {t.data = new Complex((Complex)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Row((Row)(f.data));},
              (Func t, Func f) => {t.data = new Matrix((Matrix)(f.data));}
                         };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void set(Func f)
        {
            Func.set_func[f.type](this, f);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void copy(ref Func f)
        {
            Func.set_func[type](f, this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void set0()
        {
            type = Func.t_num; data = new Complex(0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void set1()
        {
            type = Func.t_num; data = new Complex(1);
        }
        static public Func<Func, Complex>[] get_num_part_func = {
                (Func t) => {return new Complex(0);},
                (Func t) => {return (Complex)(t.data);},
                (Func t) => {return ((Many2)(t.data)).get_num_part();},
                (Func t) => {return new Complex(0);},
                (Func t) => {return new Complex(0);},
                (Func t) => {return new Complex(0);},
                (Func t) => {return new Complex(0);},
                (Func t) => {return new Complex(0);},
                (Func t) => {return new Complex(0);}
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex get_num_part()
        {
            return Func.get_num_part_func[type](this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isconst(int r, int i) { return (type == Func.t_num ? ((Complex)data).isint(r, i) : false); }
        static public Func<Func, int>[] type_pow_func = {
                (Func t) => {return 2;},
                (Func t) => {return (((Complex)(t.data)).i.sign == 0 ? (((Complex)(t.data)).isint() ? 0 : 1) : 2);},
                (Func t) => {return 2;},
                (Func t) => {return 2;},
                (Func t) => {return 2;},
                (Func t) => {return 2;},
                (Func t) => {return 2;},
                (Func t) => {return 2;},
                (Func t) => {return 2;}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int type_pow()
        {
            return Func.type_pow_func[type](this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void revert(Func val)
        {
            if (type == Func.t_many2) ((Many2)data).revert(val);
        }
        static public Action<Func, Vals, Func>[] repl_func = {
                (Func t, Vals v, Func f) => {
                    if (((Vals)(t.data) == v)) {t.type = f.type; t.data = f.data;}
                },
                (Func t, Vals v, Func f) => {},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {},
                (Func t, Vals v, Func f) => {((Matrix)(t.data)).replace(v,f);},
         };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void replace(Vals v, Func f)
        {
            Func.repl_func[type](this, v, f);
        }

        static public Func<Func, Func, Exps_f, Exps_f, bool>[] expand2_func = {
                (Func t, Func v, Exps_f u, Exps_f d) => {if (t.data == v.data) {t.type = Func.t_many2; t.data = new Many2(new Many(u.mvar),new Many(d.mvar)); return true;} else return false;},
                (Func t, Func v, Exps_f u, Exps_f d) => {return false;},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Row)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Matrix)(t.data)).expand(v,u,d);}
            };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool expand(Func val, Exps_f exu, Exps_f exd)
        {
            return Func.expand2_func[type](this, val, exu, exd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool expand(Func fv)
        {
            if (fv.type != Func.t_val) return false;
            Vals v = (Vals)(fv.data);
            Func f_exp = v.var.var;
            if ((f_exp == null) || (f_exp.type == Func.t_num)) return false;
            Exps_f exu, exd;
            if (f_exp.type == Func.t_many2)
            {
                exu = new Exps_f(((Many2)(f_exp.data)).up, v.deep);
                exd = new Exps_f(((Many2)(f_exp.data)).down, v.deep);
            }
            else
            {
                exu = new Exps_f(new Many(new Func(f_exp)), v.deep);
                exd = new Exps_f(new Many(new Complex(1)), v.deep);
            }
            bool ret = Func.expand2_func[type](this, fv, exu, exd);

            return ret;
        }
        static public Action<Func>[] expand_func = {
                (Func t) => {},
                (Func t) => {},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Row)(t.data)).expand();},
                (Func t) => {((Matrix)(t.data)).expand();}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void expand()
        {
            Func.expand_func[type](this);
        }
        static public Action<Func>[] neg_func = {
                (Func t) => {
                    t.data = new Many2(new Many(new One(new Func(t))));
                    t.type = Func.t_many2; ((Many2)(t.data)).neg();
                },
                (Func t) => {((Complex)(t.data)).neg();},
                (Func t) => {((Many2)(t.data)).neg();},
                (Func t) => {},
                (Func t) => {},
                (Func t) => {},
                (Func t) => {},
                (Func t) => {},
                (Func t) => {((Matrix)(t.data)).neg();}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void neg()
        {
            Func.neg_func[type](this);
        }
        static public Action<Func>[] div_func = {
                (Func t) => {
                    t.data = new Many2(new Many(new One(new Func(t))));
                    t.type = 2; ((Many2)(t.data)).div();
                },
                (Func t) => {((Complex)(t.data)).div();},
                (Func t) => {((Many2)(t.data)).div();},
                (Func t) => {},
                (Func t) => {},
                (Func t) => {},
                (Func t) => {},
                (Func t) => {},
                (Func t) => {}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void div()
        {
            Func.div_func[type](this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _mul(Func t, Func f)
        {
            One _o = new One(new Func(t)); _o.addto(new Func(f), new Func(new Complex(1)));
            t.type = Func.t_many2; t.data = new Many2(new Many(_o));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _mul(Func t, Complex f)
        {
            One _o = new One(new Func(t));
            t.type = Func.t_many2; t.data = new Many2(new Many(_o, new Complex(f)));
        }
        static public Action<Func, Func>[] mul_func = {
                (Func t, Func f) => {//0:0
                    Func._mul(t,f);
                },
                (Func t, Func f) => {
                    if (((Complex)(f.data)).iszero()) {
                        t.type = Func.t_num; t.data = new Complex(0);
                    } else if (! ((Complex)(f.data)).isint(1,0)) Func._mul(t,(Complex)(f.data));
                },
                (Func t, Func f) => {
                    One _o = new One(new Func(t));
                    t.type = Func.t_many2; t.data = new Many2((Many2)(f.data));
                    ((Many2)(t.data)).mul(_o,Complex._r1);
                },
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.mul(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => {//1 *= 0
                    if (! ((Complex)(t.data)).iszero()) {
                        if (((Complex)(t.data)).isint(1,0)) {
                            t.type = f.type; t.data = f.data; //point to var
                        } else Func._mul(t,(Complex)(f.data));
                    }
                },
                (Func t, Func f) => {  //1 *= 1
                    ((Complex)(t.data)).mul((Complex)(f.data));
                },
                (Func t, Func f) => { //1 *= 2
                    if (! ((Complex)(t.data)).iszero()) {
                        if (((Complex)(t.data)).isint(1,0)) {
                            t.type = Func.t_many2; t.data = new Many2((Many2)(f.data));
                        } else {
                            t.type = Func.t_many2; t.data = new Many2(new Complex((Complex)(t.data)));
                            ((Many2)(t.data)).mul((Many2)f.data);
                        }
                    }
                },
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.mul((Complex)(t.data));
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => {//2 *= 0
                    ((Many2)(t.data)).mul(new One(new Func(f)),Complex._r1);
                },
                (Func t, Func f) => {  //2 *= 1
                    if (! ((Complex)(f.data)).isint(1,0)) {
                        if (((Complex)(f.data)).iszero()) {
                            t.type = f.type; t.data = new Complex(0);
                        } else ((Many2)(t.data)).mul((Complex)f.data);
                    }
                },
                (Func t, Func f) => { //2 *= 2
                    ((Many2)(t.data)).mul((Many2)f.data);
                },
                (Func t, Func f) => {((Many2)(t.data)).mul(new One(f),Complex._r1);},
                (Func t, Func f) => {((Many2)(t.data)).mul(new One(f),Complex._r1);},
                (Func t, Func f) => {((Many2)(t.data)).mul(new One(f),Complex._r1);},
                (Func t, Func f) => {((Many2)(t.data)).mul(new One(f),Complex._r1);},
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.mul(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => {Func._mul(t,f);},//3 *= 0
                (Func t, Func f) => {
                    Func._mul(t,(Complex)(f.data));
                },
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.mul(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => {Func._mul(t,f);},//4 *= 0
                (Func t, Func f) => {
                    Func._mul(t,(Complex)(f.data));
                },
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.mul(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => {Func._mul(t,f);},//5 *= 0
                (Func t, Func f) => {
                    Func._mul(t,(Complex)(f.data));
                },
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.mul(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => {Func._mul(t,f);},//6 *= 0
                (Func t, Func f) => {
                    Func._mul(t,(Complex)(f.data));
                },
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {Func._mul(t,f);},
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.mul(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => {},//7 *= 0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {((Matrix)(t.data)).mul(f);},//8 *= 0
                (Func t, Func f) => {((Matrix)(t.data)).mul((Complex)(f.data));},
                (Func t, Func f) => {((Matrix)(t.data)).mul(f);},
                (Func t, Func f) => {((Matrix)(t.data)).mul(f);},
                (Func t, Func f) => {((Matrix)(t.data)).mul(f);},
                (Func t, Func f) => {((Matrix)(t.data)).mul(f);},
                (Func t, Func f) => {((Matrix)(t.data)).mul(f);},
                (Func t, Func f) => {},
                (Func t, Func f) => {((Matrix)(t.data)).mul(((Matrix)(f.data)));

                }
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void mul(Func f)
        {
            Func.mul_func[type * Func.types + f.type](this, f);
        }
        static public Action<Func, Complex>[] muln_func = {
                (Func t, Complex n) => {Func._mul(t,n);},
                (Func t, Complex n) => { 
                    ((Complex)(t.data)).mul(n);
                },
                (Func t, Complex n) => { 
                    ((Many2)(t.data)).mul(n);
                },
                (Func t, Complex n) => {Func._mul(t,n);},
                (Func t, Complex n) => {Func._mul(t,n);},
                (Func t, Complex n) => {Func._mul(t,n);},
                (Func t, Complex n) => {Func._mul(t,n);},
                (Func t, Complex n) => {},
                (Func t, Complex n) => { 
                    ((Matrix)(t.data)).mul(n);
                }
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void mul(Complex n)
        {
            if (!n.isint(1, 0))
            {
                if (n.iszero())
                {
                    type = Func.t_num; data = new Complex(0);
                }
                else
                {
                    Func.muln_func[type](this, n);
                }
            }
        }
        static void _zero(Func t)
        {
            t.type = Func.t_num; t.data = new Complex(0);
        }
        static public Action<Func, Func>[] common_func = {
                (Func t, Func f) => {//0:0
                    if (t.data != f.data) Func._zero(t);
                },
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => {
                    One _o = new One(new Func(t));
                    Complex _nd = ((Many2)(f.data)).down.get_Num(); _nd.div();
                    if ((_nd != null) && ((Many2)(f.data)).up.data.ContainsKey(_o) && (((Many2)(f.data)).up.data[_o].sign() * _nd.sign() > 0)) {
                        _nd.mul(((Many2)(f.data)).up.data[_o]);
                        if (_nd.great(Complex._r1)) {
                            Many _u = new Many(); _u.data.Add(_o,_nd);
                            t.type = Func.t_many2; t.data = new Many2(_u,new Many(new Complex(1)));
                        }
                    } else Func._zero(t);
                },
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => {},
                (Func t, Func f) => Func._zero(t),

                (Func t, Func f) => Func._zero(t), //1:0
                (Func t, Func f) => { //1 1
                        t.data = Complex.common((Complex)(t.data),(Complex)(f.data));
                },
                (Func t, Func f) => { //1 2
                        t.data = Complex.common((Complex)(t.data),((Many2)(f.data)).get_num_part());
                },
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => {},
                (Func t, Func f) => Func._zero(t),

                (Func t, Func f) => {//2:0
                    One _o = new One(new Func(f));
                    Complex _nd = ((Many2)(t.data)).down.get_Num(); _nd.div();
                    if ((_nd != null) && ((Many2)(t.data)).up.data.ContainsKey(_o) && (((Many2)(t.data)).up.data[_o].sign() * _nd.sign() > 0)) {
                        _nd.mul(((Many2)(t.data)).up.data[_o]);
                        if (_nd.great(new Complex(1))) {
                            ((Many2)(t.data)).up = new Many(_o,_nd);
                            ((Many2)(t.data)).down = new Many(new Complex(1));
                        } else {
                            t.type = Func.t_val; t.data = f.data;
                        }
                    } else Func._zero(t);
                },
                (Func t, Func f) => { //2:1
                        t.type = 1; t.data = Complex.common(((Many2)(t.data)).get_num_part(),(Complex)(f.data)); 
                },
                (Func t, Func f) => { //2:2
                        ((Many2)(t.data)).common((Many2)(f.data));
                        Complex r = ((Many2)(t.data)).get_Num();
                        t.type = Func.t_num; t.data = (r == null ? new Complex(0) : r);
                },
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => {},
                (Func t, Func f) => Func._zero(t),

                (Func t, Func f) => Func._zero(t), //3:0
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => {},
                (Func t, Func f) => Func._zero(t),

                (Func t, Func f) => Func._zero(t), //4:0
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => {},
                (Func t, Func f) => Func._zero(t),

                (Func t, Func f) => Func._zero(t), //5:0
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => {},
                (Func t, Func f) => Func._zero(t),

                (Func t, Func f) => Func._zero(t), //6:0
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => {},
                (Func t, Func f) => Func._zero(t),

                (Func t, Func f) => {}, //7:0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => ((Matrix)(t.data)).common(f), //8:0
                (Func t, Func f) => ((Matrix)(t.data)).common(f),
                (Func t, Func f) => ((Matrix)(t.data)).common(f),

                (Func t, Func f) => ((Matrix)(t.data)).common(f),
                (Func t, Func f) => ((Matrix)(t.data)).common(f),
                (Func t, Func f) => ((Matrix)(t.data)).common(f),
                (Func t, Func f) => ((Matrix)(t.data)).common(f),
                (Func t, Func f) => Func._zero(t),
                (Func t, Func f) => Func._zero(t)
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void common(Func f)
        {
            Func.common_func[type * Func.types + f.type](this, f);
        }

        static void _add(Func t, Func f)
        {
            Many _u = new Many(new Func(t));
            One _o = new One(new Func(f));
            _u.add(_o);
            t.type = Func.t_many2; t.data = new Many2(_u);
        }
        static void _add(Many2 t, Func f)
        {
            Many _u = new Many(); _u.data.Add(new One(new Func(f)), new Complex(1));
            t.add(new Many2(_u, new Many(new Complex(1))));
        }
        static public Action<Func, Func>[] add_func = {
                (Func t, Func f) => Func._add(t,f),//0:0
                (Func t, Func f) => {
                    if (! ((Complex)(f.data)).iszero()) {
                        Many _u = new Many(new Func(t));
                        _u.add(new One(),(Complex)(f.data));
                        t.type = Func.t_many2; t.data = new Many2(_u);
                    }
                },
                (Func t, Func f) => {
                    Many2 _tm = new Many2(new One(new Func(t)));
                    _tm.add((Many2)(f.data));
                    t.type = Func.t_many2; t.data = _tm;
                },
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.add(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => {//1:0
                    if (((Complex)(t.data)).iszero()) {
                        t.type = Func.t_val; t.data = f.data; //pnt to var
                    } else {
                        Many _u = new Many(new One(new Func(f)));
                        _u.add(new One(),(Complex)(t.data));
                        t.type = Func.t_many2; t.data = new Many2(_u);
                    }
                },
                (Func t, Func f) => { //1:1
                    ((Complex)(t.data)).add((Complex)(f.data));
                },
                (Func t, Func f) => { //1:2
                    Many2 rez = new Many2((Many2)(f.data));
                    rez.add(new Many2((Complex)(t.data)));
                    t.type = Func.t_many2; t.data = rez;
                },
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.add(t);
                    t.type = Func.t_matr; t.data = tm;
                },



                (Func t, Func f) => Func._add((Many2)(t.data),f),//2:0
                (Func t, Func f) => { //2:1
                    if (! ((Complex)(f.data)).iszero()) {
                        ((Many2)(t.data)).add((Complex)f.data);
                    }
                },
                (Func t, Func f) => { //2:2
                    ((Many2)(t.data)).add((Many2)(f.data));
                },
                (Func t, Func f) => Func._add((Many2)(t.data),f),
                (Func t, Func f) => Func._add((Many2)(t.data),f),
                (Func t, Func f) => Func._add((Many2)(t.data),f),
                (Func t, Func f) => Func._add((Many2)(t.data),f),
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.add(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => Func._add(t,f), //3:0
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.add(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => Func._add(t,f), //4:0
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.add(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => Func._add(t,f), //5:0
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.add(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => Func._add(t,f), //6:0
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => Func._add(t,f),
                (Func t, Func f) => {},
                (Func t, Func f) => {
                    Matrix tm = new Matrix((Matrix)(f.data));
                    tm.add(t);
                    t.type = Func.t_matr; t.data = tm;
                },

                (Func t, Func f) => {}, //7:0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},


                (Func t, Func f) => ((Matrix)(t.data)).add(f), //8:0
                (Func t, Func f) => ((Matrix)(t.data)).add(f),
                (Func t, Func f) => ((Matrix)(t.data)).add(f),
                (Func t, Func f) => ((Matrix)(t.data)).add(f),
                (Func t, Func f) => ((Matrix)(t.data)).add(f),
                (Func t, Func f) => ((Matrix)(t.data)).add(f),
                (Func t, Func f) => ((Matrix)(t.data)).add(f),
                (Func t, Func f) => {},
                (Func t, Func f) => ((Matrix)(t.data)).add((Matrix)(f.data))

              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void add(Func f)
        {
            Func.add_func[type * Func.types + f.type](this, f);
        }

        static public Func<Func, Func, int>[] comp_func = {
                (Func t, Func f) => {return (t.data == f.data ? 0 : (((Vals)(t.data)).ind < ((Vals)(f.data)).ind ? -1 : 1));}, //0:0
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},

                (Func t, Func f) => {return -1;}, //1:0
                (Func t, Func f) => {return ((Complex)(t.data)).CompareTo(f.data);},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return ((Complex)(t.data)).CompareTo(null);},

                (Func t, Func f) => {return -1;}, //2:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(f.data);},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(null);},

                (Func t, Func f) => {return -1;}, //3:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(f.data);},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(null);},

                (Func t, Func f) => {return -1;}, //4:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(f.data);},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(null);},

                (Func t, Func f) => {return -1;}, //5:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(f.data);},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(null);},

                (Func t, Func f) => {return -1;}, //6:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo((Many2)(f.data));},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(null);},

                (Func t, Func f) => {return -1;}, //7:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Row)(t.data)).CompareTo((Row)(f.data));},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},

                (Func t, Func f) => {return -1;}, //8:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Matrix)(t.data)).CompareTo((Matrix)(f.data));},
                (Func t, Func f) => {return 1;}

               };
        public int CompareTo(object obj)
        {
            Func f = obj as Func;
            int t = (f == null ? Func.types : f.type);
            return Func.comp_func[type * (Func.types + 1) + f.type](this, f);
        }

        static public Action<Func>[] simple_func = {
                (Func t) => {},
                (Func t) => {((Complex)(t.data)).simple();},
                (Func t) => {
                    Complex r = ((Many2)(t.data)).simple();
                    if (r != null) {t.data = r; t.type = 1;} else {
                        Func _t = ((Many2)(t.data)).get_Func();
                        if (_t != null) {
                            t.data = _t.data; t.type = _t.type;
                        }
                    }
                },
                (Func t) => {
                    ((Many2)(t.data)).simple();
                },
                (Func t) => {
                    ((Many2)(t.data)).simple();
                    Complex n = ((Many2)(t.data)).get_Num();
                    if (n != null) {
                        t.data = Func.f_fact(n,t); t.type = 1;
                    }
                },
                (Func t) => {
                    ((Many2)(t.data)).simple();
                    Complex n = ((Many2)(t.data)).get_Num();
                    if (n != null) {
                        t.type = 1; t.data = Func.f_int(n);
                    }
                },
                (Func t) => {
                    ((Many2)(t.data)).simple();
                    Complex n = ((Many2)(t.data)).get_Num();
                    if (n != null) {
                        t.type = 1; t.data = Func.f_sign(n);
                    }
                },
                (Func t) => {((Row)(t.data)).simple();},
                (Func t) => {((Matrix)(t.data)).simple();}
                              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void simple()
        {
            Func.simple_func[type](this);
        }

        static public Func<Func, int>[] sign_func = {
                (Func t) => {return 1;},
                (Func t) => {return ((Complex)(t.data)).sign();},
                (Func t) => {return ((Many2)(t.data)).sign();},
                (Func t) => {return 1;},
                (Func t) => {return 1;},
                (Func t) => {return 1;},
                (Func t) => {return 1;},
                (Func t) => {return 1;},
                (Func t) => {return 1;}
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int sign()
        {
            return Func.sign_func[type](this);
        }


        static public Action<Func, int>[] deeper_func = {
                (Func t, int d) => {
                    int _i; if ((_i = ((Vals)(t.data)).deep + d) >= ((Vals)(t.data)).var.vals.Length) IDS.root.sys.error("too deep");
                    t.data = ((Vals)(t.data)).var.vals[_i];
                },
                (Func t, int d) => {},
                (Func t, int d) => ((Many2)(t.data)).deeper(d),
                (Func t, int d) => ((Many2)(t.data)).deeper(d),
                (Func t, int d) => ((Many2)(t.data)).deeper(d),
                (Func t, int d) => ((Many2)(t.data)).deeper(d),
                (Func t, int d) => ((Many2)(t.data)).deeper(d),
                (Func t, int d) => ((Row)(t.data)).deeper(d),
                (Func t, int d) => ((Matrix)(t.data)).deeper(d)
               };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void deeper(int d)
        {
            Func.deeper_func[type](this, d);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func deeper(Func f, int d)
        {
            Func r = new Func(f);
            Func.deeper_func[r.type](r, d);
            return r;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex f_ln(Complex n, Func t)
        {
            IDS.now_func = t;
            Complex r = new Complex(n); r.ln(); return r;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex f_fact(Complex n, Func t)
        {
            IDS.now_func = t;
            if ((n.i.sign != 0) || (n.r.sign < 1) || (!n.r.isint()) || (n.r.up >= IDS.root.fact.Count())) IDS.root.sys.error("fact: not yet");
            return new Complex(IDS.root.fact[(int)n.r.up]);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex f_int(Complex n)
        {
            if ((n.r.down == 1) && (n.i.down == 1)) return n;
            return new Complex(new Num(n.r.sign * n.r.up / n.r.down), new Num(n.i.sign * n.i.up / n.i.down));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex f_sign(Complex n)
        {
            return new Complex(IDS.nums[n.r.sign + IDS.znums], IDS.nums[n.i.sign + IDS.znums]);
        }
        static public Func<Func, Complex>[] calc_func = {
                (Func t) => {return ((Vals)(t.data)).get_val(); },
                (Func t) => {return (Complex)(t.data);},
                (Func t) => {return ((Many2)(t.data)).calc();},
                (Func t) => {return Func.f_ln(((Many2)(t.data)).calc(),t);},
                (Func t) => {return Func.f_fact(((Many2)(t.data)).calc(),t);},
                (Func t) => {return Func.f_int(((Many2)(t.data)).calc());},
                (Func t) => {return Func.f_sign(((Many2)(t.data)).calc());},
                (Func t) => {
                    IDS.root.sys.error("row:not prep");
                    return new Complex(0);
                },
                (Func t) => {
                    IDS.root.sys.error("matr:not prep");
                    return new Complex(0);
                }
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex calc()
        {
            return Func.calc_func[type](this);
        }

        static public Func<Func, Complex, Complex>[] calce_func = {
                (Func t, Complex e) => {return ((Vals)(t.data)).get_val(e); },
                (Func t, Complex e) => {return Complex.exp((Complex)(t.data),e);},
                (Func t, Complex e) => {return Complex.exp(((Many2)(t.data)).calc(),e);},
                (Func t, Complex e) => {
                    return Complex.exp(Func.f_ln(((Many2)(t.data)).calc(),t),e);
                },
                (Func t, Complex e) => {
                    return Complex.exp(Func.f_fact(((Many2)(t.data)).calc(),t),e);
                },
                (Func t, Complex e) => {
                    return Complex.exp(Func.f_int(((Many2)(t.data)).calc()),e);
                },
                (Func t, Complex e) => {
                    int s = ((Many2)(t.data)).calc().sign();
                    return new Complex((e.r.up & 1) == 0 ? s*s : s);
                },
                (Func t, Complex e) => {
                    IDS.root.sys.error("row: not prep");
                    return new Complex(0);
                },
                (Func t, Complex e) => {
                    IDS.root.sys.error("matr: not prep");
                    return new Complex(0);
                }
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex calc(Complex e)
        {
            return (e.iszero() ? new Complex(1) : (e.isequ(Complex._r1) ? Func.calc_func[type](this) : Func.calce_func[type](this, e)));
        }
        static public Action<Func, One>[] findvals_func = {
              (Func t, One o) => {if (! o.exps.ContainsKey(t)) o.exps.Add(new Func(t),new Func(new Complex(1)));},
              (Func t, One o) => {},
              (Func t, One o) => ((Many2)(t.data)).findvals(o),
              (Func t, One o) => ((Many2)(t.data)).findvals(o),
              (Func t, One o) => ((Many2)(t.data)).findvals(o),
              (Func t, One o) => ((Many2)(t.data)).findvals(o),
              (Func t, One o) => ((Many2)(t.data)).findvals(o),
              (Func t, One o) => ((Row)(t.data)).findvals(o),
              (Func t, One o) => ((Matrix)(t.data)).findvals(o)
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void findvals(One o)
        {
            Func.findvals_func[type](this, o);
        }

        static public Action<Func, Vals>[] diff_down_func = {
              (Func t, Vals a) => {if (t.data == a) t.set1(); else t.set0();},
              (Func t, Vals a) => {t.set0();},
              (Func t, Vals a) => {t.data = ((Many2)(t.data)).diff_down(a);},
              (Func t, Vals a) => {},

              (Func t, Vals a) => {},
              (Func t, Vals a) => {},
              (Func t, Vals a) => {},
              (Func t, Vals a) => {},
              (Func t, Vals a) => {}
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void diff_down(Vals d)
        {
            Func.diff_down_func[type](this, d);
        }


        static public Action<Func, Vals>[] diff_up_func = {
              (Func t, Vals a) => {if (t.data == a) {
                      t.data = new Many2(new One(new Func(a),new Func(new Complex(2))),new Complex(new Num(1,2)));
                  } else {
                      t.data = new Many2(new Func(t)); ((Many2)(t.data)).mul(new One(new Func(a)),Complex._r1);
                  }
                  t.type = Func.t_many2;
              },
              (Func t, Vals a) => {t.data = new Many2(new Many(new One(new Func(a)),(Complex)(t.data))); t.type = Func.t_many2;},
              (Func t, Vals a) => {t.data = ((Many2)(t.data)).diff_up(a);},
              (Func t, Vals a) => {},

              (Func t, Vals a) => {},
              (Func t, Vals a) => {},
              (Func t, Vals a) => {},
              (Func t, Vals a) => {},
              (Func t, Vals a) => {}
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void diff_up(Vals d)
        {
            Func.diff_up_func[type](this, d);
        }

        static public Func<Func, Func, Vals, Func>[] diffe_down_func = {
                (Func t, Func e, Vals a) => { //0:0
                    One o;
                    if ((t.type == Func.t_val) && (t.data == a)) {
                        if ((e.type == Func.t_val) && (e.data == a)) {
                            o = new One(new Func(t),new Func(e));
                            Many m = new Many(new Complex(1));
                            m.add(new One(new Func(3,new Many2(new Func(t)))),Complex._r1);
                            o.addto(new Func(new Many2(m)));
                        } else {
                            Func tt = new Func(e); tt.add(new Func(new Complex(-1)));
                            o = new One (new Func(t),tt); o.addto(new Func(e)); 
                        }
                    } else  {
                        if ((e.type == Func.t_val) && (e.data == a)) {
                            o = new One(new Func(t),new Func(e)); o.addto(new Func(3,new Many2(new Func(t))));
                        } else {
                            return new Func(new Complex(0));
                        }
                    }
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {//0:1
                    if ((t.type == Func.t_val) && (t.data == a)) {
                        One o = new One (new Func(t),new Func(Complex.sub((Complex)(e.data),new Complex(1))));
                        return new Func(new Many2(o,(Complex)(e.data)));
                    }
                    return new Func(new Complex(0));
                },
                (Func t, Func e, Vals a) => {//0:2
                    Many2 dm = ((Many2)(e.data)).diff_down(a);
                    One o = new One(new Func(3,new Many2(new Func(t))));
                    if ((t.type == Func.t_val) && (t.data == a)) {
                        o.addto(new Func(dm));
                        o.addto(new Func(t));
                        Many2 m0 = new Many2((Many2)(e.data));
                        m0.add(new Many2(o));
                        Many2 m1 = new Many2((Many2)(e.data));
                        m1.add(new Complex(-1));
                        One o0 = new One(new Func(t),new Func(m1));
                        o0.addto(new Func(m0));
                        return new Func(new Many2(o0));
                    } else {
                        o.addto(new Func(dm)); 
                        o.addto(new Func(t),new Func(e));
                        return new Func(new Many2(o));
                    }
                },
                (Func t, Func e, Vals a) => {//0:3
                    Many2 dm = ((Many2)(e.data)).diff_down(a);
                    if ((t.type == Func.t_val) && (t.data == a)) {
                        One o0 = new One(new Func(3,new Many2(new Func(t))));
                        o0.addto(new Func(dm));
                        o0.addto(new Func(t));
                        o0.addto(new Func(new Many2((Many2)(e.data))),new Func(new Complex(-1)));
                        Many m0 = new Many(o0);
                        m0.add(new One(new Func(e)));
                        One o = new One(new Func(new Many2(m0)));
                        Many2 m1 = new Many2(new Func(e));
                        m1.add(new Complex(-1));
                        o.addto(new Func(t),new Func(m1));
                        return new Func(new Many2(o));
                    } else {
                        One o = new One(new Func(3,new Many2(new Func(t))));
                        o.addto(new Func(dm)); 
                        o.addto(new Func(t),new Func(e));
                        o.addto(new Func(new Many2((Many2)(e.data))),new Func(new Complex(-1)));
                        return new Func(new Many2(o));
                    }
                },
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},


                (Func t, Func e, Vals a) => {//1:0
                    if ((e.type == Func.t_val) && (e.data == a)) {
                        One o = new One(new Func(t),new Func(e)); o.addto(new Func(3,new Many2(new Func(t))));
                    }
                    return new Func(new Complex(0));
                },
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},//1:1
                (Func t, Func e, Vals a) => {//1:2
                    Many2 dm = ((Many2)(e.data)).diff_down(a);
                    One o = new One(new Func(3,new Many2(new Func(t))));
                    o.addto(new Func(dm)); 
                    o.addto(new Func(t),new Func(e));
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {//1:3
                    Many2 dm = ((Many2)(e.data)).diff_down(a);
                    One o = new One(new Func(3,new Many2(new Func(t))));
                    o.addto(new Func(dm)); 
                    o.addto(new Func(t),new Func(e));
                    o.addto(new Func(new Many2((Many2)(e.data))),new Func(new Complex(-1)));
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},

                (Func t, Func e, Vals a) => {//2:0
                    Func _e = new Func(e); _e.add(new Func(new Complex(-1)));
                    One o = new One(new Func(t),_e);
                    Many2 dm = ((Many2)(t.data)).diff_down(a);
                    if ((e.type == Func.t_val) && (e.data == a)) {
                        One om = new One(new Func(t)); 
                        om.addto(new Func(3,new Many2((Many2)(t.data))));
                        dm.mul(new One(e),Complex._r1);
                        Many m0 = new Many(om);
                        m0.add(new One(new Func(dm)));
                        o.addto(new Func(new Many2(m0)));
                    } else {
                        o.addto(new Func(dm));
                        o.addto(new Func(e));
                    }
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {//2:1
                    One o = new One(new Func(t),new Func(Complex.sub((Complex)(e.data),new Complex(1))));
                    Many2 dm = ((Many2)(t.data)).diff_down(a);
                    o.addto(new Func(dm));
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {//2:2
                    Many2 dt = ((Many2)(t.data)).diff_down(a);
                    Many2 de = ((Many2)(e.data)).diff_down(a);

                    One o0 = new One(new Func(dt)); o0.addto(new Func(e));
                    One o1 = new One(new Func(de)); o1.addto(new Func(t));
                    o1.addto(new Func(3,new Many2((Many2)(t.data))));
                    Many m = new Many(o0); m.add(o1);

                    Many2 _e = new Many2((Many2)(e.data)); _e.add(new Complex(-1));
                    One o = new One(new Func(t),new Func(_e));

                    o.addto(new Func(new Many2(m)));

                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {//2:3
                    Many2 dt = ((Many2)(t.data)).diff_down(a);
                    Many2 de = ((Many2)(e.data)).diff_down(a);
                    One o0 = new One(new Func(dt)); o0.addto(new Func(e));
                    o0.addto(new Func(3,new Many2((Many2)(t.data))),new Func(new Complex(-1)));
                    One o1 = new One(new Func(de)); o1.addto(new Func(t));
                    o1.addto(new Func(3,new Many2((Many2)(e.data))),new Func(new Complex(-1)));
                    Many m = new Many(o0); m.add(o1);
                    One o = new One(new Func(t),new Func(e));
                    o.addto(new Func(new Many2(m)));
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},

                (Func t, Func e, Vals a) => {//3:0
                    Many2 dm = ((Many2)(t.data)).diff_down(a);
                    if ((e.type == Func.t_val) && (e.data == a)) {
                        One o0 = new One(new Func(t),new Func(new Complex(-1)));
                        o0.addto(new Func(new Many2((Many2)(t.data))),new Func(new Complex(-1)));
                        dm.mul(new One(e),Complex._r1);
                        o0.addto(new Func(dm));
                        Many _m = new Many(new Func(3,new Many2(new Func(t))));
                        _m.add(o0);
                        One o = new One(new Func(t), new Func(e));
                        o.addto(new Func(new Many2(_m)));
                        return new Func(new Many2(o));
                    } else {
                        Many2 m = new Many2((Many2)(t.data));
                        One o = new One(new Func(dm));
                        o.addto(new Func(m),new Func(new Complex(-1)));
                        Many _m = new Many(new Func(e)); _m.add(new One(),Complex._r_1);
                        o.addto(new Func(t),new Func(new Many2(_m)));
                        o.addto(new Func(e));
                        return new Func(new Many2(o));
                    }
                },
                (Func t, Func e, Vals a) => {//3:1
                    Many2 m = new Many2((Many2)(t.data));
                    Many2 dm = ((Many2)(t.data)).diff_down(a);
                    One o = new One(new Func(dm));
                    o.addto(new Func(m),new Func(new Complex(-1)));
                    o.addto(new Func(t),new Func(Complex.sub((Complex)(e.data),new Complex(1))));
                    return new Func(new Many2(o,(Complex)(e.data)));
                },
                (Func t, Func e, Vals a) => {//3:2
                    Many2 dt = ((Many2)(t.data)).diff_down(a);
                    Many2 de = ((Many2)(e.data)).diff_down(a);

                    One o0 = new One(new Func(dt)); o0.addto(new Func(new Many2((Many2)(e.data))));
                    o0.addto(new Func(new Many2((Many2)(t.data))),new Func(new Complex(-1)));
                    o0.addto(new Func(t),new Func(new Complex(-1)));
                    One o1 = new One(new Func(3,new Many2(new Func(t))));
                    o1.addto(new Func(de));
                    Many _m = new Many(o0); _m.add(o1);

                    One o = new One(new Func(t), new Func(e));
                    o.addto(new Func(new Many2(_m)));
                    
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {//3:3
                    Many2 dt = ((Many2)(t.data)).diff_down(a);
                    Many2 de = ((Many2)(e.data)).diff_down(a);

                    One o0 = new One(new Func(dt)); o0.addto(new Func(e));
                    o0.addto(new Func(new Many2((Many2)(t.data))),new Func(new Complex(-1)));
                    o0.addto(new Func(t),new Func(new Complex(-1)));
                    One o1 = new One(new Func(3,new Many2(new Func(t))));
                    o1.addto(new Func(de));
                    o1.addto(new Func(new Many2((Many2)(e.data))),new Func(new Complex(-1)));
                    Many _m = new Many(o0); _m.add(o1);

                    One o = new One(new Func(t), new Func(e));
                    o.addto(new Func(new Many2(_m)));
                    
                    return new Func(new Many2(o));
                    
                },
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},

                (Func t, Func e, Vals a) => {return new Func(new Complex(0));}, //4:0
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},

                (Func t, Func e, Vals a) => {return new Func(new Complex(0));}, //5:0
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},

                (Func t, Func e, Vals a) => {return new Func(new Complex(0));}, //6:0
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},

                (Func t, Func e, Vals a) => {return new Func(new Complex(0));}, //7:0
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},

                (Func t, Func e, Vals a) => {return new Func(new Complex(0));}, //8:0
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));},
                (Func t, Func e, Vals a) => {return new Func(new Complex(0));}
               };


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func diff_down(Func exp, Vals d)
        {
            if ((type == Func.t_row) || (exp.type == Func.t_row)) IDS.root.sys.error("cant diff on innate row");
            Func r;
            if (type > Func.t_ln)
            {
                if (((Many2)(data)).hasval(d)) IDS.root.sys.error("cant diff on such");
                if (exp.type > Func.t_ln)
                {
                    if (((Many2)(exp.data)).hasval(d)) IDS.root.sys.error("cant diff on such");
                    return new Func(new Complex(0));
                }
                else
                {
                    r = Func.diffe_down_func[0 + exp.type](this, exp, d);
                }
            }
            else
            {
                if (exp.type > Func.t_ln)
                {
                    if (((Many2)(exp.data)).hasval(d)) IDS.root.sys.error("cant diff on such");
                    r = Func.diffe_down_func[type * Func.types + 0](this, exp, d);
                }
                else
                {
                    r = Func.diffe_down_func[type * Func.types + exp.type](this, exp, d);
                }
            }
            r.simple(); return r;
        }
    }

    public class MAO_dict
    {
        static short bmexp = 11;
        static int mexp = 1 << bmexp;
        public int nvals;
        public Num[] exps;
        public Func[] vals;
        public Many_as_one[] mao;
        public SortedDictionary<Func, int> to_val;
        ushort[] eneg, eadd; bool[] eflg_a;
        ushort lexp, lval;
        public MAO_dict(int v)
        {
            nvals = v;
            exps = new Num[mexp];
            vals = new Func[nvals];
            mao = new Many_as_one[nvals];
            to_val = new SortedDictionary<Func, int>();
            eneg = new ushort[mexp * mexp]; eadd = new ushort[mexp * mexp]; eflg_a = new bool[mexp * mexp];
            for (uint i = 0; i < mexp * mexp; i++) { eneg[i] = 0xFFFF; eadd[i] = 0xFFFF; eflg_a[i] = true; }
            lexp = 2; exps[0] = new Num(0); exps[1] = new Num(1);
            lval = 0;
        }
        public void save(BinaryWriter file)
        {
            file.Write((Int32)nvals);
            file.Write((UInt16)lexp);
            int m, i = 0; while (i < lexp)
            {
                exps[i].save(file);
                i++;
            }
            file.Write((UInt16)lval);
            m = 0; i = 0; while (i < lval)
            {
                vals[i].save(file);
                if (mao[i] != null) m++;
                i++;
            }
            file.Write((UInt16)m);
            i = 0; while (i < lval)
            {
                if (mao[i] != null)
                {
                    file.Write((UInt16)i);
                    mao[i].save(file);
                }
                i++;
            }
        }
        public static MAO_dict load(BinaryReader file)
        {
            MAO_dict ret = new MAO_dict(file.ReadInt32());
            ret.lexp = file.ReadUInt16();
            int m, i = 0; while (i < ret.lexp)
            {
                ret.exps[i] = Num.load(file);
                i++;
            }
            ret.lval = file.ReadUInt16();
            Func v;
            i = 0; while (i < ret.lval)
            {
                v = Func.load(file);
                ret.to_val[v] = i;
                ret.vals[i] = v;
                i++;
            }
            i = 0; m = file.ReadUInt16();
            while (i < m)
            {
                ret.mao[file.ReadUInt16()] = Many_as_one.load(file, ret);
                i++;
            }
            return ret;
        }
        public ushort exp(Num e)
        {
            ushort i;
            for (i = 0; i < lexp; i++) if (e.CompareTo(exps[i]) == 0) return i;
            if (lexp > mexp - 2) IDS.root.sys.error("too Many exp");
            exps[lexp++] = new Num(e);
            return i;
        }
        public int val(Func v)
        {
            if (!to_val.ContainsKey(v))
            {
                if (lval >= nvals) IDS.root.sys.error("MAO: too many funcs");
                to_val[v] = lval;
                vals[lval++] = v;
            }
            return to_val[v];
        }
        public void set_mao(int i)
        {
            if (i >= lval) IDS.root.sys.error("MAO: wrong expand");
            if (mao[i] == null)
            {
                if ((vals[i].type != 0) || (((Vals)(vals[i].data)).var.var == null)) IDS.root.sys.error("MAO: wrong expand");
                Func f = new Func(((Vals)(vals[i].data)).var.var); if (f.type != 2) IDS.root.sys.error("MAO: only many2");
                f.deeper(((Vals)(vals[i].data)).deep);
                mao[i] = new Many_as_one(f, this);
            }
        }
        public void expand(int to, int by)
        {
            set_mao(to); set_mao(by);
            mao[to].expand(mao[by], by);
        }
        public bool test_a(ushort e0, ushort e1)
        {
            uint ee = (uint)e0 + (((uint)e1) << bmexp);
            return eflg_a[ee];
        }
        public ushort add(ushort e0, ushort e1)
        {
            uint ee = (uint)e0 + (((uint)e1) << bmexp);
            if (eadd[ee] == 0xFFFF)
            {
                Num sum = new Num(exps[e0]);
                sum.add(exps[e1]);
                eadd[ee] = exp(sum);
                eflg_a[ee] = ((sum.up == 0) || (exps[e1].up == 0) || (sum.sign != exps[e1].sign));
            }
            return eadd[ee];
        }
        public ushort neg(ushort e)
        {
            if (eneg[e] == 0xFFFF)
            {
                Num neg = new Num(exps[e]);
                neg.neg();
                eneg[e] = exp(neg);
            }
            return eneg[e];
        }
    }
    public class MAO_key : IComparable
    {
        public MAO_dict dict;
        public ushort[] key;
        public MAO_key(MAO_dict d)
        {
            dict = d;
            key = new ushort[d.nvals];
            for (int i = 0; i < d.nvals; i++) key[i] = 0;
        }
        public MAO_key(MAO_dict d, ushort[] k)
        {
            dict = d;
            key = k;
        }
        public MAO_key(MAO_key k)
        {
            dict = k.dict;
            key = new ushort[dict.nvals];
            set(k);
        }
        public void save(BinaryWriter file)
        {
            int i = 0; while (i < dict.nvals)
            {
                file.Write((UInt16)(key[i]));
                i++;
            }
        }
        public static MAO_key load(BinaryReader file, MAO_dict d)
        {
            ushort[] k = new ushort[d.nvals];
            int i = 0; while (i < d.nvals)
            {
                k[i] = file.ReadUInt16();
                i++;
            }
            return new MAO_key(d, k);
        }
        public void set(MAO_key k)
        {
            for (int i = 0; i < dict.nvals; i++) key[i] = k.key[i];
        }
        public void neg()
        {
            for (int i = 0; i < dict.nvals; i++) key[i] = dict.neg(key[i]);
        }
        public bool test_m(MAO_key a)
        {
            bool ret = true; ushort tmp;
            for (int i = 0; i < dict.nvals; i++)
            {
                tmp = dict.add(key[i], a.key[i]);
                ret = ret && dict.test_a(key[i], a.key[i]);
            }
            return ret;
        }
        public bool mul(MAO_key a)
        {
            bool ret = true; ushort tmp;
            for (int i = 0; i < dict.nvals; i++)
            {
                tmp = dict.add(key[i], a.key[i]);
                ret = ret && dict.test_a(key[i], a.key[i]);
                key[i] = tmp;
            }
            return ret;
        }
        public bool mul(MAO_key a0, MAO_key a1)
        {
            bool ret = true;
            for (int i = 0; i < dict.nvals; i++)
            {
                key[i] = dict.add(a0.key[i], a1.key[i]);
                ret = ret && dict.test_a(a0.key[i], a1.key[i]);
            }
            return ret;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            MAO_key k = obj as MAO_key;
            for (int i = 0; i < dict.nvals; i++)
            {
                if (key[i] > k.key[i]) return -1;
                if (key[i] < k.key[i]) return 1;
            }
            return 0;
        }
    }
    public class Many_as_one
    {
        MAO_dict dict;
        public SortedDictionary<MAO_key, Complex>[] data;

        public Many_as_one(MAO_dict d)
        {
            dict = d;
            _data_i();
        }
        public void save(BinaryWriter file, SortedDictionary<MAO_key, Complex> m)
        {
            file.Write((Int32)m.Count);
            foreach (KeyValuePair<MAO_key, Complex> d in m)
            {
                d.Key.save(file);
                d.Value.save(file);
            }
        }
        public void save(BinaryWriter file)
        {
            save(file, data[0]);
            save(file, data[1]);
        }
        public static void load(BinaryReader file, SortedDictionary<MAO_key, Complex> m, MAO_dict d)
        {
            int i = 0, max = file.ReadInt32(); while (i < max)
            {
                m.Add(MAO_key.load(file, d), Complex.load(file));
                i++;
            }
        }
        public static Many_as_one load(BinaryReader file, MAO_dict d)
        {
            Many_as_one ret = new Many_as_one(d);
            Many_as_one.load(file, ret.data[0], d);
            Many_as_one.load(file, ret.data[1], d);
            return ret;
        }
        void _data_i()
        {
            data = new SortedDictionary<MAO_key, Complex>[2];
            data[0] = new SortedDictionary<MAO_key, Complex>();
            data[1] = new SortedDictionary<MAO_key, Complex>();
        }
        public KeyValuePair<MAO_key, Complex> fr_one(KeyValuePair<One, Complex> o)
        {
            int i0, v0;
            KeyValuePair<MAO_key, Complex> ret = new KeyValuePair<MAO_key, Complex>(new MAO_key(dict), new Complex(o.Value));
            for (i0 = 0; i0 < dict.nvals; i0++) ret.Key.key[i0] = 0;
            foreach (KeyValuePair<Func, Func> f in o.Key.exps)
            {
                if (f.Value.type_pow() > 1) IDS.root.sys.error("cant fast on complex exp");
                v0 = dict.val(f.Key);
                ret.Key.key[v0] = dict.exp(((Complex)(f.Value.data)).r);
            }
            return ret;
        }
        public One to_one(MAO_key fr)
        {
            int i;
            One ret = new One();
            for (i = 0; i < dict.nvals; i++)
                if (fr.key[i] != 0) ret.exps.Add(new Func(dict.vals[i]), new Func(new Complex(dict.exps[fr.key[i]])));
            return ret;
        }

        public void add(int ud, MAO_key m, Complex n)
        {
            if (data[ud].ContainsKey(m)) data[ud][m].add(n); else data[ud].Add(new MAO_key(m), new Complex(n));
        }
        public void add(int ud, KeyValuePair<MAO_key, Complex> a)
        {
            if (data[ud].ContainsKey(a.Key)) data[ud][a.Key].add(a.Value); else data[ud].Add(new MAO_key(a.Key), new Complex(a.Value));
        }
        public void add(int ud, SortedDictionary<MAO_key, Complex> fr)
        {
            KeyValuePair<MAO_key, Complex> tmp;
            foreach (KeyValuePair<MAO_key, Complex> d in fr) { tmp = d; add(ud, tmp); }
        }
        public void mul(int ud, MAO_key m, Complex n)
        {
            foreach (KeyValuePair<MAO_key, Complex> d in data[ud])
            {
                d.Key.mul(m);
                d.Value.mul(n);
            }
        }
        public void muladd(int ud, SortedDictionary<MAO_key, Complex> fr, MAO_key m, Complex n)
        {
            KeyValuePair<MAO_key, Complex> tmp = new KeyValuePair<MAO_key, Complex>(new MAO_key(dict), new Complex(0));
            foreach (KeyValuePair<MAO_key, Complex> d in fr)
            {
                tmp.Key.mul(m, d.Key);
                tmp.Value.set(n);
                tmp.Value.mul(d.Value);
                add(ud, tmp);
            }
        }

        public void mul(int ud, SortedDictionary<MAO_key, Complex> m0)
        {
            SortedDictionary<MAO_key, Complex> tmp1 = data[ud];
            data[ud] = new SortedDictionary<MAO_key, Complex>();
            foreach (KeyValuePair<MAO_key, Complex> d in m0) muladd(ud, tmp1, d.Key, d.Value);
        }
        public void mul(int ud, SortedDictionary<MAO_key, Complex> m0, SortedDictionary<MAO_key, Complex> m1)
        {
            data[ud].Clear();
            foreach (KeyValuePair<MAO_key, Complex> d in m0) muladd(ud, m1, d.Key, d.Value);
        }
        void set(Many_as_one fr)
        {
            for (int i = 0; i < 2; i++)
            {
                data[i].Clear();
                foreach (KeyValuePair<MAO_key, Complex> d in fr.data[i]) data[i].Add(new MAO_key(d.Key), new Complex(d.Value));
            }
        }
        public Many_as_one(Func f, MAO_dict d)
        {
            dict = d;
            _data_i();
            foreach (KeyValuePair<One, Complex> o in ((Many2)f.data).up.data) add(0, fr_one(o));
            foreach (KeyValuePair<One, Complex> o in ((Many2)f.data).down.data) add(1, fr_one(o));
        }
        public Func to_func()
        {
            int i = 0, cn = data[0].Count + data[1].Count;
            Many _u = new Many(); Many _d = new Many();
            foreach (KeyValuePair<MAO_key, Complex> d in data[0]) { _u.data.Add(to_one(d.Key), new Complex(d.Value)); IDS.root.sys.progr(i++, cn); }
            foreach (KeyValuePair<MAO_key, Complex> d in data[1]) { _d.data.Add(to_one(d.Key), new Complex(d.Value)); IDS.root.sys.progr(i++, cn); }
            return new Func(new Many2(_u, _d));
        }

        public Many_as_one(Many_as_one _m, int _e)
        {
            dict = _m.dict;
            Many_as_one tmp = new Many_as_one(dict);
            Many_as_one _tmp = new Many_as_one(dict);
            Many_as_one fr = new Many_as_one(dict);
            Num exp = dict.exps[_e], nexp = new Num(0);
            int i0, _eu = (int)(exp.up);
            fr.set(_m);
            if (exp.down > 1)
            {
                if ((fr.data[0].Count > 1) || (fr.data[1].Count > 1)) return;
                if (!fr.data[0].ToArray()[0].Value.isint(1)) return;
                if (!fr.data[1].ToArray()[0].Value.isint(1)) return;
                for (i0 = 0; i0 < dict.nvals; i0++)
                {
                    nexp.set(dict.exps[fr.data[0].ToArray()[0].Key.key[i0]]);
                    nexp.mul((BigInteger)1, exp.down);
                    fr.data[0].ToArray()[0].Key.key[i0] = dict.exp(nexp);
                }
            }
            _data_i();
            data[0].Add(new MAO_key(dict), new Complex(1));
            data[1].Add(new MAO_key(dict), new Complex(1));

            tmp.set(fr);
            for (int i = _eu; i > 0; i >>= 1)
            {
                if ((i & 1) != 0)
                {
                    mul(0, tmp.data[0]);
                    mul(1, tmp.data[1]);
                }
                if (i > 1)
                {
                    _tmp.set(tmp);
                    tmp.mul(0, _tmp.data[0], _tmp.data[0]);
                    tmp.mul(1, _tmp.data[1], _tmp.data[1]);
                }
            }

            if (exp.sign < 0) { tmp.data[0] = data[0]; data[0] = data[1]; data[1] = tmp.data[0]; }
        }
        bool expand(int n, Many_as_one e, int ex)
        {
            bool ret = false;
            int ee; ushort tex;
            Num max_u = new Num(0), max_d = new Num(0), now_u = new Num(0), now_d = new Num(0);
            Many_as_one[] me = new Many_as_one[254], ae = new Many_as_one[254];
            MAO_key z = new MAO_key(dict);
            KeyValuePair<MAO_key, Complex> tu = new KeyValuePair<MAO_key, Complex>(new MAO_key(dict), new Complex(0));
            me[0] = new Many_as_one(e, 0);
            me[1] = new Many_as_one(e, 1);
            if ((e.data[0].Count < 1) || (e.data[1].Count < 1)) IDS.root.sys.error("wrong");
            int pnow = 0;
            foreach (KeyValuePair<MAO_key, Complex> u in data[n])
            {
                tex = u.Key.key[ex];
                tu.Key.set(u.Key);
                tu.Value.set(u.Value);
                if (me[tex] == null)
                {
                    me[tex] = new Many_as_one(e, tex);
                    if (me[tex].data == null) me[tex] = null;
                    else
                    {
                        if (max_u.great(dict.exps[tex])) max_u.set(dict.exps[tex]);
                        if (!max_d.great(dict.exps[tex])) max_d.set(dict.exps[tex]);
                    }
                }
                if (me[tex] != null) tu.Key.key[ex] = 0; else tex = 0;
                if (ae[tex] == null) ae[tex] = new Many_as_one(dict);
                ae[tex].add(0, tu);
                IDS.root.sys.progr(pnow++, data[n].Count);
            }
            max_d.neg();
            for (tex = 0; tex < 254; tex++)
            {
                if (ae[tex] != null)
                {
                    if (tex > 0) ret = true;
                    now_u.set(max_u); now_d.set(max_d);
                    if (dict.exps[tex].down == 1)
                    {
                        if (dict.exps[tex].sign > 0) now_u.add_up(0 - dict.exps[tex].up);
                        else now_d.add_up(0 - dict.exps[tex].up);
                    }
                    else
                    {
                        for (int tex0 = 0; tex0 < 254; tex0++) if ((ae[tex0] != null) && (tex0 != tex)) ae[tex0].mul(0, me[tex].data[1]);
                        mul(1 - n, me[tex].data[1]);
                    }
                    ee = dict.exp(now_u);
                    if (me[ee] == null) me[ee] = new Many_as_one(e, ee);
                    ae[tex].mul(0, me[ee].data[1]);
                    ee = dict.exp(now_d);
                    if (me[ee] == null) me[ee] = new Many_as_one(e, ee);
                    ae[tex].mul(0, me[ee].data[0]);
                }
                IDS.root.sys.progr(tex, 254);
            }
            ee = dict.exp(max_u);
            if (me[ee] == null) me[ee] = new Many_as_one(e, ee);
            mul(1 - n, me[ee].data[1]);
            ee = dict.exp(max_d);
            if (me[ee] == null) me[ee] = new Many_as_one(e, ee);
            mul(1 - n, me[ee].data[0]);
            data[n].Clear();
            for (tex = 0; tex < 254; tex++)
            {
                if (ae[tex] != null)
                {
                    me[tex].mul(0, ae[tex].data[0]);
                    add(n, me[tex].data[0]);
                }
                IDS.root.sys.progr(tex, 254);
            }
            return ret;
        }
        public bool expand(Many_as_one e, int id)
        {
            bool r0 = expand(0, e, id);
            bool r1 = expand(1, e, id);
            foreach (KeyValuePair<MAO_key, Complex> d in data[0]) d.Value.simple();
            foreach (KeyValuePair<MAO_key, Complex> d in data[1]) d.Value.simple();
            return r0 | r1;
        }
    }


    public class Fileio//: IDisposable
    {
        StreamReader fin, f611 = null;
        public StreamWriter file_flag;
        public string flag;
        List<string>[] sout;
        public string name;
        public Boolean has;
        public Fileio(string nin)
        {
            string fname;
            if (!File.Exists(nin+".sh0")) Environment.Exit(-1);
            name = nin;
            fname = name+".flg"; flag = "";
            if (File.Exists(fname))
            {
                StreamReader _fl = new StreamReader(fname);
                flag = _fl.ReadLine(); if (flag == null) flag = ""; _fl.Close();
                if ((flag.Length > 0) && (flag[0] == '#')) flag = "";
            }
            file_flag = new StreamWriter(fname);
            //            f611 = (File.Exists("611"+iexf) ?  new StreamReader("611"+iexf) : null);
            fin = new StreamReader(nin+".sh0");
            sout = new List<string>[40];
            has = true;
        }
        private string rfile()
        {
            string r;
            if ((f611 != null) && ((r = f611.ReadLine()) != null)) return r;
            return fin.ReadLine();
        }

        public void progr(int now, int all)
        {
            /*
                        if (Program.m0.IsDisposed) Environment.Exit(-1);
                        if (all < 6) return;
                        if (now > all) now = all;
                        int pr_now = now*(Program.m0.sx-1)/all, l_now = ncline*(Program.m0.sx-1)/clines;
                        if ((pr_now == Program.m0.pr_now) && (l_now == Program.m0.l_now)) return;
                        Program.m0.pr_now = pr_now; Program.m0.l_now = l_now; 
                        Program.m0.Set(0);
             */
        }

        public string rline()
        {
            string r;
            has = true;
            r = rfile();
            if (r == null) { has = false; r = ""; }
            return r;
        }
        public void finish(bool thr)
        {
            StreamWriter fout;
            fin.Close();
            int i = 0; while (i < sout.Length)
            {
                if (sout[i] != null)
                {
                    fout = new StreamWriter(name + Parse.m_n_to_c[i] + ".txt", true);
                    foreach (string _s in sout[i]) fout.WriteLine(_s);
                    fout.Close();
                }
                i++;
            }
            if (IDS.root.pic != null) IDS.root.pic.Save(name + ".png");
            file_flag.BaseStream.Seek(0, SeekOrigin.Begin);
            file_flag.WriteLine(flag);
            file_flag.Close();
            if (flag[0] == '#')
            {
                File.Delete(name + ".bin");
            }
            if (thr) throw new FinishException();
        }
        public void finish() { finish(true); }
        public void error(string e)
        {
            wline(0, "");
            wline(0, IDS.root.par.prev);
            wline(0, IDS.root.par.val);
            wline(0, "Position " + IDS.root.par.pos.ToString() + ": " + ((IDS.now_func != null) ? IDS.root.par.print(IDS.now_func, true) + " " : "") + e);
            flag = "#error:" + e;
            File.Delete(name + ".bin");
            finish();
        }
        public void wline(int n, string s)
        {
            if (sout[n] == null) sout[n] = new List<string>();
            sout[n].Add(s);
        }
        public void wstr(int n, string s)
        {
            if (sout[n] == null) { sout[n] = new List<string>(); sout[n].Add(""); }
            if (s == "\\n") sout[n].Add(""); else sout[n][sout[n].Count - 1] += s;
        }
    }
    public class Deep
    {
        public char pair, oper;
        public int pos;
        public Deep(char p, char o)
        {
            pair = p; oper = o;
        }
    }
    public class Mbody
    {
        public int nparm;
        public string body;
        public Mbody(int n, string s)
        { nparm = n; body = s; }
    }
    public class Parse//: IDisposable
    {
        static public char[] m_n_to_c = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        static public int[] m_c_to_n = {
        -1,-1,-1,-1, -1,-1,-1,-1,  -1,-1,-1,-1, -1,-1,-1,-1,
        -1,-1,-1,-1, -1,-1,-1,-1,  -1,-1,-1,-1, -1,-1,-1,-1,

        -1,-1,-1,-1, -1,-1,-1,-1,  -1,-1,-1,-1, -1,-1,-1,-1,
         0, 1, 2, 3,  4, 5, 6, 7,   8, 9,-1,-1, -1,-1,-1,-1,

        -1,10,11,12, 13,14,15,16,  17,18,19,20, 21,22,23,24,
        25,26,27,28, 29,30,31,32,  33,34,35,-1, -1,-1,-1,-1,

        -1,10,11,12, 13,14,15,16,  17,18,19,20, 21,22,23,24,
        25,26,27,28, 29,30,31,32,  33,34,35,-1, -1,-1,-1,-1};
        static int[] m_c_type = {//0- num, 1- abc, 2- oper+-*/^, 3- symb,  4- ([{, 5- )]}, 6 - sys #"`
        -1,-1,-1,-1, -1,-1,-1,-1,  -1,-1,-1,-1, -1,-1,-1,-1,
        -1,-1,-1,-1, -1,-1,-1,-1,  -1,-1,-1,-1, -1,-1,-1,-1,

        -1, 3, 6, 6,  3, 3, 3, 1,   4, 5, 2, 2,  3, 2, 0, 2,
//          !  "  #   $  % 	&  ' 	(  )  *  +   ,  -  .  /
         0, 0, 0, 0,  0, 0, 0, 0,   0, 0, 3, 3,  3, 3, 3, 3,
//       0  1  2  3   4  5  6  7    8  9  :  ;   <  =  >  ?

         3, 1, 1, 1,  1, 1, 1, 1,   1, 1, 1, 1,  1, 1, 1, 1,
//        @
         1, 1, 1, 1,  1, 1, 1, 1,   1, 1, 1, 4,  3, 5, 2, 1,
//                                           [   \  ]  ^  _
         6, 1, 1, 1,  1, 1, 1, 1,   1, 1, 1, 1,  1, 1, 1, 1,
//       `
         1, 1, 1, 1,  1, 1, 1, 1,   1, 1, 1, 4,  3, 5, 3,-1};
        //                                           {   |  }  ~ 	
        public static int[] m_c_type_fr = { 0, 0, 2, 4, 0, 1, 2, 3, 4, 5, 6 };
        public static int[] m_c_type_sz = { 7, 2, 5, 2, 1, 1, 1, 1, 1, 1, 1 };
        public static char isall = (char)0, isname = (char)1, issymb = (char)2, ispair = (char)3,
                           isnum = (char)4, isabc = (char)5,
                           isoper = (char)6, isother = (char)7, isopen = (char)8, isclose = (char)9, issys = (char)10,
                           isend = ';';
        static int[] m_c_prior = {
         0, 0, 0, 0,  0, 0, 0, 0,   0, 0, 0, 0,  0, 0, 0, 0,
         0, 0, 0, 0,  0, 0, 0, 0,   0, 0, 0, 0,  0, 0, 0, 0,

         0, 0, 0, 0,  0, 0, 0, 0,   0, 0, 2, 1,  0, 1, 0, 2,
//          !  "  #   $  % 	& ' 	(  )  *  +   ,  -  .  /
         0, 0, 0, 0,  0, 0, 0, 0,   0, 0, 0, 0,  0, 0, 0, 0,
//       0  1  2  3   4  5  6  7    8  9  :  ;   <  =  >  ?

         0, 0, 0, 0,  0, 0, 0, 0,   0, 0, 0, 0,  0, 0, 0, 0,
//        @
         0, 0, 0, 0,  0, 0, 0, 0,   0, 0, 0, 0,  0, 0, 3, 0,
//                                           [   \  ]  ^  _
         0, 0, 0, 0,  0, 0, 0, 0,   0, 0, 0, 0,  0, 0, 0, 0,
//       `
         0, 0, 0, 0,  0, 0, 0, 0,   0, 0, 0, 0,  0, 0, 0, 0};
        //                                           {   |  }  ~ 	
        public bool flag_out_exptomul, flag_out_desc;
        public string prev, val, name;
        public int pos, opers, body_num;
        public char now, oper, main_oper;
        SortedDictionary<string, Mbody> macro;
        List<Deep> deep;
        List<List<Deep>> stack;
        public List<string> body;
        public Fileio sys;
        public Parse(Fileio s)
        {
            sys = s;
            pos = 0; opers = 0;
            macro = new SortedDictionary<string, Mbody>();
            deep = new List<Deep>();
            stack = new List<List<Deep>>();
            body = new List<string>();
            val = ""; prev = "";
            flag_out_exptomul = false;
            flag_out_desc = false;
        }
        public int find2(string s0, string s1)
        {
            int i0 = val.IndexOf(s0, pos);
            int i1 = val.IndexOf(s1, pos);
            if (i0 < 0) i0 = val.Length - 1;
            if (i1 < 0) i1 = val.Length - 1;
            return Math.Min(i0, i1);
        }
        public void init()
        {
            macro.Add("#sx(", new Mbody(0, IDS.root.pic.Height.ToString()));
            macro.Add("#sy(", new Mbody(0, IDS.root.pic.Width.ToString()));
            string _now = "";
            bool inside = false;
            while (sys.has) if (lnext() && (val.Length > 0))
                {
                    pos = 0; while (pos < val.Length)
                    {
                        if (inside)
                        {
                            pos = find2("\"", "\\\"");
                            if (val[pos] == '"') inside = false; else pos++; pos++;
                        }
                        else
                        {
                            pos = find2("\"", ";");
                            if (val[pos] == '"') inside = true;
                            if (val[pos] == ';')
                            {
                                _now += val.Substring(0, pos); val = val.Substring(pos + 1);
                                while ((_now.Length > 0) && (_now[0] < '!')) _now = _now.Substring(1);
                                if (_now.IndexOf('=') < 0) opers++;
                                body.Add(_now); _now = ""; pos = 0;
                            }
                            pos++;
                        }
                    }
                    _now += val;
                }
            body_num = 0;
            IDS.root.steps = opers;
        }
        string _parm()
        {
            string s1;
            if (isequnow('{')) s1 = calc().r.get_sup().ToString();
            else
            {
                s1 = get(",<>");
                s1 = s1.Replace("&0", "#");
                s1 = s1.Replace("&1", "&0");
                s1 = s1.Replace("&2", "&1");
                s1 = s1.Replace("&3", "&2");
                s1 = s1.Replace("&4", "&3");
            }
            return s1;
        }
        public void push()
        {
            deep[deep.Count - 1].pos = pos;
            stack.Add(new List<Deep>(deep));
        }
        public void pop()
        {
            if (stack.Count == 0) sys.error("parse: pop no push");
            deep = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);
            pos = deep[deep.Count - 1].pos;
            now = (more() ? val[pos] : isend);
        }
        public bool bnext()
        {
            body_num++; deep.Clear(); deep.Add(new Deep(isend, isend));
            if (body_num >= body.Count) return false;
            prev = val; val = body[body_num]; pos = 0; now = val[0];
            if (isequnow(Parse.isabc)) name = get(Parse.isname); else name = "";
            main_oper = now;
            return true;
        }
        public bool lnext()
        {
            string name, s0, s1, st, sf;
            int _np, i0, i1, i2;
            bool l_add = true;
            deep.Clear(); deep.Add(new Deep(isend, isend));
            val = sys.rline(); pos = 0;
            if ((val.Length == 0) || (val[0] == '`')) return false;
            if (val.Substring(0, 2) == "##")
            {
                pos = 2; name = "#" + get(isname) + "("; if (!isequnow('#')) sys.error("macro: wrong num");
                next(); if (!isequnow(isname)) sys.error("macro: wrong num");
                _np = m_c_to_n[now];
                string _m = val.Substring(pos + 2);
                for (i0 = 0; i0 < m_n_to_c.Length; i0++)
                {
                    if ((_m.IndexOf("#" + m_n_to_c[i0]) > -1) && (i0 >= _np)) sys.error("macro: used nonparm");
                }
                if (macro.ContainsKey(name))
                {
                    if (_np != macro[name].nparm) sys.error("macro: wrong num");
                    macro[name].body += "\n" + _m;
                }
                else macro.Add(name, new Mbody(_np, _m));
                return false;
            }
            while ((pos = val.LastIndexOf("#")) > -1)
            {
                sf = val.Substring(0, pos); now = val[++pos]; name = "#" + get(isname) + "(";
                if (!macro.ContainsKey(name)) sys.error("macro: not found");
                s0 = macro[name].body.Replace("`\n", "");
                int ploop = -1, floop = 0, tloop = 0;
                i2 = 0; i1 = deep.Count;
                while (i2 < macro[name].nparm)
                {
                    s1 = _parm();
                    if (isequnow('<') || isequnow('>'))
                    {
                        l_add = isequnow('<'); next();
                        if (ploop > -1) sys.error("macro: wrong loop");
                        if (!int.TryParse(s1, out floop)) sys.error("macro: wrong loop");
                        if (!int.TryParse(_parm(), out tloop)) sys.error("macro: wrong loop");
                        ploop = i2;
                    }
                    else s0 = s0.Replace("#" + m_n_to_c[i2], s1);
                    if (i1 != deep.Count) sys.error("macro: call nparm");
                    next(); i2++;
                }
                if (i2 == 0) next(); if (!isequnow(')')) sys.error("macro:"); next();
                st = (pos < val.Length ? val.Substring(pos, val.Length - pos) : "");
                if (ploop < 0) val = sf + s0 + st;
                else
                {
                    if (l_add) for (s1 = "", i2 = floop; i2 <= tloop; i2++) s1 += s0.Replace("#" + m_n_to_c[ploop], i2.ToString().Trim());
                    else for (s1 = "", i2 = floop; i2 >= tloop; i2--) s1 += s0.Replace("#" + m_n_to_c[ploop], i2.ToString().Trim());
                    val = sf + s1 + st;
                }
            }
            pos = 0; now = val[0]; return true;
        }
        public bool more() { return pos < val.Length; }
        public bool isequ(char t, char tst)
        {
            if (tst < m_c_type_fr.Length)
            {
                int _t = m_c_type[t] - m_c_type_fr[tst];
                return _t >= 0 && _t < m_c_type_sz[tst];
            }
            else return t == tst;
        }
        public bool isequnow(char tst) { return isequ(now, tst); }
        public bool isequnow(string tst)
        {
            int i = 0; while (i < tst.Length)
            {
                if (now != tst[i]) return false;
                next(); i++;
            }
            return true;
        }
        void setnow()
        {
            if (more()) now = val[pos];
            else
            {
                now = isend;
                if (deep.Count != 1) sys.error("parse: nonpair");
            }
        }
        public void next()
        {
            if (!more()) return;
            int _tp = m_c_type[now];
            if (_tp == 2) oper = now;
            if (_tp == 4) deep.Add(new Deep(now, oper));
            if (_tp == 5)
            {
                if ((deep.Count < 1) || (deep.Last().pair > now) || (now - deep.Last().pair > 2)) sys.error("parse: nonpair");
                deep.RemoveAt(deep.Count - 1);
            }
            do { pos++; } while (more() && (val[pos] == ' ')); setnow();
        }
        public string get(char tst)
        {
            string ret = ""; int nowdeep = deep.Count;
            while (true)
            {
                if (isequnow(isend)) return ret;
                if ((nowdeep == deep.Count) && (isequ(now, isclose) || (!isequ(now, tst)))) return ret;
                ret += now.ToString(); next();
            }
        }
        public int get_int()
        {
            int r = 0;
            if (!(isequnow(isnum) && Int32.TryParse(get(isnum), out r))) IDS.root.sys.error("not int");
            return r;
        }
        public string get(string delim)
        {
            string ret = "";
            if ((delim.Length == 1) && (delim.IndexOf(val[pos]) > -1)) pos++;
            while (true)
            {
                if ((!more()) || (delim.IndexOf(val[pos]) > -1))
                {
                    if (delim.Length == 1) pos++;
                    setnow(); return ret;
                }
                ret += val[pos].ToString(); pos++;
            }
        }
        public void branch(char t, char[] i, Action[] f)
        {
            bool r = false;
            for (int i0 = 0; i0 < i.Count(); i0++)
            {
                if (isequ(t, i[i0]))
                {
                    r = true; f[i0]();
                }
            }
            if (!r) f[i.Count()]();
        }
        public void branchnow(char[] i, Action[] f)
        {
            branch(now, i, f);
        }
        public void branch(char t, string i, Action[] f)
        {
            int p = i.IndexOf(t);
            if (p > -1) f[p](); else f[i.Length]();
        }
        public void branchnow(string i, Action[] f)
        {
            branch(now, i, f);
        }
        Func infunc()
        {
            Func r;
            string _n = get(isname);
            if (isequnow('('))
            {
                if (IDS.root.fnames.ContainsKey(_n)) r = fpars(_n, true);
                else
                {
                    Vars vr = IDS.root.find_var(_n);
                    if ((vr.var == null) || (vr.var.type != 7)) IDS.root.sys.error("not row");
                    next(); if (!isequnow(isabc)) IDS.root.sys.error("wrong row call");
                    Vals vl = IDS.root.find_val(get(isname));
                    if (!isequnow(',')) IDS.root.sys.error("wrong row call");
                    next(); if (!isequnow(isnum)) IDS.root.sys.error("wrong row call");
                    int st; Int32.TryParse(get(isnum), out st);
                    r = new Func(((Row)(vr.var.data)).prep_calc(vl, st));
                }
            }
            else r = new Func(IDS.root.find_val(_n));
            return r;
        }
        public KeyValuePair<One, Complex> opars()
        {
            bool repeat;
            Func eval = null;
            Func ex = new Func(new Complex(1));
            One ro = new One(); Complex rn = new Complex(now == '-' ? -1 : +1);
            if ((now == '-') || (now == '+')) next();
            bool l = true;
            Action eset = () =>
            {
                if (eval != null)
                {
                    if ((eval.type == Func.t_num) && (ex.type_pow() == 0))
                    {
                        Complex nval = new Complex((Complex)(eval.data));
                        nval.exp((Complex)(ex.data)); rn.mul(nval);
                    }
                    else
                    {
                        if (ro.exps.ContainsKey(eval)) ro.exps[eval].add(ex); else ro.exps.Add(eval, ex);
                    }
                    eval = null; ex = new Func(new Complex(1));
                }
            };
            char[] pc = { isabc, isnum, '{', '(', '[' };
            Action[] pf = {
                      () => { 
                          ex.mul(new Func(new One(infunc()),new Complex(1)));
                      },
                      () => ex.mul(new Complex(get(isnum))),
                      () => ex.mul(calc()),
                      () => ex.mul(fpars("",true)),
                      () => ex.mul(fpars("",true)),
                      () => sys.error("noNum in calc")
                          };
            const string ooc = "+-*/^";
            Action[] oof = {
                () => {l = false; }, //+
                () => {l = false; }, //-
                () => {eset(); next();}, //*
                () => {eset(); next(); ex.neg();}, // /
                () => { //^
                    next();
                    if (eval == null) sys.error("parse");
                    branchnow(pc,pf); 
                    eset();
                    repeat = true;
                },
                () => sys.error("noNum in calc")
                };
            char[] nc = { isabc, isnum, '{', '(', '[' };
            Action[] nf = {
                () => {eval = infunc();},
                () => {eval = new Func(new Complex(get(isnum)));},
                () => {eval = new Func(calc());},
                () => {eval = fpars("",true);},
                () => {eval = fpars("",true);},
                () => sys.error("noNum in calc")
                };
            char[] oc = { isoper, isclose, isend, ',' };
            Action[] of = {
                () => branchnow(ooc,oof),
                () => {l = false; },
                () => {l = false; },
                () => {l = false; },
                () => sys.error("noNum in calc")
                };
            while (l)
            {
                branchnow(nc, nf);
                do
                {
                    repeat = false;
                    branchnow(oc, of);
                } while (repeat);
            }
            eset();
            rn.mul(ro.simple());
            rn.simple(); return new KeyValuePair<One, Complex>(ro, rn);
        }
        public Many mpars()
        {
            Many m = new Many();
            KeyValuePair<One, Complex> on;
            int d = deep.Count;
            while ((!isequnow(isclose)) && (!isequnow(isend)) && (!isequnow(',')))
            {
                on = opars(); m.add(on.Key, on.Value);
            }
            if (d < deep.Count) next();
            return m;
        }
        public Many2 m2pars()
        {
            return new Many2(mpars(), new Many(new Complex(1)));
        }
        public Func fpars(string _fn, bool _pair)
        {
            int tp, d = deep.Count;
            Func r = null;
            if (!IDS.root.fnames.ContainsKey(_fn)) sys.error("parse: func");
            tp = IDS.root.fnames[_fn];
            if (_pair) { if (isequnow(ispair)) { if (isequnow('[')) tp = 8; next(); } else sys.error("parse: func"); }
            switch (tp)
            {
                case 7:
                    r = new Func(new Row());
                    if (!isequnow(isnum)) sys.error("parse:row");
                    ((Row)(r.data)).point = get_int();
                    if (!isequnow(',')) sys.error("parse:row");
                    next();
                    int i = 0; while (true)
                    {
                        if (!isequnow('(')) sys.error("parse:row");
                        next(); ((Row)(r.data)).data.Add(i, m2pars()); next();
                        if (!isequnow(',')) break;
                        next(); i++;
                    }
                    break;
                case 8:
                    List<List<Func>> tm = new List<List<Func>>();
                    while (!isequnow(']'))
                    {
                        if (!isequnow('[')) sys.error("parse: matr");
                        next(); tm.Add(new List<Func>());
                        while (!isequnow(']'))
                        {
                            tm.Last().Add(new Func(Func.t_many2, m2pars()));
                            if (isequnow(',')) next();
                        }
                        next();
                        if (isequnow(',')) next();
                        if (tm.Last().Count != tm[0].Count) sys.error("parse: matr size");
                    }
                    r = new Func(new Matrix(tm));
                    break;
                default:
                    r = new Func(tp, m2pars());
                    break;
            }
            if (d < deep.Count) next();
            r.simple(); return r;
        }
        public Complex calc()
        {
            char[] lo = { '+', ' ', ' ', ' ' };
            Complex[] ln = { new Complex(0), new Complex(0), new Complex(0), new Complex(0) };
            int lp = 0;
            if (isequnow(isnum))
            {
                Complex r = new Complex(get(isnum));
                if (isequnow(',')) next();
                return r;
            }
            if (isequnow(ispair)) next();
            bool isi = false, l = true, li;
            Action[] fnr = {
                      () => ln[lp-1].add(ln[lp]),() => ln[lp-1].sub(ln[lp]),
                      () => ln[lp-1].mul(ln[lp]),() => ln[lp-1].div(ln[lp]),
                      () => ln[lp-1].exp(ln[lp]),
                      () => sys.error("noNum in calc")
                     };
            Action[] fni = {
                      () => ln[lp-1].i = Num.add(ln[lp-1].i,ln[lp].i),() => ln[lp-1].i = Num.sub(ln[lp-1].i,ln[lp].i),
                      () => ln[lp-1].i = Num.mul(ln[lp-1].i,ln[lp].i),() => ln[lp-1].i = Num.div(ln[lp-1].i,ln[lp].i),
                      () => ln[lp-1].i = Num.exp(ln[lp-1].i,ln[lp].i),
                      () => sys.error("noNum in calc")
                     };
            char[] nc = { isopen, isnum };
            Action[] nf = {
                              () => ln[++lp] = calc(),
                              () => ln[++lp] = (isi ? new Complex(0,get(isnum)) : new Complex(get(isnum))),
                              () => sys.error("in calc")
                          };
            char[] oc = { ':', isoper, isclose, isend };
            Action[] of = {
                              () => {li = false; next();},
                              () => {lo[lp] = now; next();},
                              () => {l = false; next();},
                              () => {l = false;},
                              () => sys.error("in calc")
                          };
            while (l)
            {
                if (ln[0].i.sign != 0) sys.error("wrong complex in calc");
                lo[0] = '+';
                if ((now == '+') || (now == '-')) { lo[0] = now; next(); }
                li = true;
                while (li && l)
                {
                    branchnow(nc, nf);
                    branchnow(oc, of);
                    while ((lp > 0) && (m_c_prior[lo[lp - 1]] >= m_c_prior[lo[lp]]))
                    {
                        branch(lo[lp - 1], "+-*/^", (isi ? fni : fnr)); lo[lp - 1] = lo[lp]; lp--;
                    }
                }
                isi = !isi;
            }
            ln[0].simple(); return ln[0];
        }

        public string print_mul(Complex p, ref bool first, bool noone)
        {
            string ret = "";
            if (p.i.sign != 0)
            {
                ret = (first ? "" : "+") + "{" + print(p.r, true) + ":" + print(p.i, true) + "}";
                first = false;
            }
            else
            {
                ret = (p.r.sign < 0 ? "-" : (first ? "" : "+"));
                if (noone || ((p.r.down > 1) || (p.r.up != 1))) { ret += print(p.r, false); first = false; } else first = true;
            }
            return ret;
        }
        public string print(Complex p, bool nopow, bool nosign)
        {
            string ret = "";
            if (p.i.sign != 0)
            {
                ret = "{" + print(p.r, true) + ":" + print(p.i, true) + "}";
            }
            else
            {
                if (((p.r.sign > -1) || nosign) && (p.r.isint() || nopow)) ret += print(p.r, !nosign);
                else ret = "{" + print(p.r, nopow) + "}";
            }
            return ret;
        }

        public string print(Num v, bool sign)
        {
            string ret = "";
            if (sign)
            {
                if (v.sign < 0) ret = "-";
                if (v.sign > 0) ret = "+";
            }
            ret += v.up.ToString().Trim();
            if (!v.isint()) { ret += "/" + v.down.ToString().Trim(); }
            return ret;
        }
        public string print(Complex v, int b, int a)
        {
            string s = print(v.r, b, a);
            if (v.i.sign != 0) s += ":" + print(v.i, b, a);
            return s;
        }
        public string print(Complex v, int b)
        {
            return print(v, b, IDS.prec10);
        }
        public string print(Num v, int b, int a)
        {
            string s; if (a < 0) a = IDS.prec10;
            Num _v = new Num(v); _v.sign = 1;
            BigInteger nn = _v.toint();
            s = (v.sign < 0 ? "-" : (v.sign > 0 ? "+" : " ")) + nn.ToString().Trim() + ".";
            s = (b > 0 ? s.PadLeft(b) : s);
            if (a > 0)
            {
                Num tn = Num.sub(_v, new Num(nn)); tn.add(1); tn.up *= IDS.e10[a];
                s += tn.toint().ToString().Trim().Substring(1);
            }
            return s;
        }
        public string print(One o, Complex n, bool first)
        {
            string ret = print_mul(n, ref first, o.exps.Count == 0);
            bool div, one;
            Complex tmp;
            foreach (KeyValuePair<Func, Func> m in o.exps)
            {
                if ((m.Value.type != 1) || (!((Complex)(m.Value.data)).iszero()))
                {
                    if (m.Value.type == 1)
                    {
                        tmp = (Complex)(m.Value.data);
                        div = ((tmp.i.sign == 0) && (tmp.r.sign < 0));
                        one = (tmp.isint(1) || tmp.isint(-1));
                        if (flag_out_exptomul && (tmp.i.sign == 0) && (tmp.r.isint()) && (tmp.r.up < 11)) {
                            if (first) ret += (div ? "1/" : ""); else ret += (div ? "/" : "*");
                            int i = 1; while(i < tmp.r.up) {
                                ret += print(m.Key, true) + (div ? "/" : "*");
                                i++;
                            }
                            ret += print(m.Key, true);
                        } else {
                            if (first) ret += (div ? "1/" : ""); else ret += (div ? "/" : "*");
                            if (one) ret += print(m.Key, true);
                            else ret += print(m.Key, false) + "^" + print_pow(m.Value);
                        }
                    } else {
                        if (! first) ret += "*";
                        ret += print(m.Key, false) + "^" + print_pow(m.Value);
                    }
                    first = false;
                }
            }
            return ret;
        }
        public string print(Many m)
        {
            string ret = ""; bool first = true;
            foreach (KeyValuePair<One, Complex> o in m.data)
            {
                ret += print(o.Key, o.Value, first);
                first = false;
            }
            return ret;
        }
        public string print(Many2 m)
        {
            string ret = "";
            if (Complex.isint(m.down.get_Num(), 1))
            {
                ret += print(m.up);
            }
            else
            {
                ret += "(" + print(m.up) + ") / (" + print(m.down) + ")";
            }
            return "(" + ret + ")";
        }
        public string print(Func f, bool nopow)
        {
            if (f == null) return "";
            string ret = IDS.root.funcs_name[f.type];
            Action[] p = {
                  () => {
                      ret += (flag_out_desc ? ((Vals)(f.data)).var.desc : ((Vals)(f.data)).get_name());
                  },
                  () => {
                      ret += print((Complex)(f.data), nopow, false);
                  },
                  () => {ret += print((Many2)(f.data));},
                  () => {ret += print((Many2)(f.data));},
                  () => {ret += print((Many2)(f.data));},
                  () => {ret += print((Many2)(f.data));},
                  () => {ret += print((Many2)(f.data));},
                  () => {
                      ret += "(" + ((Row)(f.data)).point.ToString();
                      foreach(KeyValuePair<int,Many2> m in ((Row)(f.data)).data) {
                          ret += "," + print(m.Value);
                      }
                      ret += ")";
                  },
                  () => {
                      ret += "[";
                      int ix,iy;
                      ix = 0; while (ix < ((Matrix)(f.data)).x) {
                          ret += (ix > 0 ? ",[" : "[");
                          iy = 0; while (iy < ((Matrix)(f.data)).y) {
                              ret += (iy > 0 ? "," : "") + print(((Matrix)(f.data)).data[ix,iy],false);
                              iy++;
                          }
                          ret += "]";
                          ix++;
                      }
                      ret += "]";
                  }
                 };
            p[f.type]();
            return ret;
        }
        public string print_pow(Func f)
        {
            if (f == null) return "";
            Func<string>[] p = {
                  () => {
                      return ((Vals)(f.data)).get_name();
                  },
                  () => {
                      return print((Complex)(f.data),false,true);
                  },
                  () => {return IDS.root.funcs_name[2] + print((Many2)(f.data));},
                  () => {return IDS.root.funcs_name[3] + print((Many2)(f.data));},
                  () => {return IDS.root.funcs_name[4] + print((Many2)(f.data));},
                  () => {return IDS.root.funcs_name[5] + print((Many2)(f.data));},
                  () => {return IDS.root.funcs_name[6] + print((Many2)(f.data));},
                  () => {
                      string _r = IDS.root.funcs_name[7] + "(" + ((Row)(f.data)).point.ToString();
                      foreach(KeyValuePair<int,Many2> m in ((Row)(f.data)).data) {
                          _r += "," + print(m.Value);
                      }
                      return _r + ")";
                  }
                 };
            return p[f.type]();
        }
        public string print(Vars v)
        {
            return (flag_out_desc ? v.desc : v.name) + " = " + (v.var == null ? "" : print(v.var, false)) + ";";
        }
        public string print(int v)
        {
            return print(Vars.inds[v]);
        }
    }


    public class Fdim
    {
        public Vars var;
        public Complex now, from, step, to;
        Fdim()
        {
        }
        public Fdim(Vars v, Complex n)
        {
            var = v; from = n; now = new Complex(n); step = new Complex(0); to = new Complex(0);
        }
        public Fdim(Vars v, Complex f, Complex s, Complex t)
        {
            var = v; from = f; now = new Complex(f); step = new Complex(s); to = new Complex(t);
            Complex _t = Complex.sub(to, from);
            if ((_t.sign() == 0) || (_t.r.sign != step.r.sign) || (_t.i.sign != step.i.sign)) IDS.root.sys.error("fcalc: wrong direction");
        }
        public void save(BinaryWriter file)
        {
            file.Write((Int32)var.ind);
            now.save(file);
            from.save(file);
            step.save(file);
            to.save(file);
        }
        public static Fdim load(BinaryReader file)
        {
            Fdim ret = new Fdim();
            ret.var = Vars.inds[file.ReadInt32()];
            ret.now = Complex.load(file);
            ret.from = Complex.load(file);
            ret.step = Complex.load(file);
            ret.to = Complex.load(file);
            return ret;
        }
        public bool next()
        {
            bool r = false;
            if (step.sign() != 0)
            {
                now = Complex.add(now, step);
                if (to.r.great(now.r) || to.i.great(now.i)) now.set(from); else r = true;
            }
            return r;
        }
    }
    public class Fborder
    {
        public Complex from, to, siz0;
        public Complex max, min;
        public int scale;
        public Fborder(Complex _nfr, Complex _n, int sc)
        {
            from = _nfr; to = _n;
            max = null; min = null; siz0 = null;
            scale = sc;
            if ((from != null) && (to != null))
            {
                siz0 = Complex.sub(to, from);
                siz0.r.div(); siz0.i.div();
            }
        }

        public static void save(BinaryWriter file, Fborder f)
        {
            if (f == null) file.Write(false);
            else
            {
                file.Write(true);
                Complex.save(file, f.from);
                Complex.save(file, f.to);
                Complex.save(file, f.min);
                Complex.save(file, f.max);
                file.Write((Int32)f.scale);
            }
        }
        public static Fborder load(BinaryReader file)
        {
            Fborder ret = null;
            if (file.ReadBoolean())
            {
                ret = new Fborder(Complex.load(file), Complex.load(file), 0);
                ret.min = Complex.load(file);
                ret.max = Complex.load(file);
                ret.scale = file.ReadInt32();
            }
            return ret;
        }

        public Complex get(Complex _n)
        {
            Complex ret = new Complex(_n);
            if (max == null) max = new Complex(ret);
            else
            {
                if (max.r.great(ret.r)) max.r = ret.r;
                if (max.i.great(ret.i)) max.i = ret.i;
            }
            if (min == null) min = new Complex(ret);
            else
            {
                if (ret.r.great(min.r)) min.r = ret.r;
                if (ret.i.great(min.i)) min.i = ret.i;
            }
            return ret;
        }
        public Complex getnow(Complex c)
        {
            Complex ret = Complex.sub(c, from);
            if (ret.r.sign < 0) ret.r = IDS.nums[IDS.znums];
            if (ret.i.sign < 0) ret.i = IDS.nums[IDS.znums];
            ret.r = Num.mul(ret.r, siz0.r);
            ret.i = Num.mul(ret.i, siz0.i);
            if (ret.r.up > ret.r.down) ret.r = IDS.nums[IDS.znums + 1];
            if (ret.i.up > ret.i.down) ret.i = IDS.nums[IDS.znums + 1];
            return ret;
        }
        static Func<double, double>[] _sc = {
            (double _t) => {double _tm = 1-_t, _t3 = _tm*_tm*_tm; return 1-_t3*_t3*_t3;},
            (double _t) => {double _tm = 1-_t, _t3 = _tm*_tm*_tm; return 1-_t3*_t3*_tm*_tm;},
            (double _t) => {double _tm = 1-_t, _t3 = _tm*_tm*_tm; return 1-_t3*_t3*_tm;},
            (double _t) => {double _tm = 1-_t, _t2 = _tm*_tm; return 1-_t2*_t2*_t2;},
            (double _t) => {double _tm = 1-_t, _t2 = _tm*_tm; return 1-_t2*_t2*_tm;},
            (double _t) => {double _tm = 1-_t, _t2 = _tm*_tm; return 1-_t2*_t2;},
            (double _t) => {double _tm = 1-_t; return 1-_tm*_tm*_tm;},
            (double _t) => {double _tm = 1-_t; return 1-_tm*_tm;},
            (double _t) => {return _t;},
            (double _t) => {return 1;},
            (double _t) => {return _t;},
            (double _t) => {return _t*_t;},
            (double _t) => {return _t*_t*_t;},
            (double _t) => {double _t2 = _t*_t; return _t2*_t2;},
            (double _t) => {double _t2 = _t*_t; return _t2*_t2*_t;},
            (double _t) => {double _t2 = _t*_t; return _t2*_t2*_t2;},
            (double _t) => {double _t3 = _t*_t*_t; return _t3*_t3*_t;},
            (double _t) => {double _t3 = _t*_t*_t; return _t3*_t3*_t*_t;},
            (double _t) => {double _t3 = _t*_t*_t; return _t3*_t3*_t3;}
        };
        public void reborder(Complex[] _c)
        {
            if (from == null) from = min;
            if (to == null) to = max;
            Complex siz = Complex.sub(to, from); siz.div();

            int i = 0; while (i < _c.Length)
            {
                if (_c[i] != null)
                {
                    _c[i].sub(from);
                    if (_c[i].r.sign < 0) _c[i].r = IDS.nums[IDS.znums];
                    if (_c[i].i.sign < 0) _c[i].i = IDS.nums[IDS.znums];
                    _c[i].mul(siz);
                    if (_c[i].r.up > _c[i].r.down) _c[i].r = IDS.nums[IDS.znums + 1];
                    if (_c[i].i.up > _c[i].i.down) _c[i].i = IDS.nums[IDS.znums + 1];
                }
                i++;
            }
        }
        public double[] rdouble(Complex[] _c)
        {
            double[] ret = new double[_c.Length];
            int i = 0; while (i < _c.Length)
            {
                ret[i] = (_c[i] == null ? double.NaN : _sc[scale](_c[i].r.todouble()));
                i++;
            }
            return ret;
        }
        public double[] idouble(Complex[] _c)
        {
            double[] ret = new double[_c.Length];
            int i = 0; while (i < _c.Length)
            {
                ret[i] = (_c[i] == null ? double.NaN : _sc[scale](_c[i].r.todouble()));
                i++;
            }
            return ret;
        }
    }
    public class Fdo
    {
        public int type, parm;
        public int x0, y0, xs, ys;
        public Fdo(int _t, int _p, int _x0, int _y0, int _xs, int _ys)
        {
            type = _t; parm = _p; x0 = _x0; y0 = _y0; xs = _xs; ys = _ys;
        }
        public virtual void save(BinaryWriter file) { }
        public void _save(BinaryWriter file)
        {
            file.Write((Int32)type); file.Write((Int32)(parm));
            file.Write((Int32)x0);
            file.Write((Int32)y0);
            file.Write((Int32)xs);
            file.Write((Int32)ys);
        }
        public static Fdo load(BinaryReader file)
        {
            int _t, _p, _x0, _y0, _xs, _ys;
            _t = file.ReadInt32(); _p = file.ReadInt32();
            _x0 = file.ReadInt32();
            _y0 = file.ReadInt32();
            _xs = file.ReadInt32();
            _ys = file.ReadInt32();
            switch (_t)
            {
                case 0:
                    Fdo0 r0 = new Fdo0(_t, _p, _x0, _y0, _xs, _ys);
                    r0._load(file); return r0;
                case 1:
                    Fdo1 r1 = new Fdo1(_t, _p, _x0, _y0, _xs, _ys);
                    r1._load(file); return r1;
                case 2:
                    Fdo2 r2 = new Fdo2(_t, _p, _x0, _y0, _xs, _ys);
                    r2._load(file); return r2;
            }
            return null;
        }
        public virtual void doit() { }
        public virtual void finish() { }
    }
    public class Fdo0 : Fdo
    {
        string sb = "", sa = "";
        public Vals x;
        public Fdo0(int _t, int _p, int _x0, int _y0, int _xs, int _ys) : base(_t, _p, _x0, _y0, _xs, _ys) { }
        public Fdo0(int p, string b, string a, Vals v, int i0, int i1)
            : base(0, p, i0, 0, i1, 0)
        {
            sb = b; sa = a; x = v;
        }
        public override void save(BinaryWriter file)
        {
            base._save(file);
            file.Write(sb); file.Write(sa);
            Vals.save(file, x);
        }
        public void _load(BinaryReader file)
        {
            sb = file.ReadString(); sa = file.ReadString();
            x = Vals.load(file);
        }
        public override void doit()
        {
            //print x"str" to #parm
            IDS.root.sys.wstr(parm, sb + (x == null ? "" : (x0 < 0 ? IDS.root.par.print(x.get_val(), 0) : (x0 == 0 ? x.get_val().toint().ToString().Trim() : IDS.root.par.print(x.get_val(), x0, xs)))) + sa);
        }
    }
    public class Fdo1 : Fdo
    {
        public Vals x, y;
        public Fborder tx, ty;
        public Complex[] y1;
        public Complex min, max;
        public Fdo1(int _t, int _p, int _x0, int _y0, int _xs, int _ys) : base(_t, _p, _x0, _y0, _xs, _ys) { }
        public Fdo1(int _p, int _x0, int _y0, int _xs, int _ys, Vals _x, Fborder _tx, Vals _y, Fborder _ty)
            : base(1, _p, _x0, _y0, _xs, _ys)
        {
            x = _x; tx = _tx; y = _y; ty = _ty;
            y1 = new Complex[_xs];
        }
        public override void save(BinaryWriter file)
        {
            base._save(file);
            Vals.save(file, x);
            Vals.save(file, y);
            Fborder.save(file, tx);
            Fborder.save(file, ty);
            Complex.save_a(file, y1);
        }
        public void _load(BinaryReader file)
        {
            x = Vals.load(file);
            y = Vals.load(file);
            tx = Fborder.load(file);
            ty = Fborder.load(file);
            y1 = Complex.load_a(file);
        }
        public override void doit()
        {
            int _x;
            _x = (int)(tx.getnow(x.get_val()).r.todouble() * ((double)y1.Length - 0.000000000001));
            y1[_x] = ty.get(y.get_val());
        }
        public override void finish()
        {
            ty.reborder(y1);
            Graphics _g = Graphics.FromImage(IDS.root.pic);
            Pen _p = new Pen(Color.White);
            int i, px = -1, py = -1, _y, _x;
            double[] _r = ty.rdouble(y1);
            if (parm == 0)
            {
                i = 0; while (i < y1.Length)
                {
                    if (!double.IsNaN(_r[i]))
                    {
                        _y = (int)(ys - 1 - _r[i] * ys);
                        if (px > -1) _g.DrawLine(_p, px, py, i + x0, _y + y0);
                        px = i + x0; py = _y + y0;
                    }
                    i++;
                }
            }
            else
            {
                double[] _i = ty.idouble(y1);
                i = 0; while (i < y1.Length)
                {
                    if (!double.IsNaN(_r[i]))
                    {
                        _y = (int)(ys - 1 - _r[i] * ys);
                        _x = (int)(_i[i] * xs);
                        if (px > -1) _g.DrawLine(_p, px, py, _x + x0, _y + y0);
                        px = _x + x0; py = _y + y0;
                    }
                    i++;
                }
            }
            _g.Dispose();
        }
    }
    public class Fdo2 : Fdo
    {
        public Vals x, y, r, g, b;
        public Fborder tx, ty, tr, tg, tb;
        public Complex[] r2, g2, b2;
        public Fdo2(int _t, int _p, int _x0, int _y0, int _xs, int _ys) : base(_t, _p, _x0, _y0, _xs, _ys) { }
        public Fdo2(int _x0, int _y0, int _xs, int _ys, Vals _x, Fborder _tx, Vals _y, Fborder _ty, Vals _r, Fborder _tr, Vals _g, Fborder _tg, Vals _b, Fborder _tb)
            : base(2, 0, _x0, _y0, _xs, _ys)
        {
            x = _x; tx = _tx; y = _y; ty = _ty;
            r = _r; tr = _tr;
            g = _g; tg = _tg;
            b = _b; tb = _tb;
            r2 = new Complex[ys * xs];
            g2 = new Complex[ys * xs];
            b2 = new Complex[ys * xs];
        }
        public override void save(BinaryWriter file)
        {
            base._save(file);
            Vals.save(file, x);
            Vals.save(file, y);
            Vals.save(file, r);
            Vals.save(file, g);
            Vals.save(file, b);
            Fborder.save(file, tx);
            Fborder.save(file, ty);
            Fborder.save(file, tr);
            Fborder.save(file, tg);
            Fborder.save(file, tb);
            Complex.save_a(file, r2);
            Complex.save_a(file, g2);
            Complex.save_a(file, b2);
        }
        public void _load(BinaryReader file)
        {
            x = Vals.load(file);
            y = Vals.load(file);
            r = Vals.load(file);
            g = Vals.load(file);
            b = Vals.load(file);
            tx = Fborder.load(file);
            ty = Fborder.load(file);
            tr = Fborder.load(file);
            tg = Fborder.load(file);
            tb = Fborder.load(file);
            r2 = Complex.load_a(file);
            g2 = Complex.load_a(file);
            b2 = Complex.load_a(file);
        }
        public override void doit()
        {
            int _x, _y;
            _x = (int)(tx.getnow(x.get_val()).r.todouble() * ((double)xs - 0.000000000001));
            _y = (int)(ty.getnow(y.get_val()).r.todouble() * ((double)ys - 0.000000000001));
            if (r != null) r2[_y * xs + _x] = tr.get(r.get_val());
            if (g != null) g2[_y * xs + _x] = tg.get(g.get_val());
            if (b != null) b2[_y * xs + _x] = tb.get(b.get_val());
        }
        public override void finish()
        {
            double[] _r2 = null, _g2 = null, _b2 = null;
            if (r != null) { tr.reborder(r2); _r2 = tr.rdouble(r2); }
            if (g != null) { tg.reborder(g2); _g2 = tg.rdouble(g2); }
            if (b != null) { tb.reborder(b2); _b2 = tb.rdouble(b2); }
            int a = 0, ix, iy = 0; while (iy < ys)
            {
                ix = 0; while (ix < xs)
                {
                    IDS.root.pic.SetPixel(ix + x0, iy + y0, Color.FromArgb(
                        (r == null ? 0 : (int)(_r2[a] * 255)),
                        (g == null ? 0 : (int)(_g2[a] * 255)),
                        (b == null ? 0 : (int)(_b2[a] * 255))));
                    a++; ix++;
                }
                iy++;
            }
        }
    }
    public class Flow
    {
        public int[] patch;
        public BinaryReader read = null;
        public List<Func> id;
        DateTime time;
        public Flow(int size, int sec)
        {
            patch = new int[size];
            id = new List<Func>();
            time = DateTime.Now.AddSeconds(sec);
        }
        public void save(BinaryWriter file)
        {
            file.Write((UInt16)patch.Length);
            foreach (int i in patch) file.Write((Int32)i);
            file.Write((UInt16)id.Count);
            foreach (Func f in id) f.save(file);
        }
        public static Flow load(BinaryReader file, int time)
        {
            Flow ret = new Flow(file.ReadUInt16(), time);
            int i = 0; while (i < ret.patch.Length) ret.patch[i++] = file.ReadInt32();
            int cnt = file.ReadUInt16(); i = 0; while (i < cnt) { ret.id.Add(Func.load(file)); i++; }
            ret.read = file;
            return ret;
        }
        public void clear()
        {
            int i = 0; while (i < patch.Length) patch[i++] = 0;
            id.Clear();
        }
        public void set_bit(int level, int bit)
        {
            patch[level] |= (1 << bit);
        }
        public bool is_bit(int level, int bit)
        {
            return (patch[level] & (1 << bit)) != 0;
        }
        public Func get_var_func(int level) { return Vars.inds[patch[level]].var; }
        public Vars get_var(int level) { return Vars.inds[patch[level]]; }
        public void save(Action<BinaryWriter> _s)
        {
            BinaryWriter _f = IDS.root.save(IDS.root.sys.name + ".bin");
            _s(_f);
            _f.Close();
            IDS.root.sys.flag = IDS.root.flag();
        }
        public void select(int level, Action[] sel)
        {
            if (patch[level] < sel.Length) sel[patch[level]](); else IDS.root.sys.error("flow: select");
        }
        public void repeat(int level, int max, Action<int> rep, Action<BinaryWriter> savex)
        {
            while (patch[level] < max)
            {
                if (DateTime.Now > time)
                {
                    save(savex); IDS.root.sys.finish();
                }
                rep(patch[level]);
                patch[level]++;
            }
            patch[level] = 0;
        }
        public void repeat<T>(int level, List<T> list, Action<T> rep, Action<BinaryWriter> savex)
        {
            while (patch[level] < list.Count)
            {
                if (DateTime.Now > time)
                {
                    save(savex); IDS.root.sys.finish();
                }
                rep(list[patch[level]]);
                patch[level]++;
            }
            patch[level] = 0;
        }
        public void repeat(int level, Action[] rep, Action<BinaryWriter> savex)
        {
            while (patch[level] < rep.Length)
            {
                if (DateTime.Now > time)
                {
                    save(savex); IDS.root.sys.finish();
                }
                rep[patch[level]]();
                patch[level]++;
            }
        }
        public void repeat(Func<bool> rep, Action<BinaryWriter> savex)
        {
            do
            {
                if (DateTime.Now > time)
                {
                    save(savex); IDS.root.sys.finish();
                }
            } while (rep());
        }
        public void add_id(Vals _v)
        {
            Func f = new Func(_v);
            foreach (Func _f in id) if (f.CompareTo(_f) == 0) IDS.root.sys.error(_v.get_name() + " duplicate");
            id.Add(f);
        }
    }
    public static class Program
    {
        static IDS root;
        static int Main(string[] args)
        {
            if (args.Length < 1) return 0;
            try
            {
                Vars._ind = 0; Vals._ind = 0;
                Vals.inds = new Vals[100];
                Vars.inds = new Vars[100];
                Fileio _f = new Fileio(args[0]);
                root = new IDS(_f, new Parse(_f));
                doit();
            }
            catch (FinishException fe)
            {
                return 0;
                /*            } catch (Exception ee) {
                                root.par.sys.wline(0,"Position " + IDS.root.par.pos.ToString() + ": " + ((IDS.now_func != null) ? IDS.root.par.print(IDS.now_func,true) + " " : "") + "fatal: " + ee.ToString());
                                root.par.sys.flag = "#error:fatal:" + ee.ToString();
                                root.par.sys.finish(false);
                                File.Delete(root.sys.name + ".bin");
                                return -1;
                            */
            }
            File.Delete(root.sys.name + ".bin");
            return 0;
        }
        struct calc_out
        {
            public readonly string str;
            public readonly int val, nout;
            public calc_out(string s, int v, int o)
            {
                str = s; val = v; nout = o;
            }
        };
        static Fborder border(List<Fdim> fdim, Vals v)
        {
            int sc = 10;
            Parse par = IDS.root.par;
            if (par.isequnow('<'))
            {
                par.next();
                if (par.isequnow(Parse.isnum)) { sc = 9 - (par.now - '0'); par.next(); }
            }
            if (par.isequnow('>'))
            {
                par.next();
                if (par.isequnow(Parse.isnum)) { sc = 9 + (par.now - '0'); par.next(); }
            }
            if (par.isequnow(','))
            {
                foreach (Fdim fd in fdim) if ((fd.var == v.var) && (fd.step.sign() != 0)) return new Fborder(fd.from, fd.to, sc);
                IDS.root.sys.error("draw: no interval");
            }
            else
            {
                Complex n0 = null, ns = null;
                if (par.isequnow('{')) n0 = par.calc(); else if (par.isequnow('@')) par.next(); else IDS.root.sys.error("wrong draw");
                if (par.isequnow('{')) ns = par.calc(); else if (par.isequnow('@')) par.next(); else IDS.root.sys.error("wrong draw");
                return new Fborder(n0, ns, sc);
            }
            return null;
        }
        static List<Func> d_vals(Func v)
        {
            List<Func> r = new List<Func>();
            One o = new One();
            v.findvals(o);
            foreach (KeyValuePair<Func, Func> m in o.exps) r.Add(m.Key);
            return r;
        }
        static string[] str_proc(string s)
        {
            List<string> r = new List<string>(); string t0, t1, ac;
            ac = ""; int i1, i0 = 0; while (i0 < s.Length)
            {
                if ((i1 = s.IndexOf('%', i0)) < 0) break;
                t0 = s.Substring(i0, i1 - i0);
                t1 = s.Substring(i1 + 1) + " ";
                switch (t1[0])
                {
                    case '%': t0 += "%"; i1++; break;
                    case '\'': t0 += '"'; i1++; break;
                    case 'n': t0 += '\n'; i1++; break;
                    default:
                        r.Add(ac + t0); ac = ""; t0 = "";
                        break;
                }
                ac += t0; i0 = i1 + 1;
            }
            r.Add(ac + s.Substring(i0));
            return r.ToArray();
        }
        static char[] spl = { '.' };
        static void doit()
        {
            Vars var0;
            Vals val0;
            string val;
            bool repeat;
            MAO_dict mao = null;
            Parse par = IDS.root.par;
            Flow flow = IDS.root.flow;
            List<Fdim> fdim = new List<Fdim>();
            List<Fdo> fdo = new List<Fdo>();
            const int flow_level_oper = 0;
            const int flow_level_var = 1;
            const int flow_level_flag = 2;
            const int flow_level_step0 = 3;
            const int flow_level_step1 = 4;
            Action[] load = {
                () => {},
                () => {if (IDS.root.flow.is_bit(flow_level_flag,0)) mao = MAO_dict.load(IDS.root.fload);},
                () => {},
                () => {
                    int _i = 0, cnt = IDS.root.fload.ReadInt16();
                    while (_i < cnt) {fdim.Add(Fdim.load(IDS.root.fload)); _i++;}
                    _i = 0; cnt = IDS.root.fload.ReadInt16();
                    while (_i < cnt) {fdo.Add(Fdo.load(IDS.root.fload)); _i++;}
                }
            };

            if (IDS.root.fload != null)
            {
                load[flow.patch[flow_level_oper]]();
                IDS.root.fload.Close();
                File.Delete("step.bin");
            }

            Action<BinaryWriter> save1 = (BinaryWriter _f) =>
            {
                if (mao != null) mao.save(_f);
            };
            Action<BinaryWriter> save2 = (BinaryWriter _f) =>
            {
            };
            Action<BinaryWriter> save3 = (BinaryWriter _f) =>
            {
                _f.Write((Int16)fdim.Count);
                foreach (Fdim fd in fdim) fd.save(_f);
                _f.Write((Int16)fdo.Count);
                foreach (Fdo fd in fdo) fd.save(_f);
            };
            Action<Func> expand_revert = (Func _i) => flow.get_var_func(flow_level_var).revert(_i);
            Action<Func> expand_expand = (Func _i) =>
            {
                if (flow.is_bit(flow_level_flag, 0)) mao.expand(0, mao.val(_i)); else flow.get_var_func(flow_level_var).expand(_i);
            };
            Action[] expand = {
            () => {
                flow.repeat(flow_level_step1,flow.id,expand_revert,save1);
                if (flow.is_bit(flow_level_flag,0)) mao.val(new Func(flow.get_var(flow_level_var).vals[0]));
            },
            () => flow.repeat(flow_level_step1,flow.id,expand_expand,save1),
            () => {
                if (flow.is_bit(flow_level_flag,0)) flow.get_var(flow_level_var).var = mao.mao[0].to_func(); else flow.get_var_func(flow_level_var).simple();
            },
            () => {
                if (flow.is_bit(flow_level_flag,1)) Vars.inds[flow.patch[flow_level_var]].var.expand();
            },
            () => {
                if (flow.is_bit(flow_level_flag,2) && ((flow.get_var_func(flow_level_var).type == 2) && (((Many2)(flow.get_var_func(flow_level_var).data)).down.type_exp() < 2)))
                {
                    ((Many2)(flow.get_var_func(flow_level_var).data)).down.div();
                    ((Many2)(flow.get_var_func(flow_level_var).data)).up.mul(((Many2)(flow.get_var_func(flow_level_var).data)).down.data.ElementAt(0).Key,((Many2)(flow.get_var_func(flow_level_var).data)).down.data.ElementAt(0).Value);
                    ((Many2)(flow.get_var_func(flow_level_var).data)).down = new Many(new Complex(1));
                }
                flow.get_var_func(flow_level_var).simple();
                par.sys.wline(0,par.print(flow.patch[flow_level_var]));
            }
            };
            Func<Num, string> _tostr = (Num _n) =>
            {
                string ret = "";
                switch (_n.sign)
                {
                    case -1:
                        ret = "n";
                        break;
                    case 0:
                        return "0";
                    case 1:
                        ret = "p";
                        break;
                }
                ret += _n.up.ToString();
                if (_n.down > 1) ret += "_" + _n.down.ToString();
                return ret;
            };
            Func<Complex, string> tostr = (Complex _n) =>
            {
                return _tostr(_n.r) + (_n.i.sign == 0 ? "" : "_" + _tostr(_n.i));
            };
            Func<bool> extract = () =>
            {
                if (flow.id.Count == 1)
                {
                    SortedDictionary<int, int> lout = new SortedDictionary<int, int>();
                    Func extr = flow.id[0];
                    Vals ve = (Vals)(extr.data);
                    string n0 = ve.var.name + "_" + Vals.inds[flow.patch[flow_level_var]].var.name + "_", n1;
                    One _o = null;
                    Complex pow = null, div = ((Many2)(flow.get_var_func(flow_level_var).data)).down.get_Num(); div.div();
                    Vars va = null;
                    foreach (KeyValuePair<One, Complex> m in ((Many2)(flow.get_var_func(flow_level_var).data)).up.data)
                    {
                        if (m.Key.exps.ContainsKey(extr))
                        {
                            if (m.Key.exps[extr].type != Func.t_num) par.sys.error("@ only numeric exponent");
                            pow = (Complex)(m.Key.exps[extr].data);
                            _o = new One(m.Key);
                            _o.exps.Remove(extr);
                        }
                        else { _o = m.Key; pow = new Complex(0); }
                        n1 = n0 + tostr(pow);
                        va = root.findadd_var(n1);
                        if (!lout.ContainsKey(va.ind)) lout.Add(va.ind, 0);
                        if (va.var == null) va.var = new Func(new Many2(_o, Complex.mul(m.Value, div)));
                        else ((Many2)(va.var.data)).up.add(_o, Complex.mul(m.Value, div));
                    }
                    foreach (KeyValuePair<int, int> k in lout)
                    {
                        par.sys.wline(0, par.print(Vars.inds[k.Key]));
                    }
                }
                else
                {
                    SortedDictionary<One, Many> extra = new SortedDictionary<One, Many>();
                    One from, to;
                    Many2 doit = (Many2)(flow.get_var_func(flow_level_var).data);
                    string ns = flow.get_var(flow_level_var).name + "_", nn;
                    foreach (Func _f in flow.id)
                    {
                        ns += ((Vals)_f.data).var.name + "_";
                    }
                    foreach (KeyValuePair<One, Complex> m in doit.up.data)
                    {
                        to = new One();
                        from = new One(m.Key);
                        foreach (Func _f in flow.id)
                        {
                            if (from.exps.ContainsKey(_f))
                            {
                                to.exps.Add(_f, from.exps[_f]);
                                from.exps.Remove(_f);
                            }
                        }
                        if (extra.ContainsKey(to)) extra[to].add(from, m.Value);
                        else extra.Add(to, new Many(from, new Complex(m.Value)));
                    }
                    doit.up.data.Clear();
                    int vs = 0;
                    Vars vr; One ores;
                    foreach (KeyValuePair<One, Many> _res in extra)
                    {
                        nn = ns + vs.ToString();
                        if (root.var.ContainsKey(nn)) par.sys.error(nn + " @ already exist");
                        vr = root.findadd_var(nn);
                        _res.Value.simple();
                        vr.var = new Func(new Many2(_res.Value));
                        ores = new One(_res.Key); ores.exps.Add(new Func(vr.vals[0]), new Func(new Complex(1)));
                        doit.up.data.Add(ores, new Complex(1));
                        par.sys.wline(0, par.print(vr));
                        vs++;
                    }
                    par.sys.wline(0, par.print(flow.get_var(flow_level_var)));
                }
                return false;
            };
            Func<bool> numeric = () =>
            {
                root.uncalc(); foreach (Fdim fd in fdim) fd.var.set_now(fd.now);
                foreach (Fdo fd in fdo) fd.doit();
                foreach (Fdim fd in fdim)
                {
                    if (fd.next()) return true;
                }
                foreach (Fdo fd in fdo) fd.finish();
                return false;
            };



            Action[] oper = {
            () => {},
            () => flow.repeat(flow_level_step0,expand,save1),
            () => flow.repeat(extract,save2),
            () => flow.repeat(numeric,save3)
            };

            char[] c_flag = { '^', 'N', ';' };
            Action[] a_flag = {
                () => {par.flag_out_exptomul = ! par.flag_out_exptomul;},
                () => {par.flag_out_desc = ! par.flag_out_desc;},
                () => {repeat = false; },
                () => {repeat = false; }
                };

            do
            {
                flow.select(flow_level_oper, oper);
                flow.clear(); mao = null;

                if (par.body_num >= par.body.Count) break;
                if (par.main_oper != '=') IDS.root.n_step++;

                switch (par.main_oper)
                {
                    case '%':
                        do {
                            par.next();
                            repeat = true;
                            par.branchnow(c_flag, a_flag);
                        } while (repeat);
                        break;
                    case '=':
                        if (root.fnames.ContainsKey(par.name)) IDS.root.sys.error("reserved name");
                        var0 = root.findadd_var(par.name);
                        if (var0.ind < IDS.v_res) IDS.root.sys.error("reserved var");
                        par.next();
                        if (par.isequnow('"')) var0.desc = par.get("\"");
                        var0.var = (par.isequnow(Parse.isend) ? null : par.fpars("", false));
                        par.sys.wline(0, par.print(var0));
                        break;
                    case '$':
                        flow.patch[flow_level_oper] = 1;
                        flow.patch[flow_level_var] = root.find_var_fill(par.name).ind;
                        par.next();
                        if (par.isequnow('!'))
                        {
                            flow.set_bit(flow_level_flag, 0); par.next(); mao = new MAO_dict(par.get_int()); par.next();
                        }
                        if (par.isequnow('*'))
                        {
                            flow.id = d_vals(flow.get_var_func(flow_level_var)); par.next();
                        }
                        else
                        {
                            flow.id.Clear();
                            while (par.isequnow(Parse.isname))
                            {
                                val = par.get(Parse.isname);
                                if (par.isequnow(',')) par.next();
                                val0 = root.find_val(val);
                                if (val0.var.ind == flow.patch[flow_level_var]) par.sys.error(par.name + " $recursion - look recursion");
                                if (val0.var.var != null) flow.add_id(val0);
                            }
                        }
                        if (par.isequnow('@'))
                        {
                            flow.set_bit(flow_level_flag, 1); par.next();
                            if (flow.is_bit(flow_level_flag, 0)) IDS.root.sys.error("cant @ on !");
                        }
                        if (par.isequnow('$')) { flow.set_bit(flow_level_flag, 2); par.next(); }
                        break;

                    case '@':
                        flow.patch[flow_level_oper] = 2;
                        Vars _v = root.find_var_fill(par.name);
                        if ((_v.var.type != Func.t_many2) || (((Many2)(_v.var.data)).down.get_Num() == null)) par.sys.error("@ wrong");
                        flow.patch[flow_level_var] = _v.ind;
                        par.next();
                        flow.id.Clear();
                        while (par.isequnow(Parse.isname))
                        {
                            val = par.get(Parse.isname);
                            if (par.isequnow(',')) par.next();
                            val0 = root.find_val(val);
                            if (val0.var.ind == flow.patch[flow_level_var]) par.sys.error(par.name + " @recursion - look recursion");
                            flow.add_id(val0);
                        }
                        if (flow.id.Count < 1) par.sys.error("@ list empty");
                        break;

                    case '|':
                        var0 = root.find_var_fill(par.name);
                        if (var0.var.type != Func.t_matr) par.sys.error("not matr");
                        par.next();
                        Many det = ((Matrix)(var0.var.data)).det();
                        det.simple(); var0.var = new Func(new Many2(det));
                        par.sys.wline(0, par.print(var0));
                        break;

                    case '<':
                        var0 = root.find_var_fill(par.name);
                        par.next();
                        val0 = root.find_val(par.get(Parse.isname));
                        var0.var.diff_down(val0);
                        par.sys.wline(0, par.print(var0));
                        break;
                    case '>':
                        var0 = root.find_var_fill(par.name);
                        par.next();
                        val0 = root.find_val(par.get(Parse.isname));
                        var0.var.diff_up(val0);
                        par.sys.wline(0, par.print(var0));
                        break;
                    case '[':
                        flow.patch[flow_level_oper] = 3;
                        par.next();
                        fdim.Clear();
                        fdo.Clear();
                        do
                        {
                            if (!par.isequnow(Parse.isabc)) IDS.root.sys.error("calc: wrong");
                            var0 = root.find_var(par.get(Parse.isname));
                            foreach (Fdim fd in fdim) if (fd.var.ind == var0.ind) IDS.root.sys.error("calc: double");
                            switch (par.now)
                            {
                                case '[':
                                    par.next();
                                    fdim.Add(new Fdim(var0, par.calc(), par.calc(), par.calc()));
                                    par.next();
                                    break;
                                case '{':
                                    fdim.Add(new Fdim(var0, par.calc()));
                                    break;
                                default:
                                    IDS.root.sys.error("calc: wrong");
                                    break;
                            }
                            if (par.now == ']') break;
                            par.next();
                        } while (true);
                        par.next();
                        while (par.more())
                        {
                            if (par.isequnow(Parse.isabc))
                            {
                                val0 = root.find_val(par.get(Parse.isname));
                                if (!par.isequnow('"')) IDS.root.sys.error("wrong do");
                                String[] sp = str_proc(par.get("\""));
                                if (sp.Length != 3) IDS.root.sys.error("wrong do");
                                int _a = -1, _b = 0;
                                if (sp[1] == "i") _a = 0;
                                else
                                {
                                    String[] _sp = sp[1].Split(spl);
                                    if (_sp.Length == 2)
                                    {
                                        Int32.TryParse(_sp[0], out _a); if (_sp[1].Length > 0) Int32.TryParse(_sp[1], out _b); else _b = -1;
                                    }
                                }
                                fdo.Add(new Fdo0(par.get_int(), sp[0], sp[2], val0, _a, _b));
                            }
                            else switch (par.now)
                                {
                                    case '"':
                                        val = par.get("\"");
                                        fdo.Add(new Fdo0(par.get_int(), str_proc(val)[0], "", null, -1, 0));
                                        break;
                                    case '[':
                                        int _fx, _sx, _fy, _sy;
                                        par.next();
                                        if (par.isequnow('['))
                                        {
                                            par.next(); _fx = par.get_int();
                                            par.next(); _sx = par.get_int();
                                            par.next(); _fy = par.get_int();
                                            par.next(); _sy = par.get_int();
                                            par.next();
                                            if ((_sx < 4) || (_sy < 4) || (_fx + _sx >= IDS.root.pic.Width) || (_fy + _sy >= IDS.root.pic.Height)) IDS.root.sys.error("draw: wrong");
                                        }
                                        else
                                        {
                                            _fx = 0; _sx = IDS.root.pic.Width - 1; _fy = 0; _sy = IDS.root.pic.Height - 1;
                                        }
                                        Vals[] _tv = new Vals[6];
                                        Fborder[] _tb = new Fborder[6];
                                        Fdo _fd = null;
                                        int _tn = 0;
                                        while (!par.isequnow(']'))
                                        {
                                            if (_tn > 5) root.sys.error("draw: wrong parm num");
                                            if (!par.isequnow(','))
                                            {
                                                _tv[_tn] = root.find_val(par.get(Parse.isname));
                                                _tb[_tn] = border(fdim, _tv[_tn]);
                                            }
                                            if (par.isequnow(',')) par.next();
                                            _tn++;
                                        }
                                        switch (_tn)
                                        {
                                            case 2:
                                                _fd = new Fdo1(0, _fx, _fy, _sx, _sy, _tv[0], _tb[0], _tv[1], _tb[1]);
                                                break;
                                            case 3:
                                                _fd = new Fdo1(1, _fx, _fy, _sx, _sy, _tv[0], _tb[0], _tv[1], _tb[1]);
                                                break;
                                            case 5:
                                                _fd = new Fdo2(_fx, _fy, _sx, _sy, _tv[0], _tb[0], _tv[1], _tb[1], _tv[2], _tb[2], _tv[3], _tb[3], _tv[4], _tb[4]);
                                                break;
                                            default:
                                                root.sys.error("draw: wrong parm num");
                                                break;
                                        }
                                        par.next(); fdo.Add(_fd);
                                        break;
                                }
                            if (par.isequnow(',')) par.next();
                        }
                        break;
                }



            } while (par.bnext());
            IDS.root.flow.select(flow_level_oper, oper);


            par.sys.wline(0, "finished, vars = " + (root.var.Count()).ToString());
            par.sys.flag = "#ok";
            par.sys.finish();
        }
        static void none()
        {
        }
    }
}



/*
    class vlist: IEnumerator,  IEnumerable
    {
        public IDS root;
        public UInt64[] data;
        private int dsize, num, first, pos, rest, rest_ext;
        private vlist ext;
        void init()
        {
            dsize = (root.size >> 6) + 1;
            data = new UInt64[dsize];
            for (int i = 0; i < dsize; i++) data[i] = 0;
        }
        public vlist(IDS h)
        {
            root = h; init();
            Num = 0; ext = null;
            Reset();
        }
        public vlist(vlist v)
        {
            root = v.root; init();
            set(v); Reset();
        }
        public void set(vlist v) 
        {
            ext = null; Num = v.num; first = v.first;
            for (int i = 0; i < dsize; i++) data[i] = v.data[i];
        }
        public void Reset()
        {
            pos = first-1; rest = num; rest_ext = (ext == null ? 0 : ext.num);
        }
        public void link()
        {
            ext = null; Reset();
        }
        public void link(vlist v)
        {
            ext = v; Reset();
        }
        int log2(UInt64 a)
        {
            UInt64 u0,u1;
            u1 =  (a & 0x5555555555555555) + ((a >> 1) & 0x5555555555555555);
            u0 = (u1 & 0x3333333333333333) + ((u1 >> 2) & 0x3333333333333333);
            u1 = (u0 & 0x0F0F0F0F0F0F0F0F) + ((u0 >> 4) & 0x0F0F0F0F0F0F0F0F);
            u0 = (u1 & 0x00FF00FF00FF00FF) + ((u1 >> 8) & 0x00FF00FF00FF00FF);
	        return  (int)(((u0 + (u0 >> 16) + (u0 >> 32) + (u0 >> 48)) & 255)); 
        }
        int dnext(int p)
        {
            UInt64 u0;
            while (p < root.size) {
                if ((u0 = (data[p >> 6] >> (p & 63))) != 0) return p + log2(u0) - 1;
                p = (p & -64) + 64;
            }
            return -1;
        }

        bool dnext()
        {
            if (rest < 1) return false;
            int p = dnext(pos+1);
            if (p < 0) return false;
            rest--; pos = p; return true;
        }

        bool dnext2()
        {
            int p = pos+1; UInt64 u0;
            if ((rest < 1) && (rest_ext < 1)) return false;
            while (p < root.size) {
                if ((u0 = ((ext.data[p >> 6] | data[p >> 6]) >> (p & 63))) != 0) {
                    pos = p + log2(u0) - 1; 
                    if (((data[pos >> 6] >> (pos & 63)) & 1) == 0) rest_ext--; else {
                        rest--;
                        if (((ext.data[pos >> 6] >> (pos & 63)) & 1) != 0) rest_ext--;
                    }
                    return true;
                }
                p = (p & -64) + 64;
            }
            return false;
        }
        public bool isempty() {return Num < 1;}
        public bool MoveNext()
        {
            if (ext == null) return dnext(); else return dnext2();
        }

        object IEnumerator.Current
        {
            get {
                return Current;
            }
        }
        public int Current
        {
            get
            {
                return pos;
            }
        }
        public IEnumerator GetEnumerator()
        {
            return this;
        }
        public void ins(int n)
        {
            UInt64 f = ((UInt64)1 << (n & 63));
            if ((data[n >> 6] & f) == 0) {
                if ((Num == 0) || (first > n)) first = n;
                data[n >> 6] |= f;
                num++; if (pos < n) rest++;
            }
        }
        public void del(int n)
        {
            UInt64 f = ((UInt64)1 << (n & 63));
            if ((data[n >> 6] & f) != 0) {
                if (Num < 1) throw new NotImplementedException();
                data[n >> 6] ^= f;
                num--; if (pos < n) rest--;
                if ((n == first) && (Num > 0)) first = dnext(n+1);
            }
        }
        public void set(int n, bool v) 
        {
            if (v) ins(n); else del(n);
        }
        public int getfirst() {return first;}
    }
*/
/*
                            BigInteger[] ua, da;
                            int val = (int)(pfunc.get_up()),fe = 0, se,ne;
                            ne = data[0].Count;
                            if ((ne < 4) || (ne > 1000)) head.sys.error("row: wrong");
                            ua = new BigInteger[ne];
                            da = new BigInteger[ne];
                            fe = (int)(data[0][0].exps[val].non.get_sup());
                            se = (int)(data[0][1].exps[val].non.get_up()) - fe;
                            if ((fe < 0) || (se < 0) || (se > 11)) head.sys.error("row: wrong exp");
                            BigInteger _u,_d,_um, _dm;
                            Num tn = new Num(head.get_val(val)); tn.exp(fe);
                            _u = tn.get_sup(); _d = 1;
                            tn.set(head.get_val(val)); tn.exp(se);
                            _um = tn.get_sup(); _dm = tn.get_down();
                            for (int i = 0; i < ne; i++)
                            {
                                ua[i] = _u; da[i] = _d; _u *= _um; _d *= _dm;
                            }
                            _u = 0;
                            for (int i = 0; i < ne; i++)
                            {
                                if (data[0][i].exps[val].non.get_sup() != fe + se*i) head.sys.error("row: wrong exp");
                                if (data[0][i].mult.get_down() > 1) 
                                    head.sys.error("row: wrong mul");
                                _u += ua[i] * da[ne - i - 1] * data[0][i].mult.get_sup();
                            }
                            tn.set(head.get_val(val)); tn.exp(fe);
                            _d = da[ne-1] * tn.get_down() * head.values[id].data[1][0].mult.get_sup();
                            rt.set(_u, _d);
 */
/*
        static void slice(IDS root, Many dv, One _ml, string s_val)
        {
            Num e = new Num(); int ip = root.last;
            One odv = null;
            One _dv = new One(_ml); _dv.div();
            SortedDictionary<num, many> res = new SortedDictionary<num, many>();
            int i1 = dv.data[0].Count;
            while (dv.data[0].Count > 0)
            {
                e.zero();
                odv = dv.data[0][0];
                if (odv.test_mul(_ml))
                {
                    while (odv.mul_t(_ml)) e.add(-1);
                    odv.mul(_dv);
                }
                else if (odv.test_mul(_dv))
                {
                    while (odv.mul_t(ref _dv)) e.add(1);
                    odv.mul(ref _ml);
                }
                if (!res.ContainsKey(e)) res.Add(new Num(e), new Many(root, -1));
                res[e].data[0].Add(odv);
                e.zero(); dv.data[0].RemoveAt(0);
                root.sys.progr(i1 - dv.data[0].Count, i1);
            }
            if (res.ContainsKey(e))
            {
                dv.set(res[e]); res.Remove(e);
            }
            e = null;
            string nn; int var1;
            foreach (KeyValuePair<num, many> _d in res)
            {
                nn = s_val + _d.Key.toname(false) + "_" + par.name;
                if ((var1 = root.find_var(nn)) < 0)
                {
                    var1 = root.set_empty(nn);
                    _d.Value.id = var1;
                    root.values[var1] = _d.Value;
                }
                else par.sys.error("@ overwrite " + nn);
            }
            for (int i0 = ip; i0 < root.last; i0++) root.values[i0].print(0);
        }
        static void r_slice(ref IDS root, ref Many dv, ref One _ml)
        {
            Num e = new Num(); int ip = root.last, var1;
            SortedDictionary<string, many> res = new SortedDictionary<string, many>();
            string _k;
            int i0 = 0, i1 = dv.data[0].Count, i2;
            while (dv.data[0].Count > i0)
            {
                i2 = 0; _k = "";
                while (i2 < root.size)
                {
                    if (!_ml.exps[i2].iszero())
                    {
                        if (!dv.data[0][i0].exps[i2].iszero())
                        {
                            if (dv.data[0][i0].exps[i2].vars != null) root.sys.error(" cant extract on multiexp");
                            _k += root.get_name_onval(i2) + dv.data[0][i0].exps[i2].non.toname(true);
                            dv.data[0][i0].exps[i2].zero();
                        }
                    }
                    i2++;
                }
                if (_k == "") i0++;
                else
                {
                    if (!res.ContainsKey(_k)) res.Add(_k, new Many(root, -1));
                    res[_k].data[0].Add(dv.data[0][i0]);
                    dv.data[0].RemoveAt(i0);
                }
                root.sys.progr(i1 - dv.data[0].Count - i0, i1);
            }
            foreach (KeyValuePair<string, many> _d in res)
            {
                _k = _d.Key + "_" + par.name;
                if ((var1 = root.find_var(_k)) < 0)
                {
                    var1 = root.set_empty(_k);
                    _d.Value.id = var1;
                    root.values[var1] = _d.Value;
                }
                else par.sys.error("@ overwrite " + _d.Key);
            }
            for (i0 = ip; i0 < root.last; i0++) root.values[i0].print(0);
        }
*/
