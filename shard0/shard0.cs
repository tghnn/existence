//for Them, who my death and my life
using System;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace shard0
{
    public partial class Shard0 : Form
    {
        private System.ComponentModel.IContainer components = null;
        public System.Drawing.Bitmap bm,bp;
        public int sx, sy;
        public int pr_now = 0, pr_was = 0, l_now = 0, l_was = 0;
        delegate void SetCallback(int i);
        System.Drawing.Graphics Gr;
        public bool rp;
        public Shard0(int x, int y)
        {
            rp = true;
            sx = x; sy = y;
            bm = new System.Drawing.Bitmap(sx, sy);
            for (int i0 = 0; i0 < sx; i0++) for (int i1 = 0; i1 < sy; i1++) bm.SetPixel(i0, i1, Color.FromArgb(0, 0, 0));
            bp = new System.Drawing.Bitmap(sx, 2);
            for (int i0 = 0; i0 < sx; i0++) for (int i1 = 0; i1 < 2; i1++) bp.SetPixel(i0, i1, Color.FromArgb(0, 0, 255));
            this.Width = sx; this.Height = sy + 2;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Gr = this.CreateGraphics();
            Paint += new System.Windows.Forms.PaintEventHandler(this.shard0_Paint);
            InitializeComponent();
        }
        public void Set(int i)
        {
            if (IsDisposed) Environment.Exit(-1);
            if (this.InvokeRequired)
            {
                SetCallback d = new SetCallback(Set);
                this.Invoke(d, new object[] { i });
            }
            else
            {
            bool flg = false;
            switch (i) {
                case 0:
                if (pr_now < pr_was) {
                    for (int i0 = 0; i0 < sx; i0++) bp.SetPixel(i0, 0, Color.FromArgb(0, 0, 255));
                    pr_was = 0; flg = true;
                }
                if (pr_now != pr_was) {
                    for (int i0 = pr_was; i0 < pr_now; i0++) bp.SetPixel(i0, 0, Color.FromArgb(255, 0, 0));
                    flg = true;
                }
                if (l_now != l_was) {
                    for (int i0 = 0; i0 < sx; i0++) bp.SetPixel(i0, 1, (i0 < l_now ? Color.FromArgb(255, 0, 0) : Color.FromArgb(0, 0, 255)));
                    flg = true;
                }
                pr_was = pr_now; l_was = l_now;
                if (flg) Gr.DrawImageUnscaled(bp, 0, 0);
                    break;
                case 1:
                Gr.DrawImageUnscaled(Program.bm1, 0, 2);
                    break;
                case 2:
                bm = Program.bm1.Clone(new Rectangle(0, 0, sx, sy),bm.PixelFormat);
                    break;
            }
            }
        }
        private void From1_Shown(object sender, EventArgs e)
        {
            if (rp) {
                Gr.DrawImageUnscaled(bp, 0, 0);
                Gr.DrawImageUnscaled(bm, 0, 2);
            }
        }
        private void shard0_Paint(object sender, PaintEventArgs e)
        {
            if (rp)
            {
                if ((this.Width != sx) || (this.Height != sy))
                {
                    this.Width = sx; this.Height = sy+2;
                }
                Gr.DrawImageUnscaled(bp, 0, 0);
                Gr.DrawImageUnscaled(bm, 0, 2);
            }
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.CausesValidation = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false; this.MinimizeBox = false;
            this.Name = "shard0";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "shard0";
            this.Width = sx; this.Height = sy+2;
            this.ResumeLayout(false);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                bp.Dispose();
                bm.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    class Exps_n
    {
        public SortedDictionary<Num,Num> data;
        public Num val;
        public Exps_n(Num v)
        {
            data = new SortedDictionary<Num,Num>();
            val = v;
            data.Add(IDS.n0,IDS.n1);
            data.Add(IDS.n1,v);
        }
        public Num exp(Num e) //link to
        {
            if (! data.ContainsKey(e)) {
                Num v = new Num(val); v.exp(e);
                data.Add(e,v);
            }
            return data[e];
        }
        public Num exp() //link to
        {
            return val;
        }
    }

    class Vals
    {
        public static int _ind = 0;
        public int ind;
        public Exps_n val;
        public Vars var;
        public int deep;
        public Vals(Vars vr, Num vl, int d)
        {
            val = new Exps_n(vl);
            var = vr; deep = d; ind = Vals._ind++;
        }
        public Exps_n get_exp()
        {
            if (var.stat != Program.root.stat_calc) { 
                if (var.stat == Program.root.stat_uncalc) {
                    if (deep == 0) IDS.sys.error(var.name + " recursion");
                } else {
                    if (var.var == null) IDS.sys.error(var.name + " var: non is non");
                    var.stat = Program.root.stat_uncalc;
                    var.set_now(var.var.calc());
                }
           }
           return val;
        }
        public Num get_val(Num e)
        {
           return get_exp().exp(e);
        }
        public Num get_val()
        {
           return get_exp().exp();
        }
        public string get_name()
        {
            return new String('\'', deep) + var.name;
        }

    }
    class Vars: IComparable
    {
        public static int _ind = 0;
        public int ind;
        public Vals[] vals;
        public Func var;
        public int stat;
        public string name;
        public Vars(string n, int valn, Num vl)
        {
            name = n; stat = 0; var = null; vals = new Vals[valn+1];
            int i = 0; while (i <= valn) {
                vals[i] = new Vals(this,vl,i); i++;
            }
            ind = Vars._ind++;
        }
        public Vars(string n, Num[] vl)
        {
            name = n; stat = 0; var = null; vals = new Vals[vl.Length+1];
            int i = 0; while (i <= vl.Length) {
                vals[i] = new Vals(this,vl[i],i); i++;
            }
            ind = Vars._ind++;
        }
        public void set_now(Num v0)
        {
            if (stat != Program.root.stat_calc) {
                int i = vals.Length-1;
                while (i > 0) {
                    vals[i].val = vals[i-1].val;
                    i--;
                }
                stat = Program.root.stat_calc;
            }
            vals[0].val = new Exps_n(v0);
        }
        public int CompareTo(object obj) {
            if (obj == null) return 1;
            Vars v = obj as Vars; 
            if (ind == v.ind) return 0;
            if (ind < v.ind) return -1; else return +1;
        }
    }
    class IDS
    {
        public const int znums = 100;
        public static Num n0,n1,n_1,ln_prec,n_e_full,n_e,n_pi_full,n_pi,n_ln2_full,n_ln2;
        public static Fileio sys;
        public static Parse par;
        public static IDS root;
        public static BigInteger exp_max;
        public static int prec2,prec10,sqr_steps,sqr_exp_bits, v_res;
        public static int[] h_to_l2;
        public static Num[] e2,e10, sqr2, nums;
        public static SortedDictionary<Num,Num> ln;
        public static Func now_func = null;
        public int stat_uncalc, stat_calc;
        public SortedDictionary<string,Vars> var;
        public Vars v_e,v_pi,v_ln2,v_x,v_n;
        public Func fzero;
        public One ozero;
        public string[] funcs_name = {"","","","ln","fact","int","sign","row"};
        public SortedDictionary<string,int> fnames;
        public Num[] fact;
        static int _l2(int i) {
            int i0 = 0;
            while (i != 0) {i >>= 1; i0++;}
            return i0;
        }
        public IDS(int sqr_s, int m_e, Fileio f, Parse p)
        {
            int i;
            IDS.sys = f; IDS.par = p; stat_uncalc = 1; stat_calc = 2;
            fnames = new SortedDictionary<string,int>();
            var = new SortedDictionary<string,Vars>();
            IDS.ln = new SortedDictionary<Num,Num>();

            BigInteger b, c;
            IDS.e10 = new Num[2000]; b = 1; i = 0;
            while (i < e10.Count()) {
                IDS.e10[i] = new Num(b); b *= 10; i++;
            }
            IDS.e2 = new Num[110000]; b = 1; i = 0;
            while (i < e2.Count()) {
                IDS.e2[i] = new Num(b); b *= 2; i++;
            }

            IDS.nums = new Num[IDS.znums*2];
            i = 0; while (i < IDS.znums*2) {IDS.nums[i] = new Num(i-IDS.znums); i++;}
            IDS.n0 = nums[IDS.znums];
            IDS.n1 = nums[IDS.znums+1];
            IDS.n_1 = nums[IDS.znums-1];
            IDS.n_pi_full = new Num("3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196442881097566593344612847564823378678316527120190914564856692346034861045432664821339360726024914127372458700660631558817488152092096282925409171536436789259036001133053054882046652138414695194151160943305727036575959195309218611738193261179310511854807446237996274956735188575272489122793818301194912983367336244065664308602139494639522473719070217986094370277053921717629317675238467481846766940513200056812714526356082778577134275778960917363717872146844090122495343014654958537105079227968925892354201995611212902196086403441815981362977477130996051870721134999999837297804995105973173281609631859502445945534690830264252230825334468503526193118817101000313783875288658753320838142061717766914730359825349042875546873115956286388235378759375195778185778053217122680661300192787661119590921642019893809525720106548586327886593615338182796823030195203530185296899577362259941389124972177528347913151557485724245415069595082953311686172785588907509838175463746493931925506040092770167113900984882401285836160356370766010471018194295559619894676783744944825537977472684710404753464620804668425906949129331367702898915210475216205696602405803815019351125338243003558764024749647326391419927260426992279678235478163600934172164121992458631503028618297455570674983850549458858692699569092721079750930295532116534498720275596023648066549911988183479775356636980742654252786255181841757467289097777279380008164706001614524919217321721477235014144197356854816136115735255213347574184946843852332390739414333454776241686251898356948556209921922218427255025425688767179049460165346680498862723279178608578438382796797668145410095388378636095068006422512520511739298489608412848862694560424196528502221066118630674427862203919494504712371378696095636437191728");
            IDS.n_e_full = new Num("2.71828182845904523536028747135266249775724709369995957496696762772407663035354759457138217852516642742746639193200305992181741359662904357290033429526059563073813232862794349076323382988075319525101901157383418793070215408914993488416750924476146066808226480016847741185374234544243710753907774499206955170276183860626133138458300075204493382656029760673711320070932870912744374704723069697720931014169283681902551510865746377211125238978442505695369677078544996996794686445490598793163688923009879312773617821542499922957635148220826989519366803318252886939849646510582093923982948879332036250944311730123819706841614039701983767932068328237646480429531180232878250981945581530175671736133206981125099618188159304169035159888851934580727386673858942287922849989208680582574927961048419844436346324496848756023362482704197862320900216099023530436994184914631409343173814364054625315209618369088870701676839642437814059271456354906130310720851038375051011574770417189861068739696552126715468895703503540212340784981933432106817012100562788023519303322474501585390473041995777709350366041699732972508868769664035557071622684471625607988265178713419512466520103059212366771943252786753985589448969709640975459185695638023637016211204774272283648961342251644507818244235294863637214174023889344124796357437026375529444833799801612549227850925778256209262264832627793338656648162772516401910590049164499828931505660472580277863186415519565324425869829469593080191529872117255634754639644791014590409058629849679128740687050489585867174798546677575732056812884592054133405392200011378630094556068816674001698420558040336379537645203040243225661352783695117788386387443966253224985065499588623428189970773327617178392803494650143455889707194258639877275471096295374152111513683506275260232648472870392076431005958411661205452970302364725492966693811513732275364509888903136020572481765851180630364428123149655070475102544650117272115551948668508003685322818");
            IDS.n_ln2_full = new Num("0.69314718055994530941723212145817656807550013436025525412068000949339362196969471560586332699641868754200148102057068573368552023575813055703267075163507596193072757082837143519030703862389167347112335011536449795523912047517268157493206515552473413952588295045300709532636664265410423915781495204374043038550080194417064167151864471283996817178454695702627163106454615025720740248163777338963855069526066834113727387372292895649354702576265209885969320196505855476470330679365443254763274495125040606943814710468994650622016772042452452961268794654619316517468139267250410380254625965686914419287160829380317271436778265487756648508567407764845146443994046142260319309673540257444607030809608504748663852313818167675143866747664789088143714198549423151997354880375165861275352916610007105355824987941472950929311389715599820565439287170007218085761025236889213244971389320378439353088774825970171559107088236836275898425891853530243634214367061189236789192372314672321720534016492568727477823445353476481149418642386776774406069562657379600867076257199184734022651462837904883062033061144630073719489002743643965002580936519443041191150608094879306786515887090060520346842973619384128965255653968602219412292420757432175748909770675268711581705113700915894266547859596489065305846025866838294002283300538207400567705304678700184162404418833232798386349001563121889560650553151272199398332030751408426091479001265168243443893572472788205486271552741877243002489794540196187233980860831664811490930667519339312890431641370681397776498176974868903887789991296503619270710889264105230924783917373501229842420499568935992206602204654941510613918788574424557751020683703086661948089641218680779020818158858000168811597305618667619918739520076671921459223672060253959543654165531129517598994005600036651356756905124592682574394648316833262490180382424082423145230614096380570070255138770268178516306902551370323405380214501901537402950994226299577964742713");

            IDS.sqr_steps = sqr_s;
//for ^1/2: 4 = 10^380, 5 = 10^760, 6 = 10^1500; 
//for ^x: 4 = 2^60, 5 = 2^121, 6 = 2^242 (72d), 7 = 2^500 (150d)
            IDS.sqr_exp_bits = (int)(IDS.e2[IDS.sqr_steps - 4].up) * 60;
            IDS.prec2 = (IDS.sqr_exp_bits * 11) / 10;
            IDS.prec10 = (IDS.prec2 * 3) / 10;
            IDS.exp_max = new BigInteger(m_e);

            IDS.h_to_l2 = new int[256]; //0-7; 8-15; ..
            i = 0; while (i < IDS.h_to_l2.Length) {
                IDS.h_to_l2[i] = _l2(i) - 1;
                i++;
            }

            IDS.ln_prec = new Num((prec2/4)*2+1); ln_prec.div();
            IDS.n_pi = new Num(IDS.n_pi_full); IDS.n_pi.prec_this();
            IDS.n_e = new Num(IDS.n_e_full); IDS.n_e.prec_this();
            IDS.n_ln2 = new Num(IDS.n_ln2_full); IDS.n_ln2.prec_this();

            IDS.sqr2 = new Num[1000]; //2 = (2:3)
            IDS.sqr2[0] = new Num(1);
            IDS.sqr2[1] = new Num("1.4");
            Num _n = new Num(2);
            Num _2 = new Num(2);
            Num __t;
            i = 2; while (i < IDS.sqr2.Length-1) {
                __t = Num.mul(_n,IDS.sqr2[1]);
                IDS.sqr2[i++] = Num.div(Num.add(_n, __t),_2).simple();
                _n.mul(_2);
                IDS.sqr2[i++] = Num.div(Num.add(__t,_n),_2).simple();
            }

            ozero = new One();
            fzero = new Func(IDS.nums[IDS.znums]);
            i = 2; while (i < funcs_name.Count()) { fnames.Add(funcs_name[i],i); i++; }
            fact = new Num[1000];
            b = new BigInteger(1); c = new BigInteger(1); fact[0] = new Num(b);
            while (b < fact.Count()) {
                c *= b; fact[(int)b] = new Num(c); b++;
            }
            IDS.root = this;
        }
        int deep(string n)
        {
            int i = 0; while ((i < n.Length) && (n[i]=='\'')) i++;
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
        public Vars findadd_var(string n)
        {
            int i = deep(n); string n0 = n.Substring(i);
            if (var.ContainsKey(n0)) {
                if (var[n0].vals.Length != i+1) sys.error(n0 + " wrong deep");
            } else {
                var.Add(n0,new Vars(n0,i,nums[IDS.znums]));
            }
            return var[n0];
        }
        public void uncalc() { stat_uncalc+=2; stat_calc+=2;}
//          val           var
//old    uncalc,calc  uncalc,calc
//uncalc  get           recurs
//calc    get             get
    }

    interface IPower{
        void exp2();
    }

    abstract class Power<T> where T :IPower, new()
    {
        public abstract void set(T s);
        public abstract void set0();
        public abstract void set1();
        public abstract void copy(ref T s);
        public abstract void mul(T m);
        public abstract void div();
        public void exp2() {
            T m0 = new T(); copy(ref m0); mul(m0);
        }
        public void exp(int ex)
        {
            int _e;
            _e = ((ex < 0) ? -ex : ex);
            if (_e == 0) {set1(); return;}
            if (_e > 1) {
                T t = new T(); copy(ref t); 
                int i0 = _e-1; while (i0 > 0) 
                {
                    if ((i0 & 1) != 0) mul(t);
                    t.exp2();
                    i0 >>= 1;
                }
            }
            if (ex < 0) div();
        }
        public void exp(int ex,T[] lex)
        {
            
            int _e;
            _e = ((ex < 0) ? -ex : ex);
            if (_e == 0) {set1(); return;}
            if (_e > 1) {
                T t = new T(); copy(ref t); 
                int i0 = _e-1; while (i0 > 0) 
                {
                    if ((i0 & 1) != 0) mul(t);





                    i0 >>= 1;
                }
            }
            if (ex < 0) div();
        }
    }

    class Num : Power<Num>, IPower, IComparable
    {
        public BigInteger up, down;
        public int sign;
        public BigInteger get_sup() { return up * sign; }
        public Num()
        {
            init(0, 0);
        }
        public Num(Num n)
        {
            set(n);
        }
        public Num(Num n, int s)
        {
            set(n,s);
        }
        static public Num add(Num a0, Num a1, int s)
        {
            Num r = new Num(a0); r.add(a1,s);
            return r;
        }
        static public Num add(Num a0, Num a1)
        {
            Num r = new Num(a0); r.add(a1,1);
            return r;
        }
        static public Num sub(Num s0, Num s1)
        {
            Num r = new Num(s0); r.add(s1,-1);
            return r;
        }
        static public Num neg(Num m0)
        {
            return new Num(-m0.sign,m0.up,m0.down);
        }
        static public Num _div(Num d0)
        {
            return new Num(d0.sign,d0.down,d0.up);
        }
        static public Num mul(Num m0, Num m1)
        {
            return new Num(m0.sign * m1.sign,m0.up * m1.up,m0.down * m1.down);
        }
        static public Num div(Num d0, Num d1)
        {
            return new Num(d0.sign * d1.sign,d0.up * d1.down,d0.down * d1.up);
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
            if (n0.sign != n1.sign) return IDS.n0;
            if (n0.up*n1.down > n1.up*n0.down) return n1; else return n0;
            
        }
        public Num(BigInteger u)
        {
            set(u);
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
        public void set(Num n,int s)
        {
            sign = n.sign * s;
            up = BigInteger.Abs(n.up);
            down = BigInteger.Abs(n.down);
        }
        static char[] pnt = {'.'};
        public void set(string s)
        {
            if (s.Length > IDS.e10.Length) IDS.sys.error("num: too long");
            string[] ss = s.Split(Num.pnt);
            if (ss.Length > 2) IDS.sys.error("num: parse");
            if (ss.Length == 2) {
                ss[0] += ss[1];
                down = BigInteger.Abs(IDS.e10[ss[1].Length].up);
            } else down = 1;
            BigInteger.TryParse(ss[0], out up);
            sign = (up > 0 ? 1 : 0);
        }
        private Num(int _s, BigInteger _u, BigInteger _d)
        {
            sign = _s; up = _u; down = _d;
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
            init(0,1);
        }
        public override void set1()
        {
            init(1, 1);
        }
        void init(int a, int b)
        {
            up = BigInteger.Abs(a);
            down = BigInteger.Abs(b);
            sign = (a < 0 ? -1 : (a > 0 ? 1: 0));
        }
        public void neg()
        {
            sign *= -1;
        }
        public bool great(Num a)
        {
            if (sign == a.sign)
            return (a.up*down*sign > up*a.down*sign);
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
        public bool equ(Num a)
        {
            return ((sign == a.sign) && (up == a.up) && (down == a.down));
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
            BigInteger a,_u,_d;
            if ((up > 1) && (down > 1)) {
                bool t = false; _u = BigInteger.Abs(up); _d = BigInteger.Abs(down);
                do {
                    a = BigInteger.GreatestCommonDivisor(_u, _d);
                    if (a > 1) t = true; else break;
                    _u = BigInteger.Divide(_u, a);
                    _d = BigInteger.Divide(_d, a);
                } while (true);
                if (t) {
                    if ((_d == 1) && (_u < IDS.znums)) return IDS.nums[IDS.znums + (int)(_u)*sign];
                    else return new Num(sign,_u,_d);
                }
            } else if (down == 0) IDS.sys.error("div0");
            if ((down == 1) && (up < IDS.znums)) return IDS.nums[IDS.znums + (int)(up)*sign];
            else return this;
        }
        public void simple_this()
        {
            BigInteger a;
            if ((up > 1) && (down > 1)) {
                do {
                    a = BigInteger.GreatestCommonDivisor(up, down);
                    if (a < 2) return;
                    up = BigInteger.Divide(up, a);
                    down = BigInteger.Divide(down, a);
                } while (true);
            }
        }
        public void common(Num n)
        {
            if (sign != n.sign) set0(); else {
                if (up*n.down > n.up*down) set(n);
            }
        }
        public void extract(Num n)
        {
            sign = ((sign < 0) && (n.sign < 0) ? -1 : 1);
            up = BigInteger.GreatestCommonDivisor(up, n.up);
            down = BigInteger.GreatestCommonDivisor(down, n.down);
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
            add(new Num(a),1);
        }
        public void add(Num a, int s)
        { // 1/6 + 1/15 : (3) : (15/3 + 6/3) / (6 * (15/3))
            if (sign == 0) set(a,s); else {
            BigInteger c = BigInteger.GreatestCommonDivisor(down, a.down);
            BigInteger d = a.down / c;
            if (sign * (a.sign * s) < 0)
            {
                up = d * up - (down / c) * a.up;
            } else {
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
            int lu, ld; int _l, l = IDS.prec2;
            if ((up < 2) || (down < 2)) return;
            lu = Num._l2(up); ld = Num._l2(down);
            if (lu > ld) lu = ld;
            _l = lu - l; if (_l > 11)
            {
                BigInteger _d, _au, _ad, _bu, _bd, _cu, _cd; 
                _d = IDS.e2[_l].up;
                _au = up / _d; _bu = up % _d; _cu = _bu / _au;
                _ad = down / _d; _bd = down % _d; _cd = _bd / _ad;
                _d += (_cu + _cd) / 2;
                up /= _d; down /= _d;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _l2(BigInteger n) {
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
            BigInteger _c = BigInteger.GreatestCommonDivisor(fr.down,_d);
            _d /= _c;
            to.up  = _d * fr.up + (fr.down / _c) * up * fr.down;
            to.down =  fr.down * _d;
            if (to.up.IsEven) to.up >>= 1; else to.down <<=1;
            to.prec_this();
        }
        Num sq_exp(Num sq) //up > down
        { 
            int i0,i1 = IDS.sqr_exp_bits;
            if (sq.down.IsEven && (sign < 0)) IDS.sys.error("exp: neg sqr");
            Num res = new Num(1), _sq = new Num(sq), tmp = new Num(1), _t = new Num(this); // (5/11-1/2)*2 = ((5*2-11)/(11*2))*2
            BigInteger _ud = up/down;
            if (_ud > 3) {
                int l2 = Num._l2(_ud);
                while (l2 > 1) {
                    _t._sq2_n(tmp,IDS.sqr2[l2]); i0 = IDS.sqr_steps; while(i0-- > 0) _t._sq2_n(tmp,tmp);
                    _sq.up <<= 1; if (_sq.up >= _sq.down) {
                        _sq.up -= _sq.down;
                        res.mul(tmp);
                    }
                    if (_sq.up.IsZero || (i1 < 0)) return res.simple();
                    _t.up = tmp.up; _t.down = tmp.down; l2 >>= 1; i1--;
                }
            }
            while ((! _sq.up.IsZero) && (i1 > 0)) {
                tmp.up = _t.up + _t.down; if (tmp.up.IsEven) {tmp.up >>= 1; tmp.down = BigInteger.Abs(_t.down);} else tmp.down = _t.down << 1;
                i0 = IDS.sqr_steps; while(i0-- > 0) _t._sq2_n(tmp,tmp);
                    _sq.up <<= 1; if (_sq.up >= _sq.down) {
                        _sq.up -= _sq.down;
                        res.mul(tmp);
                    }
                    _t.up = tmp.up; _t.down = tmp.down; i1--;
            }
            return res.simple();
        }
        public void exp(Num ex)
        {
            int se; bool dv = (down > up);
            if ((sign == 0) || isint(1) || ex.isint(1)) return;
            if (ex.up == 0) {set1(); return; }
            Num _e = new Num(ex), r0 = (dv ? new Num(sign,down,up) : new Num(this));
            se = _e.sign; _e.sign = 1;
            BigInteger _ei = _e.toint();
            if (_ei > IDS.exp_max) IDS.sys.error("exp too large");
            if (_ei > 0) {
                _e.up -= _ei*_e.down;
                Num r1 = new Num(r0);
                r0.exp((int)(_ei));
                r0.mul(r1.sq_exp(_e));
            } else r0 = r0.sq_exp(_e);
            sign = r0.sign; 
            if (dv ^ (se < 0)) {up = r0.down; down = r0.up;} else {up = r0.up; down = r0.down;}
        }
        public void ln()
        {
            if (sign < 1) IDS.sys.error("ln: not pos");
            bool dv = down > up;
            if (dv) div();
            Num r = null;
            if (IDS.ln.ContainsKey(this)) set(IDS.ln[this]); else {
                int l2 = Num._l2(toint());
                Num t = new Num(this);
                t.down *= IDS.e2[l2].up;
                t.simple_this(); t.prec_this();
                //x = (1+y)/(1-y); 1+y = x-xy; 1-x = -y-xy; x-1 = y(1+x); y = (x-1)/(x+1)
                Num y = Num.div(Num.sub(t,IDS.nums[IDS.znums + 1]),Num.add(t,IDS.nums[IDS.znums + 1])).simple();
                r = Num.mul(y,IDS.nums[IDS.znums + 2]);
                Num z = new Num(IDS.ln_prec);
                Num l = new Num(z);
                y = Num.mul(y,y);
                while (z.down > 1) {
                    z.down -= 2;
                    l = Num.add(z,Num.mul(y,l));
                }
                r.mul(l); r.simple_this();
                r.add(Num.mul(IDS.n_ln2,IDS.nums[IDS.znums + l2]));
                IDS.ln.Add(new Num(this), new Num(r)); set(r);
            }
            if (dv) sign = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        BigInteger _div(BigInteger d0, BigInteger d1) {
            BigInteger t;
            BigInteger r = BigInteger.DivRem(d0,d1,out t);
            if (r > 1000000000) return new BigInteger(1000000000);
            if (t*2 > d0) r++;
            return r;
        }
//6: +0.0000000000000000000000000000000000000000000000000000013631000934856663598194709551475972339538149682
//6: +0.0000000000000000000000000000000000000000000000000000070006026538925265359943634345869258627656933747
//6: +0.0000000000000000000000000000000000000000000000000000995320760599319535732067627950329184983090196596
//n1 = [(x > n0) n0+n0/i; (n0 > x > n0/2) n0-n0/i; (n0/2 > x) n0/i]
        public Num uf(Num n)
        {
            BigInteger nu,xu,t0,i;
            xu = up*n.down; nu = n.up*down;
            if (xu > nu) {
//n/(x-n) = i
//(nu/nd)/(nd*xu/xd*nd-xd*nu/nd*xd)
//(xd*nd*nu/nd)/(nd*xu-xd*nu)
//(xd*nu)/(nd*xu-xd*nu)
                t0 = xu - nu; i = _div(nu,t0);
                return new Num(1,n.up*i + n.up, n.down*i);
            } else if (xu*2 > nu) {
//n/(n-x)
                t0 = nu - xu; i = _div(nu,t0);
                return new Num(1,n.up*i - n.up, n.down*i);
            } else {
//n/x
                i = _div(nu,xu);
                return new Num(1,n.up, n.down*i);
            }
        }

        public BigInteger ufu(Num n, Num s)
        {
//d = n-x; i = s/d + 1; s /= i; n1 = n-s;
            BigInteger dnx, unx,t0,i;
            dnx = down*n.down; unx = n.up*down - up*n.down;
            t0 = s.down*unx; i = s.up*dnx/t0 + 1;
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
            return (double)(sign * up) / (double)(down);
        }
        public string print(string plus, string minus, string non_one)
        {
            string s0; 
            s0 = ((sign < 0) ? minus : plus);
            if ((non_one == "") || (! isint(1))) {
                s0 += up.ToString().Trim();
                if (down > 1) s0 += "/" + down.ToString().Trim();
                s0 +=  non_one;
            }
            return s0;
        }
        public string toname(bool group) {
            if (isint(0)) return "0";
            if (isint(1) && group) return "";
            return (sign < 0 ? "_" : "") + up.ToString().Trim() + (down > 1 ? ("_" + down.ToString().Trim()) : "");
        }
        public int CompareTo(object obj) {
            if (obj == null) return 1;
            Num k = obj as Num;
            if (sign != k.sign) return (sign < k.sign ? -1: +1);
            BigInteger u0 = up * k.down, u1 = k.up * down;
            if (u0 == u1) return 0;
            return (u0 < u1 ? -1 : 1);
        }
    }
    class Complex: Power<Complex>, IPower
    {
        Num k, i;
        public Complex(Num _k, Num _i) 
        {
            k = new Num(_k);
            i = new Num(_i);
        }
        public Complex(Num _k)
        {
            k = new Num(_k);
            i = new Num(0);
        }
        public Complex()
        {
            k = new Num(0);
            i = new Num(0);
        }
        public Complex(Complex _k)
        {
            k = new Num(_k.k);
            i = new Num(_k.i);
        }
        public override void set0()
        {
            k.set0(); i.set0();
        }
        public override void set1()
        {
            k.set1(); i.set0();
        }
        public void simple()
        {
            k = k.simple(); i = i.simple();
        }
        public override void set(Complex a)
        {
            k.set(a.k); i.set(a.i);
        }
        public override void copy(ref Complex a)
        {
            k.copy(ref a.k); i.copy(ref a.i);
        }
        public bool equ(Complex a)
        {
            return k.equ(a.k) && i.equ(a.i);
        }
        public void neg()
        {
            k.neg(); i.neg();
        }
        public void add(Complex a)
        {
            k.add(a.k); i.add(a.i);
        }
        public override void div()
        {
            Num x0, x1;
            x0 = new Num(k); x0.exp2();
            x1 = new Num(i); x1.exp2();
            x0.add(x1); x0.div();
            k.mul(x0); i.mul(x0);
        }
        public override void mul(Complex a)
        {
            Num x,_k;
            _k = new Num(k); _k.mul(a.k);
            x = new Num(i); x.mul(a.i);
            x.neg(); _k.add(x);
            i.mul(a.k); k.mul(a.i);
            i.add(k); k.set(_k);
        }
        public new void exp2()
        {
            //k^2-i^2 : 2*k*i
            Num k2,i2;
            k2 = new Num(k); k2.exp2();
            i2 = new Num(i); i2.exp2();
            i2.neg(); k2.add(i2);
            i.mul(k); i.mul(2);
            k.set(k2);
        }
        public string print(string plus, string minus, string non_one)
        {
            if (i.isint(0)) return k.print(plus,minus,non_one);
            return "[" + k.print("","-","") + "," + i.print("","-","") + "]" + non_one;
        }
    }

    class One: IComparable
    {
        public SortedDictionary<Func,Func> exps;
        public One()
        {
            exps = new SortedDictionary<Func,Func>();
        }
        public One(Func f)
        {
            exps = new SortedDictionary<Func,Func>();
            exps.Add(new Func(f),new Func(IDS.n1));
        }
        public One(Func fe, Func fp)
        {
            exps = new SortedDictionary<Func,Func>();
            exps.Add(new Func(fe),new Func(fp));
        }
        public One(One o)
        {
            exps = new SortedDictionary<Func,Func>();
            set(o);
        }
        public void set(One o)
        {
            exps.Clear();
            foreach(KeyValuePair<Func,Func> m in o.exps) exps.Add(new Func(m.Key),new Func(m.Value));
        }

        public int CompareTo(object obj) {
            if (obj == null) return 1;
            One o = obj as One;
            SortedDictionary<Func,Func>.Enumerator e0 = exps.GetEnumerator();
            SortedDictionary<Func,Func>.Enumerator  e1 = o.exps.GetEnumerator();
            bool n0,n1;
            int rk,rv;
            while (true) {
                do n0 = e0.MoveNext(); while (n0 && (e0.Current.Value.isconst(0)));
                do n1 = e1.MoveNext(); while (n1 && (e1.Current.Value.isconst(0)));
                if (n0 ^ n1) return (n0 ? e0.Current.Value.CompareTo(Program.root.fzero) : Program.root.fzero.CompareTo(e1.Current.Value));
                else {
                    if (! n0) return 0;
                    if ((rk = e0.Current.Key.CompareTo(e1.Current.Key)) == 0) {
                        if ((rv = e0.Current.Value.CompareTo(e1.Current.Value)) != 0) return rv;
                    } else {
                        if (rk < 0) return e0.Current.Value.CompareTo(Program.root.fzero);
                        else return Program.root.fzero.CompareTo(e1.Current.Value); 
                    }
                }
            }
        }

        public void addto(Func val,Func exp) //no new
        {
            if (exps.ContainsKey(val)) exps[val].add(exp,1); else exps.Add(val,exp);
        }
        public void addto(Func val)
        {
            if (exps.ContainsKey(val)) exps[val].add(new Func(IDS.n1),1); else exps.Add(new Func(val),new Func(IDS.n1));
        }
        public void mul(One o)
        {
            foreach(KeyValuePair<Func,Func> m in o.exps) {
                if (exps.ContainsKey(m.Key)) {
                    exps[m.Key].add(m.Value,1);
                    if (exps[m.Key].isconst(0)) exps.Remove(m.Key);
                }
                else exps.Add(new Func(m.Key), new Func(m.Value));
            }
        }
        public void div()
        {
            foreach(KeyValuePair<Func,Func> m in exps) m.Value.neg();
        }
        public void exp(int e)
        {
            exp(IDS.nums[IDS.znums+e]);
        }
        public void exp(Num e)
        {
            foreach(KeyValuePair<Func,Func> f in exps) f.Value.mul(e);
        }
        public void exp(Func e)
        {
            foreach(KeyValuePair<Func,Func> f in exps) f.Value.mul(e);
        }
        public Func get_Func(Num n) {
            Func r = null;
                if (exps.Count == 0) r = new Func(n);
                if ((exps.Count == 1) && n.isint(1) && exps.ElementAt(0).Value.isconst(1))
                    r = exps.ElementAt(0).Key;
            return r;
        }

        public void extract(One from) //common multi
        {
            SortedDictionary<Func,Func> r = new SortedDictionary<Func,Func>();
            foreach(KeyValuePair<Func,Func> m in exps) 
                if (from.exps.ContainsKey(m.Key)) {
                    m.Value.common(from.exps[m.Key]);
                    r.Add(m.Key,m.Value);
                }
            exps = r;
        }
        public Many2 expand(Num n)
        {
            Many2 r = null,t; One o = new One();
            foreach(KeyValuePair<Func,Func> m in exps) if ((m.Key.type == Func.t_many2) && (m.Value.type_pow() == 0))
            {
                if (r == null) r = new Many2(1);
                t = new Many2((Many2)(m.Key.data)); t.exp(m.Value); r.mul(t);
            } else o.addto(new Func(m.Key),new Func(m.Value));
            if (r != null) r.mul(o,n);
            return r;
        }
        public bool expand_p0(Func val, Exps_f exu, Exps_f exd)
        {
            bool rt = false;
            foreach(KeyValuePair<Func,Func> m in exps) if (m.Value.CompareTo(val) == 0)
            {
                rt = true; m.Value.type = Func.t_many2; m.Value.data = new Many2(new Many(exu.mvar), new Many(exd.mvar));
            }
            return rt;
        }
        public bool expand_2(Func val, Exps_f exu, Exps_f exd)
        {
            bool fv = false, fk = false;
            foreach(KeyValuePair<Func,Func> m in exps) 
            {
                fk = m.Key.expand(val, exu, exd) || fk;
                fv = m.Value.expand(val, exu, exd) || fv;
            }
            if (fk) {
                One r = new One();
                foreach(KeyValuePair<Func,Func> m in exps) 
                    if ((! m.Value.isconst(0)) && (! m.Key.isconst(1))) r.addto(m.Key,m.Value);
                exps = r.exps;
            }
            return fv || fk;
        }

        public void replace(Vals v, Func f) {
            foreach(KeyValuePair<Func,Func> ff in exps) {
                ff.Key.replace(v,f); ff.Value.replace(v,f);
            }
        }

        public Num simple()
        {
            Num nt, rt = new Num(1); Func p,e;
            One r = new One();
            foreach(KeyValuePair<Func,Func> ff in exps) {
                ff.Value.simple();
                if (! ff.Value.isconst(0)) {
                    ff.Key.simple();
                    if ((ff.Key.type == Func.t_num) && (ff.Value.type_pow() == 0)) rt.mul(Num.exp((Num)(ff.Key.data),(Num)(ff.Value.data)));
                    else {
                        if ((ff.Key.type == Func.t_val) && (((Vals)(ff.Key.data)).var == Program.root.v_e) && (ff.Value.type == Func.t_ln))
                        {p = new Func((Many2)(ff.Value.data)); e = new Func(IDS.n1);}
                        else {p = ff.Key; e = ff.Value;}
                        if ((p.type == Func.t_many2) && (((Many2)(p.data)).type_exp() < 2)) {
                            ((Many2)(p.data)).up.data.ElementAt(0).Key.exp(e);
                            ((Many2)(p.data)).down.data.ElementAt(0).Key.exp(e);
                            ((Many2)(p.data)).down.data.ElementAt(0).Key.div();
                            nt = Num.mul(((Many2)(p.data)).up.data.ElementAt(0).Value,Num._div(((Many2)(p.data)).down.data.ElementAt(0).Value));
                            r.mul(((Many2)(p.data)).up.data.ElementAt(0).Key);
                            r.mul(((Many2)(p.data)).down.data.ElementAt(0).Key);
                            if (e.type_pow() < 1) rt.mul(Num.exp(nt,(Num)(e.data))); 
                            else r.addto(new Func(nt),e);
                        } else r.addto(p, e);
                    }
                }
            }
            exps = r.exps;
            return rt;
        }
        public void deeper(int deep) {
            SortedDictionary<Func,Func> r = new SortedDictionary<Func,Func>();
            foreach(KeyValuePair<Func,Func> ff in exps) r.Add(Func.deeper(ff.Key,deep),Func.deeper(ff.Value,deep));
            exps = r;
        }
        public Num calc() {
            Num rt = new Num(1);
            foreach(KeyValuePair<Func,Func> f in exps) rt.mul(f.Key.calc(f.Value.calc()));
            return rt;
        }
        public void findvals(One o)
        {
            foreach(KeyValuePair<Func,Func> f in exps) {f.Key.findvals(o); f.Value.findvals(o);}
        }
        public Many diff_down(Vals at)
        {
            List<Func> d = new List<Func>(), p = new List<Func>(), e = new List<Func>();
            One f_d0 = new One();
            Func t;
            foreach(KeyValuePair<Func,Func> ff in exps) {
                t = ff.Key.diff_down(ff.Value,at);
                if (t.isconst(0)) f_d0.addto(ff.Key,ff.Value); else {
                    p.Add(ff.Key); e.Add(ff.Value); d.Add(t);
                }
            }
            Many r = new Many(); One a;
            int i1, i0 = 0; while (i0 < d.Count) {
                a = new One(f_d0);
                i1 = 0; while (i1 < d.Count) {
                    if (i0 == i1) {
                        a.addto(d[i1]);
                    } else {
                        a.addto(new Func(p[i1]),new Func(e[i1]));
                    }
                    i1++;
                }
                r.add(a,1);
                i0++;
            }
            r.simple();
            return r;
        }

    }

    class Many: Power<Many>, IPower, IComparable
    {
        public SortedDictionary<One,Num> data;
        public Many()
        {
            data = new SortedDictionary<One,Num>();
        }
        public Many(One o,Num n)
        {
            data = new SortedDictionary<One,Num>();
            data.Add(o,n);
        }
        public Many(Func f)
        {
            data = new SortedDictionary<One,Num>();
            data.Add(new One(f),IDS.n1);
        }
        public Many(One o)
        {
            data = new SortedDictionary<One,Num>();
            data.Add(o,IDS.n1);
        }
        public Many(Num n)
        {
            data = new SortedDictionary<One,Num>();
            data.Add(new One(),n);
        }
        public Many(Many m)
        {
            set(m);
        }
        public static SortedDictionary<One,Num> copy(SortedDictionary<One,Num> c)
        {
            SortedDictionary<One,Num> r = new SortedDictionary<One,Num>();
            foreach (KeyValuePair<One,Num> o in c) r.Add(new One(o.Key),o.Value);
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
            data.Add(new One(), IDS.n0);
        }
        public override void set1()
        {
            data.Clear();
            data.Add(new One(), IDS.n1);
        }
        public override void div()
        {
            if (data.Count != 1) IDS.sys.error("cant divide many");
            data[data.ElementAt(0).Key] = Num._div(data.ElementAt(0).Value); 
            data.ElementAt(0).Key.div(); 
        }

        public int sign() {
            bool p = false, m = false;
            foreach (KeyValuePair<One,Num> o in data) {
                if (o.Value.sign > 0) {
                    p = true; if (m) return 0;
                }
                if (o.Value.sign < 0) {
                    m = true; if (p) return 0;
                }
            }
            if (p) return 1;
            if (m) return -1;
            return 0;
        }

        public int CompareTo(object obj) {
            if (obj == null) {int s = sign(); return (s != 0 ? s : data.ElementAt(0).Value.sign); }
            Many m = obj as Many;
            SortedDictionary<One,Num>.Enumerator o0 = data.GetEnumerator();
            SortedDictionary<One,Num>.Enumerator o1 = m.data.GetEnumerator();
            bool n0,n1;
            int r;
            while (true) {
                do n0 = o0.MoveNext(); while (n0 && (o0.Current.Value.sign == 0));
                do n1 = o1.MoveNext(); while (n1 && (o1.Current.Value.sign == 0));
                if (n0 ^ n1) return (n0 ? o0.Current.Value.CompareTo(IDS.n0) : IDS.n0.CompareTo(o1.Current.Value));
                else {
                    if (! n0) return 0;
                    if ((r = o0.Current.Key.CompareTo(o1.Current.Key)) == 0) {
                        if ((r = o0.Current.Value.CompareTo(o1.Current.Value)) != 0) return r;
                    } else {
                        if (r > 0) return o0.Current.Value.CompareTo(IDS.n0);
                        else return IDS.n0.CompareTo(o1.Current.Value);
                    }
                }
            }
        }
        public Func get_Func()
        {
            Func r = null;
            if (data.Count == 0) r = new Func(new Num(0));
            if (data.Count == 1) r = data.ElementAt(0).Key.get_Func(data.ElementAt(0).Value);
            return r;
        }
        public Num get_Num()
        {
            Num r = null;
            if (data.Count == 0) r = new Num(0);
            if ((data.Count == 1) && (data.ElementAt(0).Key.CompareTo(Program.root.ozero) == 0))
                r = new Num(data.ElementAt(0).Value);
            return r;
        }
        public Num get_int()
        {
            Num r = new Num(1);
            BigInteger b;
            foreach (KeyValuePair<One,Num> on in data) {
                b = BigInteger.GreatestCommonDivisor(r.up,on.Value.down);
                r.up *= on.Value.down/b;
            }
            return r;
        }
        public bool isint(int i)
        {
            Num n = get_Num();
            if (n != null) return n.isint(i);
            return false;
        }

        public void add(One o, Num n, int s) {
            if (data.ContainsKey(o)) {
                data[o] = Num.add(data[o],n,s);
            } else data.Add(new One(o),new Num(n,s));
        }
        public void add(One o, int s) {
            add(o,IDS.n1,s);
        }
        public void add(Many from, int s)
        {
            foreach (KeyValuePair<One,Num> o in from.data) add(o.Key,o.Value,s);
        }

        public void mul(One o, Num n)
        {
            SortedDictionary<One,Num> r = new SortedDictionary<One,Num>();
            One _o; Num _n;
            foreach (KeyValuePair<One,Num> m0 in data) {
                _o = new One(m0.Key);
                _o.mul(o);
                _n = Num.mul(m0.Value,n);
                if (r.ContainsKey(_o)) r[_o].add(_n); //in r any Num is new
                else r.Add(_o,_n);
            }
            data = r;
        }
        public override void mul(Many _m)
        {
            SortedDictionary<One,Num> r = new SortedDictionary<One,Num>();
            One o; Num n;
            foreach (KeyValuePair<One,Num> m0 in data) {
                foreach (KeyValuePair<One,Num> m1 in _m.data) {
                    o = new One(m0.Key);
                    o.mul(m1.Key);
                    n = Num.mul(m0.Value,m1.Value);
                    if (r.ContainsKey(o)) r[o].add(n,1); //in r any Num is new
                    else r.Add(o, n);
                }
            }
            data = r;
        }
        public void mul(Num n)
        {
            for (int _i = 0; _i < data.Count; _i++) data[data.ElementAt(_i).Key] = Num.mul(data.ElementAt(_i).Value,n).simple();
        }
        public void mul_simple(Num n)
        {
            for (int _i = 0; _i < data.Count; _i++) data[data.ElementAt(_i).Key] = Num.mul(data.ElementAt(_i).Value,n).simple();
        }

        public Many expand()
        {
            Many rt = new Many(new Num(1)), r = new Many(); Many2 t;
            foreach (KeyValuePair<One,Num> o in data) {
                t = o.Key.expand(o.Value);
                if (t == null) r.add(o.Key,o.Value,1); else {
                    r.mul(t.down); t.up.mul(rt); r.add(t.up,1); rt.mul(t.down);
                }
            }
            data = r.data;
            return rt;
        }

        public void expand(Many from, One o, Num n, Func val)
        {
            One to = new One(o); to.exps[val].set0();
            One _o; Num _n;
            foreach (KeyValuePair<One,Num> on in from.data) {
                _o = new One(to); _o.mul(on.Key);
                _n = Num.mul(n,on.Value);
                add(_o,_n,1);
            }
        }
        public bool expand_p0(Func val, Exps_f exu, Exps_f exd)
        {
            bool rt = false;
            foreach (KeyValuePair<One,Num> o in data) rt = o.Key.expand_p0(val, exu, exd) || rt;
            return rt;
        }
        public bool expand_2(Func val, Exps_f exu, Exps_f exd)
        {
            bool rt = false;
            foreach (KeyValuePair<One,Num> o in data) rt = o.Key.expand_2(val,exu,exd) || rt;
            return rt;
        }
        //e0,e2,p0,p2
        public bool expand_e0(Func val, Exps_f exu, Exps_f exd)
        {
            bool rt = false;
            SortedDictionary<Func,Many> ml = new SortedDictionary<Func,Many>();
            Many res = new Many();
            exu.add(this,val,+1); exu.calc();
            exd.add(this,val,-1); exd.calc();
            int type = (exu.type > exd.type ? exu.type : exd.type);
            Func eu, ed, e;
            foreach (KeyValuePair<One,Num> o in data) 
            {
//[+1]/[-1]
//[-up] *= [+1]^min*[-1]^max 
//[+1]^(up + (-min))*[-1]^(max - up)
                if (o.Key.exps.ContainsKey(val)) {
                    if (o.Key.exps[val].type_pow() + type < 3) {
                        e = o.Key.exps[val];
                        eu = (exu.type > 1 ? new Func(Num.add(exu.min,(Num)(e.data))) : e);
                        ed = (exd.type > 1 ? new Func(Num.sub(exd.max,(Num)(e.data))) : e);
                        if (exu.data.ContainsKey(eu) && exd.data.ContainsKey(ed)) {
                            if (! ml.ContainsKey(e)) {
                                ml.Add(new Func(e),new Many(exu.data[eu]));
                                ml[e].mul(exd.data[ed]);
                            }
                            res.expand(ml[e],o.Key,o.Value,val);
                        } else res.add(o.Key,o.Value,1);
                    } else res.add(o.Key,o.Value,1);
                } else res.add(o.Key,o.Value,1);
            }
            data = res.data;
            return rt;
        }
        public void replace(Vals v, Func f) {
            foreach (KeyValuePair<One,Num> o in data) o.Key.replace(v, f);
        }

        public int type_exp() 
        {
            if (data.Count == 1) {
                if (data.ElementAt(0).Value.isint(1)) return 0; else return 1;
            } else return 2;
        }
        public void exp(Func e)
        {
            int te = type_exp(),tp = e.type_pow();
            if (te + tp > 2) IDS.sys.error("exp: cant Many to many");
            if (te > 1) exp((int)(((Num)(e.data)).up));
            else {
                One to = data.ElementAt(0).Key;
                if (tp < 2) {
                    data[to] = Num.exp(data[to],(Num)(e.data));
                    to.exp((Num)(e.data));
                } else to.exp(e);
            }
        }

        public new void exp(int e)
        {
            if (data.Count > 1) base.exp(e); else if (data.Count == 1) {
                One to = data.ElementAt(0).Key;
                data[to] = Num.exp(data[to],e);
                to.exp(e);
            }
        }
        public KeyValuePair<One,Num> extract()
        {
            if (data.Count == 0) return new KeyValuePair<One,Num>(new One(), new Num(1)); else 
            {
                One ro = null; Num rn = null;
                foreach (KeyValuePair<One,Num> m in data) {
                    if (ro == null) {
                        ro = new One(m.Key); rn = new Num(m.Value);
                    } else {
                        ro.extract(m.Key);
                        rn.extract(m.Value);
                        if (ro.exps.Count < 1) new KeyValuePair<One,Num>(ro, new Num(0));
                        if (rn.sign == 0) new KeyValuePair<One,Num>(new One(), rn);
                    }
                }
                return new KeyValuePair<One,Num>(ro, rn);
            }
        }

        public void neg() {
            for (int i = 0; i < data.Count; i++) data[data.ElementAt(i).Key] = Num.neg(data.ElementAt(i).Value);
        }

        public void deeper(int deep) {
            SortedDictionary<One,Num> r = new SortedDictionary<One,Num>();
            One o;
            foreach (KeyValuePair<One,Num> m in data)
            {
                o = new One(m.Key); o.deeper(deep);
                r.Add(o,m.Value); //deeper dont remove val from one
            }
            data = r;
        }

        public void add_toexp(Func val, Func _e) 
        {
            SortedDictionary<One,Num> r = new SortedDictionary<One,Num>();
            One o;
            foreach (KeyValuePair<One,Num> m in data)
            {
                o = new One(m.Key); o.addto(val,_e);
                if (r.ContainsKey(o)) r[o].add(m.Value); //in r any Num is new
                else r.Add(o, new Num(m.Value));
            }
            data = r;
        }

        public Num find_minexp(Func val) //_val^(-x) -> /_val^(x)
        {
            Num _min = new Num(0), t;
            foreach (KeyValuePair<One,Num> o in data)
                if (o.Key.exps.ContainsKey(val)) {
                    t = o.Key.exps[val].get_num_part();
                    if (t.CompareTo(_min) < 0) _min.set(t);
                }
            return _min;
        }

        public void common(Many m)
        {
            bool ch = false; KeyValuePair<One,Num> on; Num n;
            for (int i = 0; i < data.Count; i++) {
                on = data.ElementAt(i);
                if (m.data.ContainsKey(on.Key)) {
                    n = Num.common(on.Value,m.data[on.Key]);
                    if (n.sign == 0) ch = true;
                    data[on.Key] = n;
                } else {ch = true; data[on.Key] = IDS.n0;}
            }
            if (ch) {
                SortedDictionary<One,Num> r = new SortedDictionary<One,Num>();
                foreach (KeyValuePair<One,Num> _on in data) if (_on.Value.sign != 0) r.Add(_on.Key,_on.Value);
                data = r;
            }
        }
/* ?????? common ?????
        public void extract(Many m)
        {
            SortedDictionary<One,Num> r = new SortedDictionary<One,Num>();
            foreach (KeyValuePair<One,Num> d in data)
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
            One k; Num v;
            SortedDictionary<One,Num> r = new SortedDictionary<One,Num>();
            foreach (KeyValuePair<One,Num> d in data)
            {
                if (d.Value.up != 0) {
                    v = new Num(d.Value); k = new One(d.Key); v.mul(k.simple());
                    if (r.ContainsKey(k)) r[k].add(v,1); else r.Add(k,v.simple());
                }
            }
            data = r;
        }
        public Num calc() {
            Num rt = new Num(0);
            foreach(KeyValuePair<One,Num> o in data) rt.add(Num.mul(o.Key.calc(),o.Value),1);
            return rt;
        }
        public void findvals(One o)
        {
            foreach(KeyValuePair<One,Num> on in data) on.Key.findvals(o);
        }

        public Many diff_down(Vals at)
        {
            Many m,r = new Many(IDS.n0);
            foreach(KeyValuePair<One,Num> on in data) {
                m = on.Key.diff_down(at); m.mul(on.Value);
                r.add(m,1);
            }
            return r;
        }
    }

    class Many2: Power<Many2>, IPower, IComparable
    {
        public Many up,down;
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
            down.data.Add(new One(), IDS.n1);
        }
        public Many2(One u) //no new 
        {
            up = new Many(u); 
            down = new Many();
            down.data.Add(new One(), IDS.n1);
        }
        public Many2(Func u) //no new 
        {
            up = new Many(new One(u)); 
            down = new Many();
            down.data.Add(new One(), IDS.n1);
        }
        public Many2(int n)
        {
            up = new Many(); down = new Many();
            up.data.Add(new One(), IDS.nums[IDS.znums+n]);
            down.data.Add(new One(), IDS.n1);
        }
        public Many2(Num n)
        {
            up = new Many(); down = new Many();
            up.data.Add(new One(), n);
            down.data.Add(new One(), IDS.n1);
        }
        public Many2(One o, Num n)
        {
            up = new Many(); down = new Many();
            up.data.Add(new One(o), n);
            down.data.Add(new One(), IDS.n1);
        }

        public Many2(Many2 m)
        {
            up = new Many(m.up); down = new Many(m.down);
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
            up.data.Add(new One(), IDS.n0);
            down.data.Add(new One(), IDS.n1);
        }
        public override void set1()
        {
            up.data.Clear(); down.data.Clear();
            up.data.Add(new One(), IDS.n1);
            down.data.Add(new One(), IDS.n1);
        }
        public override void div()
        {
            Many t;
            if ((up.data.Count == 1) && (up.data.ElementAt(0).Value.sign == 0)) IDS.sys.error("div0");
            t = up; up = down; down = t;
        }

        public int sign() {
            return up.sign()*down.sign();
        }


        public int CompareTo(object obj) {
            if (obj == null) {int s = sign(); return (s != 0 ? s : down.data.ElementAt(0).Value.sign); }
            Many2 m = obj as Many2;
            int r = up.CompareTo(m.up);
            if (r != 0) return r;
            return -down.CompareTo(m.down);
        }

        public void add(Num n, int s) 
        {
            Many t = new Many(n); t.mul(down); up.add(t,s);
        }

        public void add(Many2 m, int s) 
        {
            if (down.CompareTo(m.down) == 0) up.add(m.up,s); else {
                  Many t = new Many(m.up);
                  t.mul(down); up.mul(m.down); up.add(t,s);
                  down.mul(m.down);
            }
        }

        public override void mul(Many2 m)
        {
            up.mul(m.up); down.mul(m.down);
        }
        public void mul(Num n)
        {
            up.mul(n); 
        }
        public void mul(One o, Num n)
        {
            up.mul(o,n);
        }

        public void revert(Func val) //_val^(-x) -> /_val^(x)
        {
            Num min = Num.min(up.find_minexp(val),down.find_minexp(val));
            if (min.sign == 0) return;
            min.neg(); Func f = new Func(min);
            up.add_toexp(val,f);
            down.add_toexp(val,f);
        }

        public bool expand(Func val, Exps_f exu, Exps_f exd)
        {
            bool rt0,rt02;
            rt02 = up.expand_2(val,exu,exd);
            rt02 = down.expand_2(val,exu,exd) || rt02;

            rt02 = up.expand_p0(val,exu,exd) || rt02;
            rt02 = down.expand_p0(val,exu,exd) || rt02;

            rt0 = up.expand_e0(val,exu,exd);
            Num minup = new Num(exu.min), maxdw = new Num(exd.max);
            rt0 = down.expand_e0(val,exu,exd) || rt0;
            if (rt0) {
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
        public void replace(Vals v, Func f) {
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
            up.exp(e); down.exp(e);
        }

        public new void exp(int e)
        {
            up.exp(e); down.exp(e);
        }

        public void neg() {
            up.neg();
        }

        public void deeper(int deep) {
            up.deeper(deep);
            down.deeper(deep);
        }
        public Func get_Func()
        {
            if (down.isint(1)) return up.get_Func(); else return null;
        }
        public Num get_Num()
        {
            Num u = up.get_Num();
            if (u == null) return null;
            Num d = down.get_Num();
            if (d == null) return null;
            d.div(); u.mul(d); return u;
        }
        public Num get_num_part() //up[num_one]/down(Num)
        {
            Num d = down.get_Num();
            if ((d == null) || (! up.data.ContainsKey(Program.root.ozero))) return new Num(0);
            d.div(); d.mul(up.data[Program.root.ozero]); return d;
        }
        public void common(Many2 m)
        {
            if (down.CompareTo(m.down) == 0) up.common(m.up); else {
                Many t = new Many(m.up);
                t.mul(down); up.mul(m.down); down.mul(m.down);
                up.common(t);                
            }
        }
        public void toint()
        {
            Num u = up.get_int();
            Num d = down.get_int();
            u.mul(d); up.mul_simple(u); down.mul_simple(u);
        }
        public Num simple()
        {
            up.simple();
            down.simple();
            if ((up.data.Count > 1) || (down.data.Count > 1)) {
                KeyValuePair<One,Num> f_up = up.extract(), f_down = down.extract();
                if (f_up.Key.exps.Count + f_down.Key.exps.Count > 0) {
                    One ou = new One(f_up.Key);
                    f_up.Key.div(); up.mul(f_up.Key,IDS.n1);
                    f_down.Key.div(); down.mul(f_down.Key,IDS.n1);
                    ou.mul(f_down.Key); ou.simple();
                    if (ou.exps.Count > 0) {
                        f_up.Value.div(); up.mul(f_up.Value);
                        f_down.Value.div(); down.mul(f_down.Value);
                        f_up.Value.div(); f_up.Value.mul(f_down.Value);
                        ou.addto(new Func(new Many2(up,down)),new Func(IDS.n1));
                        down = new Many(IDS.n1);
                        up = new Many(ou,f_up.Value);
                    }
                }
/*
            foreach(KeyValuePair<Func,Func> u in f_up.Key.exps) if (u.Value.sign() >= 0) u.Value.set0();
            f_up.Value.set(f_up.Key.simple());

            foreach(KeyValuePair<Func,Func> d in f_down.Key.exps) if (d.Value.sign() >= 0) d.Value.set0();
            f_down.Value.set(f_down.Key.simple());

            f_up.Key.div();
            up.mul(f_up.Key,f_up.Value);
            down.mul(f_up.Key,f_up.Value);
            f_down.Key.div();
            up.mul(f_down.Key,f_down.Value);
            down.mul(f_down.Key,f_down.Value);
*/
            } else {
                if (! down.isint(1)) {
                    down.div(); up.mul(down); down = new Many(IDS.n1);
                }
                Func t = up.get_Func();
                if ((t != null) && (t.type == Func.t_many2) && ((Many2)(t.data)).down.isint(1)) up = ((Many2)(t.data)).up;
                t = down.get_Func();
                if ((t != null) && (t.type == Func.t_many2) && ((Many2)(t.data)).down.isint(1)) down = ((Many2)(t.data)).up;
                Num nd = down.get_Num();
                if (nd == null) return null;
                Num nu = up.get_Num();
                if (nu != null) {
                    nd.div(); nu.mul(nd); return nu;
                }
            }
            return null;
        }
        public Num calc() {
            Num d = down.calc();
            if (d.sign == 0) IDS.sys.error("div0");
            d.div(); d.mul(up.calc()); return d.simple();
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
            fu.mul(down); fd.mul(up); fu.add(fd,-1); r.up = fu;
            r.down = new Many(down); r.down.mul(down);
            r.simple(); return r;
        }
    }
    class Exps_f 
    {
        public Num max, min;
        int deep;
        public int type; 
        //0 - 1*one, 1 - n*one, 2 - many
        //0,1 - sign, 2 - no sign
        public SortedDictionary<Func,Many> data;
        public Many mvar;
        Func e1;
        public Exps_f(Many m, int _deep){
            min = new Num(0); max = new Num(0);
            data = new SortedDictionary<Func,Many>();
            Func e0 = new Func(IDS.n0);
            data.Add(e0, new Many());
            data[e0].data.Add(new One(),new Num(1));
            deep = _deep;
            e1 = new Func(IDS.n1);
            mvar = new Many(m); if (deep > 0) mvar.deeper(deep);
            type = mvar.type_exp();
            data.Add(e1, mvar);
        }
        void add(Func e) {
            if (! data.ContainsKey(e)) data.Add(e,new Many());
        }

        public void add(Many m, Func val, int updown){
            Num t; 
            min.set0(); max.set0(); 
            Func te;
            if (type > 1) {
                foreach (KeyValuePair<One,Num> on in m.data) {
                    if (on.Key.exps.ContainsKey(val)) {
                        te = on.Key.exps[val];
                        if (te.type_pow() + type < 3) {
                            t = (Num)(te.data);
                            min.min(t); max.max(t);
                        }
                    }
                }
                min.neg();
                add(new Func(min)); add(new Func(max));
            }
            t = new Num(0);
/*
[+1]^(pow - min)*[-1]^(max - pow)
(a+b)/(c+d)
 -1, -2, +1
 (a+b)^2*(c+d)^1
 [+1]^(-1 - (-2))*[-1]^(+1 - (-1)) (+1) (+2)
 [+1]^(-2 - (-2))*[-1]^(+1 - (-2)) (0) (+3)
 [+1]^(+1 - (-2))*[-1]^(+1 - (+1)) (+3) (0)
*/
            foreach (KeyValuePair<One,Num> on in m.data) {
                if (on.Key.exps.ContainsKey(val)) {
                    te = on.Key.exps[val]; 
                    if (te.type_pow() + type < 3) {
                        if (type < 2) add(new Func(te)); else {
                            if (updown == +1) { //pow + (-min)
                                t.set((Num)(te.data)); t.add(min,+1);
                                add(new Func(t));
                            }
                            if (updown == -1) {//max - pow
                                t.set(max); t.add((Num)(te.data),-1);
                                add(new Func(t));
                            }
                        }
                    }
                }
            }
        }
        public void calc(){
            Func dl = new Func(IDS.n0);
            foreach (KeyValuePair<Func,Many> d in data) {
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


                    if (d.Value.data.Count == 0) {
                        d.Value.set(data[e1]);
                        d.Value.exp(d.Key);
                    }
//                }
            }
        }
    }

    class Row: IComparable {
        public int point;
        public SortedDictionary<int,Many2> data;
        public Row() {
            point = 0;
            data = new SortedDictionary<int,Many2>();
        }
        public Row(Row r) {
            point = r.point;
            data = new SortedDictionary<int,Many2>();
            foreach(KeyValuePair<int,Many2> m in r.data) data.Add(m.Key,new Many2(m.Value));
        }
        public void set(Row r) {
            point = r.point;
            data.Clear();
            foreach(KeyValuePair<int,Many2> m in r.data) data.Add(m.Key,new Many2(m.Value));
        }
        Many2 step(Many2 ind, int exp, Vals val)
        {
            Many2 r = new Many2(ind);
            r.replace(Program.root.v_x.vals[0],new Func(val));
            r.replace(Program.root.v_n.vals[0],new Func(IDS.nums[IDS.znums+exp]));
            Num n = r.simple();
            if (n != null) return new Many2(n); else return r;
        }
        public Many2 prep_calc(Vals val, int steps)
        {
            int len = data.Count - point;
            if (len < 1) IDS.sys.error("row: empty");
            Many2 calc = new Many2(IDS.n0), t;
            int i0 = 0; while (i0 < point) {calc.add(step(data[i0],i0,val),1); i0++;}
            int e = point, i1 = 0; while (i1 < steps) {
                i0 = 0; while (i0 < len) {
                    t = step(data[point + i0],e,val);
                    calc.add(t,1);
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
            foreach(KeyValuePair<int,Many2> m in data) r = m.Value.expand(val,exu,exd) || r;
            return r;
        }
        public void expand()
        {
            foreach(KeyValuePair<int,Many2> m in data) m.Value.expand();
        }
        public void simple()
        {
            foreach(KeyValuePair<int,Many2> m in data) m.Value.simple();
        }

        public void deeper(int d) 
        {
            foreach(KeyValuePair<int,Many2> m in data) m.Value.deeper(d);
        }
        public int CompareTo(object obj) {
            if (obj == null) return data.ElementAt(0).Value.CompareTo(null);
            Row r = obj as Row; int t = 0;
            if (data.Count != r.data.Count) return (data.Count < r.data.Count ? -1 : +1);
            foreach(KeyValuePair<int,Many2> m in data) 
            {
                t = m.Value.CompareTo(r.data[m.Key]);
                if (t != 0) return t;
            }
            return t;
        }
        public void findvals(One o)
        {
            foreach(KeyValuePair<int,Many2> m in data) m.Value.findvals(o);          
        }
    }

    class Func: Power<Func>, IPower, IComparable
    {
        public const int types = 8, t_val = 0, t_num = 1, t_many2 = 2, t_ln = 3, t_fact = 4, t_int = 5, t_sign = 6, t_row = 7;
        public Object data;
        public int type; //0: &val, 1: &Num {imutable}, 2: &many2, 3: ln(Many2), 4: fact(Many2), 5: int(Many2), 6: sign(Many2), 7: row(Row)
        public Func()
        {
            type = -1; data = null;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Vals v)
        {
            type = Func.t_val; data = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Num n)
        {
            type = Func.t_num; data = n;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(One o, Num n)
        {
            type = Func.t_many2; data = new Many2(o,n);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Row r)
        {
            type = Func.t_row; data = r;
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
            Func.set_func[f.type](this,f);
        }
        static public Action<Func,Func>[] set_func = {
              (Func t, Func f) => {t.data = f.data;},
              (Func t, Func f) => {t.data = f.data;},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Row((Row)(f.data));}
                         };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void set(Func f)
        {
            Func.set_func[f.type](this,f);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void copy(ref Func f)
        {
            Func.set_func[type](f,this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void set0()
        {
            type = Func.t_num; data = IDS.n0;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void set1()
        {
            type = Func.t_num; data = IDS.n1;
        }
        static public Func<Func,Num>[] get_num_part_func = {
                (Func t) => {return IDS.n0;},
                (Func t) => {return (Num)(t.data);},
                (Func t) => {return ((Many2)(t.data)).get_num_part();},
                (Func t) => {return IDS.n0;},
                (Func t) => {return IDS.n0;},
                (Func t) => {return IDS.n0;},
                (Func t) => {return IDS.n0;},
                (Func t) => {return IDS.n0;}
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Num get_num_part()
        {
            return Func.get_num_part_func[type](this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isconst(int n) { return (type == Func.t_num ? ((Num)data).isint(n): false); }
        static public Func<Func,int>[] type_pow_func = {
                (Func t) => {return 2;},
                (Func t) => {return ((((Num)(t.data)).isint()) ? 0 : 1);},
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
        static public Action<Func,Vals,Func>[] repl_func = {
                (Func t, Vals v, Func f) => {
                    if (((Vals)(t.data) == v)) {t.type = f.type; t.data = f.data;}
                },
                (Func t, Vals v, Func f) => {},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {}
         };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void replace(Vals v, Func f)
        {
            Func.repl_func[type](this,v,f);
        }

        static public Func<Func, Func, Exps_f, Exps_f,bool>[] expand2_func = {
                (Func t, Func v, Exps_f u, Exps_f d) => {if (t.data == v.data) {t.type = Func.t_many2; t.data = new Many2(new Many(u.mvar),new Many(d.mvar)); return true;} else return false;},
                (Func t, Func v, Exps_f u, Exps_f d) => {return false;},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Row)(t.data)).expand(v,u,d);}
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
            Exps_f exu,exd;
            if (f_exp.type == Func.t_many2) {
                exu = new Exps_f(((Many2)(f_exp.data)).up,v.deep);
                exd = new Exps_f(((Many2)(f_exp.data)).down,v.deep);
            } else {
                exu = new Exps_f(new Many(new Func(f_exp)),v.deep);
                exd = new Exps_f(new Many(IDS.n1),v.deep);
            }
            return Func.expand2_func[type](this,fv,exu,exd);
        }
        static public Action<Func>[] expand_func = {
                (Func t) => {},
                (Func t) => {},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Row)(t.data)).expand();}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void expand()
        {
            Func.expand_func[type](this);
        }
        static public Action<Func>[] neg_func = {
                (Func t) => {
                    t.data = new Many2(new Many(new One(t)));
                    t.type = Func.t_many2; ((Many2)(t.data)).neg();
                },
                (Func t) => {t.data = Num.neg((Num)(t.data));},
                (Func t) => {((Many2)(t.data)).neg();},
                (Func t) => {},
                (Func t) => {},
                (Func t) => {},
                (Func t) => {},
                (Func t) => {}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void neg()
        {
            Func.neg_func[type](this);
        }
        static public Action<Func>[] div_func = {
                (Func t) => {
                    t.data = new Many2(new Many(new One(t)));
                    t.type = 2; ((Many2)(t.data)).div();
                },
                (Func t) => {t.data = Num._div((Num)(t.data));},
                (Func t) => {((Many2)(t.data)).div();},
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
        static public Action<Func,Func>[] mul_func = {
                (Func t, Func f) => {//0:0
                    One _o = new One(t); _o.addto(new Func(f),new Func(IDS.n1));
                    t.type = 2; t.data = new Many2(new Many(_o));
                },
                (Func t, Func f) => {
                    if (((Num)(f.data)).sign == 0) {
                        t.type = 1; t.data = f.data;
                    } else if (! ((Num)(f.data)).isint(1)) {
                        One _o = new One(t);
                        t.type = 2; t.data = new Many2(new Many(_o,(Num)(f.data)));
                    }
                },
                (Func t, Func f) => {
                    One _o = new One(t);
                    t.type = 2; t.data = new Many2((Many2)(f.data));
                    ((Many2)(t.data)).mul(_o,IDS.n1);
                },
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {//1 *= 0
                    if (((Num)(t.data)).sign != 0) {
                        if (((Num)(t.data)).isint(1)) {
                            t.type = f.type; t.data = f.data;
                        } else {
                            One _o = new One(f);
                            t.type = 2; t.data = new Many2(new Many(_o,(Num)(t.data)));
                        }
                    }
                },
                (Func t, Func f) => {  //1 *= 1
                    t.data = Num.mul((Num)(t.data),(Num)(f.data));
                },
                (Func t, Func f) => { //1 *= 2
                    if (((Num)(t.data)).sign != 0) {
                        if (((Num)(t.data)).isint(1)) {
                            t.type = f.type; t.data = f.data;
                        } else {
                            t.type = 2; t.data = new Many2((Num)(t.data));
                            ((Many2)(t.data)).mul((Many2)f.data);
                        }
                    }
                },
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {//2 *= 0
                    ((Many2)(t.data)).mul(new One(f),IDS.n1);
                },
                (Func t, Func f) => {  //2 *= 1
                    if (! ((Num)(f.data)).isint(1)) {
                        if (((Num)(f.data)).sign == 0) {
                            t.type = f.type; t.data = f.data;
                        } else {
                            ((Many2)(t.data)).mul((Num)f.data);
                        }
                    }
                },
                (Func t, Func f) => { //2 *= 2
                    ((Many2)(t.data)).mul((Many2)f.data);
                },
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {},//3 *= 0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {},//4 *= 0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {},//5 *= 0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {},//6 *= 0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {},//7 *= 0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void mul(Func f)
        {
            Func.mul_func[type*Func.types + f.type ](this,f);
        }
        static public Action<Func,Num>[] muln_func = {
                (Func t, Num n) => {
                    One _o = new One(t);
                    t.type = 2; t.data = new Many2(new Many(_o,n));
                },
                (Func t, Num n) => { 
                    t.data = Num.mul((Num)(t.data),n);
                },
                (Func t, Num n) => { 
                    ((Many2)(t.data)).mul(n);
                },
                (Func t, Num n) => {},
                (Func t, Num n) => {},
                (Func t, Num n) => {},
                (Func t, Num n) => {},
                (Func t, Num n) => {}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void mul(Num n)
        {
            if (! n.isint(1)) {
                if (n.sign == 0) {
                    type = 1; data = n;
                } else {
                    Func.muln_func[type](this,n);
                }
            }
        }

        static public Action<Func,Func>[] common_func = {
                (Func t, Func f) => {//0:0
                    if (t.data != f.data) {
                        t.type = 1; t.data = IDS.n0;
                    }
                },
                (Func t, Func f) => {
                        t.type = 1; t.data = IDS.n0;
                },
                (Func t, Func f) => {
                    One _o = new One(t); 
                    Num _nd = ((Many2)(f.data)).down.get_Num(); _nd.div();
                    if ((_nd != null) && ((Many2)(f.data)).up.data.ContainsKey(_o) && (((Many2)(f.data)).up.data[_o].sign * _nd.sign > 0)) {
                        _nd.mul(((Many2)(f.data)).up.data[_o]);
                        if (_nd.great(IDS.n1)) {
                            Many _u = new Many(); _u.data.Add(_o,_nd);
                            t.type = 2; t.data = new Many2(_u,new Many(IDS.n1));
                        }
                    } else {t.type = 1; t.data = IDS.n0;}
                },
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {
                        t.type = 1; t.data = IDS.n0;
                }, //1:0
                (Func t, Func f) => { //1 1
                        t.data = Num.common((Num)(t.data),(Num)(f.data)); //no new - just select
                },
                (Func t, Func f) => { //1 2
                        t.data = Num.common((Num)(t.data),((Many2)(f.data)).get_num_part());
                },
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {//2:0
                    One _o = new One(f);
                    Num _nd = ((Many2)(t.data)).down.get_Num(); _nd.div();
                    if ((_nd != null) && ((Many2)(t.data)).up.data.ContainsKey(_o) && (((Many2)(t.data)).up.data[_o].sign * _nd.sign > 0)) {
                        _nd.mul(((Many2)(t.data)).up.data[_o]);
                        if (_nd.great(IDS.n1)) {
                            ((Many2)(t.data)).up = new Many(_o,_nd);
                            ((Many2)(t.data)).down = new Many(IDS.n1);
                        } else {
                            t.type = 0; t.data = f.data;
                        }
                    } else {t.type = 1; t.data = IDS.n0;}
                },
                (Func t, Func f) => { //2:1
                        t.type = 1; t.data = Num.common(((Many2)(t.data)).get_num_part(),(Num)(f.data)); 
                },
                (Func t, Func f) => { //2:2
                        ((Many2)(t.data)).common((Many2)(f.data));
                        Num r = ((Many2)(t.data)).get_Num();
                        if (r != null) {t.data = r; t.type = 1;}
                },
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {}, //3:0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {}, //4:0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {}, //5:0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {}, //6:0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {}, //7:0
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {}
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void common(Func f)
        {
            Func.common_func[type*Func.types + f.type ](this,f);
        }



        static public Action<Func,Func,int>[] add_func = {
                (Func t, Func f, int s) => {//0:0
                    Many _u = new Many(t);
                    One _o = new One(new Func(f));
                    _u.add(_o,s);
                    t.type = 2; t.data = new Many2(_u);
                },
                (Func t, Func f, int s) => {
                    if (((Num)(f.data)).sign != 0) {
                        Many _u = new Many(t);
                        _u.add(new One(),(Num)(f.data),s);
                        t.type = 2; t.data = new Many2(_u);
                    }
                },
                (Func t, Func f, int s) => {
                    Many2 _fm = new Many2((Many2)(f.data)), _tm = new Many2(new One(t));
                    _tm.add(_fm,s);
                    t.type = 2; t.data = _tm;
                },
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},

                (Func t, Func f, int s) => {//1:0
                    if (((Num)(t.data)).sign == 0) {
                        t.type = f.type; t.data = f.data;
                    } else {
                        Many _u = new Many(new One(f));
                        _u.add(new One(),(Num)(t.data),s);
                        t.type = 2; t.data = new Many2(_u);
                    }
                },
                (Func t, Func f, int s) => { //1:1
                        t.data = Num.add((Num)(t.data),(Num)(f.data),s);
                },
                (Func t, Func f, int s) => { //1:2
                    if (((Num)(t.data)).sign == 0) {
                        t.type = f.type; t.data = f.data;
                    } else {
                        t.type = 2; t.data = new Many2((Num)(t.data));
                        ((Many2)(t.data)).add((Many2)f.data,s);
                    }
                },
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},

                (Func t, Func f, int s) => {//2:0
                    One _o = new One(f);
                    Many _u = new Many(); _u.data.Add(_o,IDS.n1);
                    ((Many2)(t.data)).add(new Many2(_u,new Many(IDS.n1)),s);
                },
                (Func t, Func f, int s) => { //2:1
                    if (((Num)(f.data)).sign != 0) {
                        ((Many2)(t.data)).add((Num)f.data,s);
                    }
                },
                (Func t, Func f, int s) => { //2:2
                        ((Many2)(t.data)).add((Many2)(f.data),s);
                },
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},

                (Func t, Func f, int s) => {}, //3:0
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},

                (Func t, Func f, int s) => {}, //4:0
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},

                (Func t, Func f, int s) => {}, //5:0
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},

                (Func t, Func f, int s) => {}, //6:0
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},

                (Func t, Func f, int s) => {}, //7:0
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {}
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void add(Func f, int s)
        {
            Func.add_func[type*Func.types + f.type ](this,f,s);
        }

        static public Func<Func,Func,int>[] comp_func = {
                (Func t, Func f) => {return (t.data == f.data ? 0 : (((Vals)(t.data)).ind < ((Vals)(f.data)).ind ? -1 : 1));}, //0:0
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},

                (Func t, Func f) => {return -1;}, //1:0
                (Func t, Func f) => {return ((Num)(t.data)).CompareTo(f.data);},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return ((Num)(t.data)).CompareTo(null);},

                (Func t, Func f) => {return -1;}, //2:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(f.data);},
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
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(null);},

                (Func t, Func f) => {return -1;}, //4:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(f.data);},
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
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(null);},

                (Func t, Func f) => {return -1;}, //6:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo((Many2)(f.data));},
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
                (Func t, Func f) => {return 1;}

               };
        public int CompareTo(object obj) {
            Func f = obj as Func;
            int t = (f == null ? Func.types : f.type);
            return Func.comp_func[type*(Func.types+1) + f.type ](this,f);
        }

        static public Action<Func>[] simple_func = {
                (Func t) => {},
                (Func t) => {t.data = ((Num)(t.data)).simple();},
                (Func t) => {
                    Num r = ((Many2)(t.data)).simple();
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
                    Num n = ((Many2)(t.data)).get_Num();
                    if (n != null) {
                        t.data = Func.f_fact(n,t); t.type = 1;
                    }
                },
                (Func t) => {
                    ((Many2)(t.data)).simple();
                    Num n = ((Many2)(t.data)).get_Num();
                    if (n != null) {
                        t.type = 1; t.data = Func.f_int(n);
                    }
                },
                (Func t) => {
                    ((Many2)(t.data)).simple();
                    Num n = ((Many2)(t.data)).get_Num();
                    if (n != null) {
                        t.type = 1; t.data = Func.f_sign(n);
                    }
                },
                (Func t) => {((Row)(t.data)).simple();}
                              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void simple()
        {
            Func.simple_func[type](this);
        }

        static public Func<Func,int>[] sign_func = {
                (Func t) => {return 1;},
                (Func t) => {return ((Num)(t.data)).sign;},
                (Func t) => {return ((Many2)(t.data)).sign();},
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


        static public Action<Func,int>[] deeper_func = {
                (Func t, int d) => {
                    int _i; if ((_i = ((Vals)(t.data)).deep + d) >= ((Vals)(t.data)).var.vals.Length) IDS.sys.error("too deep");
                    t.data = ((Vals)(t.data)).var.vals[_i];
                },
                (Func t, int d) => {},
                (Func t, int d) => {((Many2)(t.data)).deeper(d);},
                (Func t, int d) => {((Many2)(t.data)).deeper(d);},
                (Func t, int d) => {((Many2)(t.data)).deeper(d);},
                (Func t, int d) => {((Many2)(t.data)).deeper(d);},
                (Func t, int d) => {((Many2)(t.data)).deeper(d);},
                (Func t, int d) => {((Row)(t.data)).deeper(d);}
               };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void deeper(int d) {
            Func.deeper_func[type](this,d);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func deeper(Func f, int d) {
            Func r = new Func(f);
            Func.deeper_func[r.type](r,d);
            return r;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Num f_ln(Num n, Func t)
        {
            IDS.now_func = t;
            Num r = new Num(n); r.ln(); return r;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Num f_fact(Num n, Func t)
        {
            IDS.now_func = t;
            if ((! n.isint()) || (n.sign < 0) || (n.up >= Program.root.fact.Count())) IDS.sys.error("fact: wrong");
            return Program.root.fact[(int)n.up];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Num f_int(Num n)
        {
            if (n.down != 1) return new Num(n.sign*n.up/n.down); else  return n;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Num f_sign(Num n)
        {
            return IDS.nums[n.sign + IDS.znums];
        }
        static public Func<Func,Num>[] calc_func = {
                (Func t) => {return ((Vals)(t.data)).get_val(); },
                (Func t) => {return (Num)(t.data);},
                (Func t) => {return ((Many2)(t.data)).calc();},
                (Func t) => {return Func.f_ln(((Many2)(t.data)).calc(),t);},
                (Func t) => {return Func.f_fact(((Many2)(t.data)).calc(),t);},
                (Func t) => {return Func.f_int(((Many2)(t.data)).calc());},
                (Func t) => {return Func.f_sign(((Many2)(t.data)).calc());},
                (Func t) => {
                    IDS.sys.error("row:not prep");
                    return new Num(0);
                }
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Num calc()
        {
            return Func.calc_func[type](this);
        }

        static public Func<Func,Num,Num>[] calce_func = {
                (Func t, Num e) => {return ((Vals)(t.data)).get_val(e); },
                (Func t, Num e) => {return Num.exp((Num)(t.data),e);},
                (Func t, Num e) => {return Num.exp(((Many2)(t.data)).calc(),e);},
                (Func t, Num e) => {
                    return Num.exp(Func.f_ln(((Many2)(t.data)).calc(),t),e);
                },
                (Func t, Num e) => {
                    return Num.exp(Func.f_fact(((Many2)(t.data)).calc(),t),e);
                },
                (Func t, Num e) => {
                    Num r = ((Many2)(t.data)).calc();
                    if (r.down != 1) {r = new Num(r.sign*r.up/r.down);}
                    return Num.exp(r,e);
                },
                (Func t, Num e) => {
                    int s = ((Many2)(t.data)).calc().sign;
                    return IDS.nums[((e.up & 1) == 0 ? s*s : s) + IDS.znums];
                },
                (Func t, Num e) => {
                    IDS.sys.error("row: not prep");
                    return new Num(0);
                }
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Num calc(Num e)
        {
            return (e == IDS.n1 ? Func.calc_func[type](this) : Func.calce_func[type](this,e));
        }
        static public Action<Func,One>[] findvals_func = {
              (Func t, One o) => {if (! o.exps.ContainsKey(t)) o.exps.Add(new Func(t),new Func(IDS.n1));},
              (Func t, One o) => {},
              (Func t, One o) => {((Many2)(t.data)).findvals(o);},
              (Func t, One o) => {((Many2)(t.data)).findvals(o);},
              (Func t, One o) => {((Many2)(t.data)).findvals(o);},
              (Func t, One o) => {((Many2)(t.data)).findvals(o);},
              (Func t, One o) => {((Many2)(t.data)).findvals(o);},
              (Func t, One o) => {((Row)(t.data)).findvals(o);}
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void findvals(One o)
        {
            Func.findvals_func[type](this,o);
        }

        static public Action<Func,Vals>[] diff_down_func = {
              (Func t, Vals a) => {if (t.data == a) t.set1(); else t.set0();},
              (Func t, Vals a) => {t.set0();},
              (Func t, Vals a) => {t.data = ((Many2)(t.data)).diff_down(a);},
              (Func t, Vals a) => {},

              (Func t, Vals a) => {},
              (Func t, Vals a) => {},
              (Func t, Vals a) => {},
              (Func t, Vals a) => {}
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void diff_down(Vals d) 
        {
            Func.diff_down_func[type](this,d);
        }

        static public Func<Func,Func,Vals, Func>[] diffe_down_func = {
                (Func t, Func e, Vals a) => { //0:0
                    One o;
                    if ((t.type == Func.t_val) && (t.data == a)) {
                        if ((e.type == Func.t_val) && (e.data == a)) {
                            o = new One(new Func(t),new Func(e));
                            Many m = new Many(IDS.n1);
                            m.add(new One(new Func(3,new Many2(new Func(t)))),IDS.n1,1);
                            o.addto(new Func(new Many2(m)));
                        } else {
                            Func tt = new Func(e); tt.add(new Func(IDS.n1),-1);
                            o = new One (new Func(t),tt); o.addto(new Func(e)); 
                        }
                    } else  {
                        if ((e.type == Func.t_val) && (e.data == a)) {
                            o = new One(new Func(t),new Func(e)); o.addto(new Func(3,new Many2(new Func(t))));
                        } else {
                            return new Func(IDS.n0);
                        }
                    }
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {//0:1
                    if ((t.type == Func.t_val) && (t.data == a)) {
                        One o = new One (new Func(t),new Func(Num.sub((Num)(e.data),IDS.n1)));
                        return new Func(new Many2(o,(Num)(e.data)));
                    }
                    return new Func(IDS.n0);
                },
                (Func t, Func e, Vals a) => {//0:2
                    Many2 dm = ((Many2)(e.data)).diff_down(a);
                    One o = new One(new Func(3,new Many2(new Func(t))));
                    if ((t.type == Func.t_val) && (t.data == a)) {
                        o.addto(new Func(dm));
                        o.addto(new Func(t));
                        Many2 m0 = new Many2((Many2)(e.data));
                        m0.add(new Many2(o),1);
                        Many2 m1 = new Many2((Many2)(e.data));
                        m1.add(IDS.n1,-1);
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
                        o0.addto(new Func(new Many2((Many2)(e.data))),new Func(IDS.n_1));
                        Many m0 = new Many(o0);
                        m0.add(new One(new Func(e)),1);
                        One o = new One(new Func(new Many2(m0)));
                        Many2 m1 = new Many2(new Func(e));
                        m1.add(IDS.n1,-1);
                        o.addto(new Func(t),new Func(m1));
                        return new Func(new Many2(o));
                    } else {
                        One o = new One(new Func(3,new Many2(new Func(t))));
                        o.addto(new Func(dm)); 
                        o.addto(new Func(t),new Func(e));
                        o.addto(new Func(new Many2((Many2)(e.data))),new Func(IDS.n_1));
                        return new Func(new Many2(o));
                    }
                },
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},


                (Func t, Func e, Vals a) => {//1:0
                    if ((e.type == Func.t_val) && (e.data == a)) {
                        One o = new One(new Func(t),new Func(e)); o.addto(new Func(3,new Many2(new Func(t))));
                    }
                    return new Func(IDS.n0);
                },
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},//1:1
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
                    o.addto(new Func(new Many2((Many2)(e.data))),new Func(IDS.n_1));
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},

                (Func t, Func e, Vals a) => {//2:0
                    Func _e = new Func(e); _e.add(new Func(IDS.n1),-1);
                    One o = new One(new Func(t),_e);
                    Many2 dm = ((Many2)(t.data)).diff_down(a);
                    if ((e.type == Func.t_val) && (e.data == a)) {
                        One om = new One(new Func(t)); 
                        om.addto(new Func(3,new Many2((Many2)(t.data))));
                        dm.mul(new One(e),IDS.n1);
                        Many m0 = new Many(om);
                        m0.add(new One(new Func(dm)),1);
                        o.addto(new Func(new Many2(m0)));
                    } else {
                        o.addto(new Func(dm));
                        o.addto(new Func(e));
                    }
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {//2:1
                    One o = new One(new Func(t),new Func(Num.sub((Num)(e.data),IDS.n1)));
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
                    Many m = new Many(o0); m.add(o1,1);

                    Many2 _e = new Many2((Many2)(e.data)); _e.add(IDS.n1,-1);
                    One o = new One(new Func(t),new Func(_e));

                    o.addto(new Func(new Many2(m)));

                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {//2:3
                    Many2 dt = ((Many2)(t.data)).diff_down(a);
                    Many2 de = ((Many2)(e.data)).diff_down(a);
                    One o0 = new One(new Func(dt)); o0.addto(new Func(e));
                    o0.addto(new Func(3,new Many2((Many2)(t.data))),new Func(IDS.n_1));
                    One o1 = new One(new Func(de)); o1.addto(new Func(t));
                    o1.addto(new Func(3,new Many2((Many2)(e.data))),new Func(IDS.n_1));
                    Many m = new Many(o0); m.add(o1,1);
                    One o = new One(new Func(t),new Func(e));
                    o.addto(new Func(new Many2(m)));
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},

                (Func t, Func e, Vals a) => {//3:0
                    Many2 dm = ((Many2)(t.data)).diff_down(a);
                    if ((e.type == Func.t_val) && (e.data == a)) {
                        One o0 = new One(new Func(t),new Func(IDS.n_1));
                        o0.addto(new Func(new Many2((Many2)(t.data))),new Func(IDS.n_1));
                        dm.mul(new One(e),IDS.n1);
                        o0.addto(new Func(dm));
                        Many _m = new Many(new Func(3,new Many2(new Func(t))));
                        _m.add(o0,1);
                        One o = new One(new Func(t), new Func(e));
                        o.addto(new Func(new Many2(_m)));
                        return new Func(new Many2(o));
                    } else {
                        Many2 m = new Many2((Many2)(t.data));
                        One o = new One(new Func(dm));
                        o.addto(new Func(m),new Func(IDS.n_1));
                        Many _m = new Many(new Func(e)); _m.add(new One(),IDS.n1,-1);
                        o.addto(new Func(t),new Func(new Many2(_m)));
                        o.addto(new Func(e));
                        return new Func(new Many2(o));
                    }
                },
                (Func t, Func e, Vals a) => {//3:1
                    Many2 m = new Many2((Many2)(t.data));
                    Many2 dm = ((Many2)(t.data)).diff_down(a);
                    One o = new One(new Func(dm));
                    o.addto(new Func(m),new Func(IDS.n_1));
                    o.addto(new Func(t),new Func(Num.sub((Num)(e.data),IDS.n1)));
                    return new Func(new Many2(o,(Num)(e.data)));
                },
                (Func t, Func e, Vals a) => {//3:2
                    Many2 dt = ((Many2)(t.data)).diff_down(a);
                    Many2 de = ((Many2)(e.data)).diff_down(a);

                    One o0 = new One(new Func(dt)); o0.addto(new Func(new Many2((Many2)(e.data))));
                    o0.addto(new Func(new Many2((Many2)(t.data))),new Func(IDS.n_1));
                    o0.addto(new Func(t),new Func(IDS.n_1));
                    One o1 = new One(new Func(3,new Many2(new Func(t))));
                    o1.addto(new Func(de));
                    Many _m = new Many(o0); _m.add(o1,1);

                    One o = new One(new Func(t), new Func(e));
                    o.addto(new Func(new Many2(_m)));
                    
                    return new Func(new Many2(o));
                },
                (Func t, Func e, Vals a) => {//3:3
                    Many2 dt = ((Many2)(t.data)).diff_down(a);
                    Many2 de = ((Many2)(e.data)).diff_down(a);

                    One o0 = new One(new Func(dt)); o0.addto(new Func(e));
                    o0.addto(new Func(new Many2((Many2)(t.data))),new Func(IDS.n_1));
                    o0.addto(new Func(t),new Func(IDS.n_1));
                    One o1 = new One(new Func(3,new Many2(new Func(t))));
                    o1.addto(new Func(de));
                    o1.addto(new Func(new Many2((Many2)(e.data))),new Func(IDS.n_1));
                    Many _m = new Many(o0); _m.add(o1,1);

                    One o = new One(new Func(t), new Func(e));
                    o.addto(new Func(new Many2(_m)));
                    
                    return new Func(new Many2(o));
                    
                },
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},

                (Func t, Func e, Vals a) => {return new Func(IDS.n0);}, //4:0
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},

                (Func t, Func e, Vals a) => {return new Func(IDS.n0);}, //5:0
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},

                (Func t, Func e, Vals a) => {return new Func(IDS.n0);}, //6:0
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},

                (Func t, Func e, Vals a) => {return new Func(IDS.n0);}, //7:0
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);},
                (Func t, Func e, Vals a) => {return new Func(IDS.n0);}
               };


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func diff_down(Func exp, Vals d) 
        {
            if ((type == Func.t_row) || (exp.type == Func.t_row)) IDS.sys.error("cant diff on innate row");
            Func r;
            if (type > Func.t_ln) {
                if (((Many2)(data)).hasval(d)) IDS.sys.error("cant diff on such");
                if (exp.type > Func.t_ln) {
                    if (((Many2)(exp.data)).hasval(d)) IDS.sys.error("cant diff on such");
                    return new Func(IDS.n0);
                } else {
                    r = Func.diffe_down_func[0 + exp.type](this,exp,d);
                }
            } else {
                if (exp.type > Func.t_ln) {
                    if (((Many2)(exp.data)).hasval(d)) IDS.sys.error("cant diff on such");
                    r = Func.diffe_down_func[type*Func.types + 0](this,exp,d);
                } else {
                    r = Func.diffe_down_func[type*Func.types + exp.type](this,exp,d);
                }
            }
            r.simple(); return r;
        }
    }

    class MAO_dict {
        static short bmexp = 11;
        static int mexp = 1 << bmexp;
        public int nvals;
        public Num[] exps;
        public Func[] vals;
        public Many_as_one[] mao;
        public SortedDictionary<Func,int> to_val;
        ushort[] eneg,eadd; bool[] eflg_a;
        ushort lexp, lval;
        public MAO_dict(int v)
        {
            nvals = v;
            exps = new Num[mexp];
            vals = new Func[nvals];
            mao = new Many_as_one[nvals];
            to_val = new SortedDictionary<Func,int>();
            eneg = new ushort[mexp * mexp]; eadd = new ushort[mexp * mexp]; eflg_a = new bool[mexp * mexp];
            for (uint i = 0; i < mexp * mexp; i++) { eneg[i] = 0xFFFF; eadd[i] = 0xFFFF; eflg_a[i] = true; }
            lexp = 2; exps[0] = new Num(0); exps[1] = new Num(1);
            lval = 0;
        }
        public ushort exp(Num e)
        {
            ushort i;
            for (i = 0; i < lexp; i++) if (e.equ(exps[i])) return i;
            if (lexp > mexp-2) IDS.sys.error("too Many exp");
            exps[lexp++] = new Num(e);
            return i;
        }
        public int val(Func v)
        {
            if (! to_val.ContainsKey(v))
            {
                if (lval >= nvals) IDS.sys.error("MAO: too many funcs");
                to_val[v] = lval;
                vals[lval++] = v;
            }
            return to_val[v];
        }
        public void set_mao(int i)
        {
            if (i >= lval) IDS.sys.error("MAO: wrong expand");
            if (mao[i] == null) {
                if ((vals[i].type != 0) || (((Vals)(vals[i].data)).var.var == null)) IDS.sys.error("MAO: wrong expand");
                Func f = new Func(((Vals)(vals[i].data)).var.var); if (f.type != 2) IDS.sys.error("MAO: only many2");
                f.deeper(((Vals)(vals[i].data)).deep);
                mao[i] = new Many_as_one(f,this);
            }
        }
        public void expand(int to, int by)
        {
            set_mao(to); set_mao(by);
            mao[to].expand(mao[by],by);
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
    class MAO_key :IComparable {
        public MAO_dict dict;
        public ushort[] key;
        public MAO_key(MAO_dict d)
        {
            dict = d;
            key = new ushort[d.nvals];
            for (int i = 0; i < d.nvals; i++) key[i]=0;
        }
        public MAO_key(MAO_dict d, ushort[] k)
        {
            dict = d;
            key = new ushort[dict.nvals];
            for (int i = 0; i < d.nvals; i++) key[i] = k[i];
        }
        public MAO_key(MAO_key k)
        {
            dict = k.dict;
            key = new ushort[dict.nvals];
            set(k);
        }
        public void set(MAO_key k)
        {
            for (int i = 0; i < dict.nvals; i++) key[i] = k.key[i];
        }
        public void neg()
        {
            for (int i = 0; i < dict.nvals; i++) key[i] = dict.neg(key[i]);
        }
        public bool test_m(MAO_key a) {
            bool ret = true; ushort tmp;
            for (int i = 0; i < dict.nvals; i++)
            {
                tmp = dict.add(key[i], a.key[i]);
                ret = ret && dict.test_a(key[i], a.key[i]);
            }
            return ret;
        }
        public bool mul(MAO_key a) {
            bool ret = true; ushort tmp;
            for (int i = 0; i < dict.nvals; i++)
            {
                tmp = dict.add(key[i], a.key[i]);
                ret = ret && dict.test_a(key[i], a.key[i]);
                key[i] = tmp;
            }
            return ret;
        }
        public bool mul(MAO_key a0, MAO_key a1) {
            bool ret = true;
            for (int i = 0; i < dict.nvals; i++)
            {
                key[i] = dict.add(a0.key[i], a1.key[i]);
                ret = ret && dict.test_a(a0.key[i], a1.key[i]);
            }
            return ret;
        }

        public int CompareTo(object obj) {
            if (obj == null) return 1;
            MAO_key k = obj as MAO_key;
            for (int i = 0; i < dict.nvals; i++) {
                if (key[i] > k.key[i]) return -1;
                if (key[i] < k.key[i]) return 1;
            }
            return 0;
        }
    }
    class Many_as_one {
        MAO_dict dict;
        public SortedDictionary<MAO_key,Num>[] data;

        public Many_as_one(MAO_dict d)
        {
            dict = d;
            _data_i();
        }
        void _data_i(){
            data = new SortedDictionary<MAO_key,Num>[2];
            data[0] = new SortedDictionary<MAO_key, Num>();
            data[1] = new SortedDictionary<MAO_key, Num>();
        }
        public KeyValuePair<MAO_key,Num> fr_one(KeyValuePair<One,Num> o)
        {
            int i0,v0;
            KeyValuePair<MAO_key, Num> ret = new KeyValuePair<MAO_key,Num>(new MAO_key(dict), new Num(o.Value));
            for (i0 = 0; i0 < dict.nvals; i0++) ret.Key.key[i0] = 0;
            foreach (KeyValuePair<Func,Func> f in o.Key.exps)
            {
                if (f.Value.type_pow() > 1) IDS.sys.error("cant fast on complex exp");
                v0 = dict.val(f.Key);
                ret.Key.key[v0] = dict.exp((Num)(f.Value.data));
            }
            return ret;
        }
        public One to_one(MAO_key fr)
        {
            int i;
            One ret = new One();
            for (i = 0; i < dict.nvals; i++)
                if (fr.key[i] != 0) ret.exps.Add(new Func(dict.vals[i]),new Func(dict.exps[fr.key[i]]));
            return ret;
        }

        public void add(int ud, MAO_key m, Num n)
        {
            if (data[ud].ContainsKey(m)) data[ud][m].add(n); else data[ud].Add(new MAO_key(m), new Num(n));
        }
        public void add(int ud, KeyValuePair<MAO_key, Num> a)
        {
            if (data[ud].ContainsKey(a.Key)) data[ud][a.Key].add(a.Value); else data[ud].Add(new MAO_key(a.Key), new Num(a.Value));
        }
        public void add(int ud, SortedDictionary<MAO_key, Num> fr)
        {
            KeyValuePair<MAO_key, Num> tmp;
            foreach (KeyValuePair<MAO_key, Num> d in fr) { tmp = d; add(ud, tmp); }
        }
        public void mul(int ud, MAO_key m, Num n)
        {
            foreach (KeyValuePair<MAO_key, Num> d in data[ud])
            {
                d.Key.mul(m);
                d.Value.mul(n);
            }
        }
        public void muladd(int ud, SortedDictionary<MAO_key, Num> fr, MAO_key m, Num n)
        {
            KeyValuePair<MAO_key,Num> tmp = new KeyValuePair<MAO_key,Num>(new MAO_key(dict), new Num(0));
            foreach (KeyValuePair<MAO_key,Num> d in fr) {
                tmp.Key.mul(m,d.Key);
                tmp.Value.set(n);
                tmp.Value.mul(d.Value);
                add(ud,tmp);
            }
        }

        public void mul(int ud, SortedDictionary<MAO_key,Num> m0)
        {
            SortedDictionary<MAO_key, Num> tmp1 = data[ud];
            data[ud] = new SortedDictionary<MAO_key, Num>();
            foreach (KeyValuePair<MAO_key, Num> d in m0) muladd(ud, tmp1, d.Key, d.Value);
        }
        public void mul(int ud, SortedDictionary<MAO_key, Num> m0, SortedDictionary<MAO_key, Num> m1)
        {
            data[ud].Clear();
            foreach (KeyValuePair<MAO_key, Num> d in m0) muladd(ud, m1, d.Key, d.Value);
        }
        void set(Many_as_one fr)
        {
            for (int i = 0; i < 2; i++)
            {
                data[i].Clear();
                foreach (KeyValuePair<MAO_key, Num> d in fr.data[i]) data[i].Add(new MAO_key(d.Key), new Num(d.Value));
            }
        }
        public Many_as_one(Func f, MAO_dict d)
        {
            dict = d;
            _data_i();
            foreach (KeyValuePair<One,Num> o in ((Many2)f.data).up.data) add(0, fr_one(o));
            foreach (KeyValuePair<One,Num> o in ((Many2)f.data).down.data) add(1, fr_one(o));
        }
        public Func to_func()
        {
            int i=0, cn = data[0].Count + data[1].Count;
            Many _u = new Many(); Many _d = new Many();
            foreach (KeyValuePair<MAO_key, Num> d in data[0]) {_u.data.Add(to_one(d.Key),new Num(d.Value)); IDS.sys.progr(i++,cn);}
            foreach (KeyValuePair<MAO_key, Num> d in data[1]) {_d.data.Add(to_one(d.Key), new Num(d.Value)); IDS.sys.progr(i++,cn);}
            return new Func(new Many2(_u,_d));
        }

        public Many_as_one(Many_as_one _m, int _e)
        {
            dict = _m.dict;
            Many_as_one tmp = new Many_as_one(dict);
            Many_as_one _tmp = new Many_as_one(dict);
            Many_as_one fr = new Many_as_one(dict);
            Num exp = dict.exps[_e], nexp = new Num(0);
            int i0,_eu = (int)(exp.up);
            fr.set(_m);
            if (exp.down > 1) {
                if ((fr.data[0].Count > 1) || (fr.data[1].Count > 1)) return;
                if (! fr.data[0].ToArray()[0].Value.isint(1)) return;
                if (! fr.data[1].ToArray()[0].Value.isint(1)) return;
                for (i0 = 0; i0 < dict.nvals; i0++) 
                {
                    nexp.set(dict.exps[fr.data[0].ToArray()[0].Key.key[i0]]);
                    nexp.mul((BigInteger)1,exp.down);
                    fr.data[0].ToArray()[0].Key.key[i0] = dict.exp(nexp);
                }
            }
            _data_i();
            data[0].Add(new MAO_key(dict), new Num(1));
            data[1].Add(new MAO_key(dict), new Num(1));

            tmp.set(fr);
            for (int i = _eu; i > 0; i >>= 1) { 
                if ((i&1) != 0) {
                     mul(0,tmp.data[0]);
                     mul(1,tmp.data[1]);
                }
                if (i > 1) {
                    _tmp.set(tmp);
                    tmp.mul(0,_tmp.data[0], _tmp.data[0]);
                    tmp.mul(1,_tmp.data[1], _tmp.data[1]);
                }
            }

            if (exp.sign < 0) {tmp.data[0] = data[0]; data[0] = data[1]; data[1] = tmp.data[0];}
        }
        bool expand(int n, Many_as_one e, int ex)
        {
            bool ret = false;
            int ee; ushort tex;
            Num max_u = new Num(0), max_d = new Num(0), now_u = new Num(0), now_d = new Num(0);
            Many_as_one[] me = new Many_as_one[254], ae = new Many_as_one[254];
            MAO_key z = new MAO_key(dict);
            KeyValuePair<MAO_key, Num> tu = new KeyValuePair<MAO_key,Num>(new MAO_key(dict), new Num(0));
            me[0] = new Many_as_one(e,0);
            me[1] = new Many_as_one(e,1);
            if ((e.data[0].Count < 1) || (e.data[1].Count < 1)) IDS.sys.error("wrong");
            int pnow = 0;
            foreach (KeyValuePair<MAO_key, Num> u in data[n]) 
            {
                tex=u.Key.key[ex];
                tu.Key.set(u.Key);
                tu.Value.set(u.Value);
                if (me[tex] == null) {
                    me[tex] = new Many_as_one(e,tex);
                    if (me[tex].data == null) me[tex] = null; else 
                    {
                        if (max_u.great(dict.exps[tex])) max_u.set(dict.exps[tex]);
                        if (!max_d.great(dict.exps[tex])) max_d.set(dict.exps[tex]);
                    }
                }
                if (me[tex] != null) tu.Key.key[ex] = 0; else tex = 0;
                if (ae[tex] == null) ae[tex] = new Many_as_one(dict);
                ae[tex].add(0,tu);
                IDS.sys.progr(pnow++,data[n].Count);
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
                    } else {
                        for (int tex0 = 0; tex0 < 254; tex0++) if ((ae[tex0] != null) && (tex0 != tex)) ae[tex0].mul(0, me[tex].data[1]);
                        mul(1-n, me[tex].data[1]);
                    }
                    ee = dict.exp(now_u);
                    if (me[ee] == null) me[ee] = new Many_as_one(e,ee);
                    ae[tex].mul(0, me[ee].data[1]);
                    ee = dict.exp(now_d);
                    if (me[ee] == null) me[ee] = new Many_as_one(e,ee);
                    ae[tex].mul(0, me[ee].data[0]);
                }
                IDS.sys.progr(tex,254);
            }
            ee = dict.exp(max_u);
            if (me[ee] == null) me[ee] = new Many_as_one(e,ee);
            mul(1-n, me[ee].data[1]);
            ee = dict.exp(max_d);
            if (me[ee] == null) me[ee] = new Many_as_one(e,ee);
            mul(1-n, me[ee].data[0]);
            data[n].Clear();
            for (tex = 0; tex < 254; tex++) 
            {
                if (ae[tex] != null) {
                  me[tex].mul(0, ae[tex].data[0]);
                  add(n, me[tex].data[0]);
                }
                IDS.sys.progr(tex,254);
            }
            return ret;
        }
        public bool expand(Many_as_one e, int id)
        {
            bool r0 =  expand(0,e,id);
            bool r1 =  expand(1,e,id);
            foreach (KeyValuePair<MAO_key, Num> d in data[0]) d.Value.simple_this();
            foreach (KeyValuePair<MAO_key, Num> d in data[1]) d.Value.simple_this();
            return r0 | r1;
        }
    }


    class Fileio: IDisposable
    {
        StreamReader fin, f611;
        StreamWriter[] fout;
        int nline,ncline, lines, clines;
        string buf,nout,xout;
        public Boolean has, quit;
        public Fileio(string nin, string _nout)
        {
            string iexf;
            StreamReader fc; string ts;
            if (!File.Exists(nin)) Environment.Exit(-1);
            iexf = Path.GetExtension(nin);
            if (_nout == "") {
                nout = Path.GetFileNameWithoutExtension(nin);
                xout = ".txt";
            } else {
                nout = Path.GetFileNameWithoutExtension(_nout);
                xout = Path.GetExtension(_nout);
            }
            fc = new StreamReader(nin);
            clines++; lines = 0; quit = false; while ((ts = fc.ReadLine()) != null)
            {
                if (ts == "`end") break;
                if (ts == "`quit") { quit = true; break; }
                lines++;
                if ((ts.Length > 4) && (ts[0] != '`') && ((ts[0] != '#') || (ts[1] != '#'))) clines++;
            }
            fc.Close();
            f611 = (File.Exists("611"+iexf) ?  new StreamReader("611"+iexf) : null);
            fin = new StreamReader(nin);
            fout = new StreamWriter[40];
            fout[0] = new StreamWriter(nout + "0" + xout);
            nline = 0; ncline = 0; has = true;
//            head = h; 
            buf = fin.ReadLine() + "\n" + fin.ReadLine();
        }
        private string rfile() {
            string r;
            if ((f611 != null) && ((r = f611.ReadLine()) != null)) return r;
            nline++;
            return fin.ReadLine();
        }

        public void progr (int now, int all) {
            if (Program.m0.IsDisposed) Environment.Exit(-1);
            if (all < 6) return;
            if (now > all) now = all;
            int pr_now = now*(Program.m0.sx-1)/all, l_now = ncline*(Program.m0.sx-1)/clines;
            if ((pr_now == Program.m0.pr_now) && (l_now == Program.m0.l_now)) return;
            Program.m0.pr_now = pr_now; Program.m0.l_now = l_now; 
            Program.m0.Set(0);
        }
        public void addline(string add){
            buf = add + buf;
        }
        public string rline()
        {
            string r;
            has = true;
            if (buf.Length > 1) {
                int i = buf.IndexOf("\n");
                if (i < 0) i = buf.Length;
                r = buf.Substring(0,i);
                buf = (i < buf.Length ? buf.Substring(i+1) : "");
            } else {
                r = rfile();
                if ((r == null) || (nline > lines)) { has = false; r = ""; }
                if ((r.Length > 4) && (r[0] != '`') && ((r[0] != '#') || (r[1] != '#'))) ncline++;
            }
            return r;
        }
        public void close()
        {
            fin.Close();
            foreach (StreamWriter _f in fout) if (_f != null) _f.Close();
            if (quit) Environment.Exit(0);
        }
        public void error(string e)
        {
            fout[0].WriteLine("");
            fout[0].WriteLine("Line {0:G}: " + ((IDS.now_func != null) ? IDS.par.print(IDS.now_func,false) + " " : "") + e, nline+1);
            fout[0].Flush();
            Environment.Exit(-1);
        }
        public void wline(int n, string s)
        {
            if (fout[n] == null) fout[n] = new StreamWriter(nout + Parse.m_n_to_c[n] + xout);
            fout[n].WriteLine(s);
            fout[n].Flush();
        }
        public void wstr(int n, string s)
        {
            if (fout[n] == null) fout[n] = new StreamWriter(nout + Parse.m_n_to_c[n] + xout);
            if (s == "\\n") fout[n].WriteLine(""); else fout[n].Write(s);
            fout[n].Flush();
        }
        public void Dispose()
        {
            fin.Close();
            for (int n = 0; n < 40; n++) if (fout[n] != null) fout[n].Close();
        }
    }
    class Deep {
        public char pair,oper;
        public int pos;
        public Deep(char p, char o) {
            pair = p; oper = o;
        }
    }
    class Mbody {
        public int nparm;
        public string body;
        public Mbody(int n, string s)
        { nparm = n; body = s; }
    }

    class Parse: IDisposable
    {
        static public char[] m_n_to_c = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
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
        public static int[] m_c_type_fr = {0,0,2,4, 0,1, 2,3,4,5,6};
        public static int[] m_c_type_sz = {7,2,5,2, 1,1, 1,1,1,1,1};
        public static char isall = (char)0, isname = (char)1, issymb = (char)2, ispair = (char)3, 
                           isnum = (char)4, isabc = (char)5,
                           isoper = (char)6, isother = (char)7, isopen = (char)8, isclose = (char)9, issys = (char)10,
                           isend = (char)13;
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
        public string val;
        public int pos;
        public char now, oper;
        SortedDictionary<string,Mbody> macro;
        List<Deep> deep;
        List<List<Deep>> stack;
        public Fileio sys;
        public Parse(Fileio s)
        {
            sys = s;
            val = ""; pos = 0;
            macro = new SortedDictionary<string,Mbody>();
            deep = new List<Deep>();
            stack = new List<List<Deep>>();
        }
        string _parm() {
            string s1;
                    if (isequnow('{')) s1 = calc().get_sup().ToString(); else {
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
            deep[deep.Count-1].pos = pos;
            stack.Add(new List<Deep>(deep));
        }
        public void pop()
        {
            if (stack.Count == 0) sys.error("parse: pop no push");
            deep = stack[stack.Count-1];
            stack.RemoveAt(stack.Count-1);
            pos = deep[deep.Count-1].pos;
            now = (more() ? val[pos] : isend);
        }
        public bool lnext()
        {
            string name,s0,s1,st,sf;
            int _np,i0,i1,i2;
            bool l_add = true;
            deep.Clear(); deep.Add(new Deep(isend,isend));
            val = sys.rline(); pos = 0;
            if ((val.Length == 0) || (val[0] == '`')) return false;
            if (val.Substring(0,2) == "##")
            {
                pos = 2; name = "#" + get(isname) + "("; if (! isequnow('#')) sys.error("macro: wrong num");
                next(); if (! isequnow(isname)) sys.error("macro: wrong num");
                _np = m_c_to_n[now];
                string _m = val.Substring(pos + 2);
                for (i0 = 0; i0 < m_n_to_c.Length; i0++)
                {
                    if ((_m.IndexOf("#" + m_n_to_c[i0]) > -1) && (i0 >= _np)) sys.error("macro: used nonparm");
                }
                if (macro.ContainsKey(name)) {
                    if (_np != macro[name].nparm) sys.error("macro: wrong num");
                    macro[name].body += "\n" + _m;
                } else macro.Add(name, new Mbody(_np,_m));
                return false;
            }
            while ((pos = val.LastIndexOf("#")) > -1) {
                sf = val.Substring(0, pos); name = get("(") + "(";
                if (! macro.ContainsKey(name)) sys.error("macro: not found");
                s0 = macro[name].body.Replace("`\n","");
                int ploop = -1,floop = 0,tloop = 0;
                i2 = 0; i1 = deep.Count;
                while (i2 < macro[name].nparm)
                {
                    s1 = _parm();
                    if (isequnow('<') || isequnow('>')) { 
                        l_add = isequnow('<'); next();
                        if (ploop > -1) sys.error("macro: wrong loop");
                        if (! int.TryParse(s1, out floop)) sys.error("macro: wrong loop");
                        if (! int.TryParse(_parm(), out tloop)) sys.error("macro: wrong loop");
                        ploop = i2;
                    } else s0 = s0.Replace("#" + m_n_to_c[i2], s1);
                    if (i1 != deep.Count) sys.error("macro: call nparm");
                    next(); i2++;
                }
                if (i2 == 0) next(); if (! isequnow(')')) sys.error("macro:"); next();
                st = (pos < val.Length ? val.Substring(pos, val.Length - pos) : "");
                if (ploop < 0) val = sf + s0 + st; else {
                    if (l_add) for (s1 = "", i2 = floop; i2 <= tloop; i2++) s1 += s0.Replace("#" + m_n_to_c[ploop], i2.ToString().Trim());
                    else for (s1 = "", i2 = floop; i2 >= tloop; i2--) s1 += s0.Replace("#" + m_n_to_c[ploop], i2.ToString().Trim());
                    val = sf + s1 + st;
                }
            }
            i1 = val.IndexOf("\n");
            if (i1 > -1) {
                sys.addline(val.Substring(i1+1)); val = val.Substring(0,i1);
            }
            pos = 0; now = val[0]; return true;
        }
        public bool more() { return pos < val.Length; }
        public bool isequ(char t, char tst)
        {
            if (tst < isend) {
                int _t = m_c_type[t] - m_c_type_fr[tst];
                return _t >= 0 && _t < m_c_type_sz[tst];
            } else return t == tst;
        }
        public bool isequnow(char tst) { return isequ(now,tst); }
        public bool isequnow(string tst) {
            int i = 0; while (i < tst.Length) {
                if (now != tst[i]) return false;
                next(); i++;
            }
            return true;
        }
        void setnow()
        {
            if (more()) now = val[pos]; else {
                now = isend;
                if (deep.Count != 1) sys.error("parse: nonpair");
            }
        }
        public void next() 
        {
            if (!more()) return;
            int _tp = m_c_type[now];
            if (_tp == 2) oper = now;
            if (_tp == 4) deep.Add(new Deep(now,oper));
            if (_tp == 5) {
                if ((deep.Count < 1) || (deep.Last().pair > now) || (now - deep.Last().pair > 2)) sys.error("parse: nonpair");
                deep.RemoveAt(deep.Count - 1);
            }
            do {pos++;} while(more() && (val[pos] == ' ')); setnow();
        }
        public string get(char tst)
        {
            string ret = ""; int nowdeep = deep.Count;
            while (true) {
                if (isequnow(isend)) return ret;
                if ((nowdeep == deep.Count) && (isequ(now,isclose) || (! isequ(now,tst)))) return ret;
                ret += now.ToString(); next();
            }
        }
        public int get_int()
        {
            int r = 0;
            if (! (isequnow(isnum) && Int32.TryParse(get(isnum),out r))) IDS.sys.error("not int");
            return r;
        }
        public string get(string delim)
        {
            string ret = "";
            if ((delim.Length == 1) && (delim.IndexOf(val[pos]) > -1)) pos++;
            while (true) {
                if ((! more()) || (delim.IndexOf(val[pos]) > -1)) {
                    if (delim.Length == 1) pos++; 
                    setnow(); return ret;
                }
                ret += val[pos].ToString(); pos++;
            }
        }
        public void branch(char t, char[] i, Action[] f)
        {
            bool r = false; 
            for (int i0 = 0; i0 < i.Count(); i0++) {
                if (isequ(t,i[i0])) {
                    r = true; f[i0]();
                }
            }
            if (!r) f[i.Count()]();
        }
        public void branchnow(char[] i, Action[] f)
        {
            branch(now,i,f);
        }
        public void branch(char t, string i, Action[] f) 
        {
            int p = i.IndexOf(t);
            if (p > -1) f[p](); else f[i.Length]();
        }
        public void branchnow(string i, Action[] f) 
        {
            branch(now,i,f);
        }
        Func infunc() {
            Func r;
            string _n = get(isname);
            if (isequnow('(')) {
                if (Program.root.fnames.ContainsKey(_n)) r = fpars(_n,true);
                else {
                    Vars vr = Program.root.find_var(_n);
                    if ((vr.var == null) || (vr.var.type != 7)) IDS.sys.error("not row");
                    next(); if (!isequnow(isabc)) IDS.sys.error("wrong row call");
                    Vals vl = Program.root.find_val(get(isname));
                    if (!isequnow(',')) IDS.sys.error("wrong row call");
                    next(); if (!isequnow(isnum)) IDS.sys.error("wrong row call");
                    int st; Int32.TryParse(get(isnum),out st);
                    r = new Func(((Row)(vr.var.data)).prep_calc(vl,st));
                }
            } else r = new Func(Program.root.find_val(_n));
            return r;
        }
        public KeyValuePair<One,Num> opars()
        {
            bool repeat;
            Func eval = null;
            Func ex = new Func(IDS.n1);
            One ro = new One(); Num rn = new Num(now == '-' ? -1: +1);
            if ((now == '-') || (now == '+')) next();
            bool l = true;
            Action eset = () => {
                if (eval != null) {
                if ((eval.type == 1)  && (ex.type_pow() == 0)) {
                    Num nval = new Num((Num)(eval.data));
                    nval.exp((Num)(ex.data)); rn.mul(nval); 
                } else {
                    if (ro.exps.ContainsKey(eval)) ro.exps[eval].add(ex,1); else ro.exps.Add(eval,ex);
                }
                eval = null; ex = new Func(IDS.n1);
                }
            };
            char[] pc = {isabc,isnum,'{','('};
            Action[] pf = {
                      () => { 
                          ex.mul(new Func(new One(infunc()),IDS.n1));
                      },
                      () => ex.mul(new Num(get(isnum))),
                      () => ex.mul(calc()),
                      () => ex.mul(fpars("",true)),
                      () => sys.error("noNum in calc")
                          };
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
            char[] nc = {isabc,isnum,'{','('};
            Action[] nf = {
                () => {eval = infunc();},
                () => {eval = new Func(new Num(get(isnum)));},
                () => {eval = new Func(calc());},
                () => {eval = fpars("",true);},
                () => sys.error("noNum in calc")
                };
            char[] oc = {isoper,isclose,isend};
            Action[] of = {
                () => branchnow("+-*/^",oof),
                () => {l = false; },
                () => {l = false; },
                () => sys.error("noNum in calc")
                };
            while (l) {
                branchnow(nc,nf);
                do {
                    repeat = false;
                    branchnow(oc,of);
                } while (repeat);
            }
            eset();
            rn.mul(ro.simple()); return new KeyValuePair<One,Num>(ro,rn.simple());
        }
        public Many mpars()
        {
            Many m = new Many();
            KeyValuePair<One,Num> on;
            int d = deep.Count;
            while ((! isequnow(isclose)) && (!isequnow(isend))) {
                on = opars(); m.add(on.Key,on.Value,1);
            }
            if (d < deep.Count) next();
            return m;
        }
        public Many2 m2pars()
        {
            return new Many2(mpars(),new Many(IDS.n1));
        }
        public Func fpars(string _fn, bool _pair)
        {
            int tp, d = deep.Count;
            Func r = null;
            if (! Program.root.fnames.ContainsKey(_fn)) sys.error("parse: func");
            tp = Program.root.fnames[_fn];
            if (_pair) { if (isequnow('(')) next(); else sys.error("parse: func"); }
            if (tp == 7) {
                    r = new Func(new Row());
                    if (! isequnow(isnum)) sys.error("parse:row");
                    ((Row)(r.data)).point = get_int();
                    if (! isequnow(',')) sys.error("parse:row");
                    next();
                    int i = 0; while(true) {
                        if (! isequnow('(')) sys.error("parse:row");
                        next(); ((Row)(r.data)).data.Add(i,m2pars()); next();
                        if (! isequnow(',')) break;
                        next(); i++;
                    }
            } else r = new Func(tp,m2pars());
            if (d < deep.Count) next();
            r.simple(); return r;
        }
        public Num calc()
        {
            char[] lo = {'+', ' ', ' ', ' '};
            Num[] ln = {new Num(0),new Num(0),new Num(0),new Num(0)};
            int lp = 0;
            if (isequnow(isnum)) {
                Num r = new Num(get(isnum));
                if (isequnow(',')) next();
                return r;
            }
            if (isequnow(ispair)) next(); 
            if ((now == '+') || (now == '-')) {lo[0] = now; next();}
            bool l = true;
            Action[] fn = {
                      () => ln[lp-1].add(ln[lp]),() => ln[lp-1].sub(ln[lp]),
                      () => ln[lp-1].mul(ln[lp]),() => ln[lp-1].div(ln[lp]),
                      () => ln[lp-1].exp(ln[lp]),
                      () => sys.error("noNum in calc")
                     };
            char[] nc = {isopen,isnum};
            Action[] nf = {
                              () => ln[++lp].set(calc()),
                              () => ln[++lp].set(get(isnum)),
                              () => sys.error("in calc")
                          };
            char[] oc = {isoper,isclose,isend};
            Action[] of = {
                              () => next(),
                              () => {l = false; next();},
                              () => {l = false;},
                              () => sys.error("in calc")
                          };
            while (l) {
                branchnow(nc,nf);
                lo[lp] = now;
                branchnow(oc,of);
                while((lp > 0) && (m_c_prior[lo[lp-1]] >= m_c_prior[lo[lp]])) {
                    branch(lo[lp-1],"+-*/^",fn); lo[lp-1] = lo[lp]; lp--;
                }
            }
            return ln[0].simple();
        }
        public void Dispose()
        {
            sys.Dispose();
        }

        public string print(Num v, bool plus, bool minus, int pair) //0 - none, 1 - sign, 2 - div
        {
            string ret = ""; bool s = false, d = false;
            if ((v.sign < 0) && minus) {s = true; ret = "-";}
            if ((v.sign > 0) && plus) {s = true; ret = "+";}
            ret += v.up.ToString().Trim();
            if (! v.isint()) {d = true; ret += "/" + v.down.ToString().Trim();}
            if (((pair > 0) && s) || ((pair > 1) && d)) ret = "(" + ret + ")";
            return ret;
        }
        public string print(Num v, int b, int a)
        {
            string s; if (a < 0) a = IDS.prec10;
            Num _v = new Num(v); _v.sign = 1;
            BigInteger nn = _v.toint();
            s = (v.sign < 0 ? "-" : (v.sign > 0 ? "+" : " ")) + nn.ToString().Trim() + ".";
            s =  (b > 0 ? s.PadLeft(b): s);
            if (a > 0) {
                Num tn = Num.sub(_v,new Num(nn)); tn.add(1); tn.mul(IDS.e10[a]);
                s += tn.toint().ToString().Trim().Substring(1);
            }
            return s;
        }
        public string print(Num v, int b)
        {
            return print(v,b,IDS.prec10);
        }
        public string print(One o, Num n)
        {
            string ret = "", s;
            bool first = true, f1 = false, neg = false;
            int ptyp;
            foreach(KeyValuePair<Func,Func> m in o.exps) {
                ptyp = m.Value.type_pow();
                if ((ptyp > 1) || (((Num)(m.Value.data)).sign != 0)) 
                {
                    if ((ptyp < 2) && (((Num)(m.Value.data)).sign < 0)) neg = true; else neg = false;
                    if (first) {
                        s = print(n,true,true,0);
                        if ((s == "+1") || (s == "-1")) {
                            if  (! neg) {
                                ret += s[0]; f1 = true;
                            } else ret += s;
                        } else ret += s;
                    }
                    ret += (neg ? "/" : (f1 ? "" : "*")) + print(m.Key,false);
                    if ((ptyp > 0) || (((Num)(m.Value.data)).up > 1)) ret += "^" + print(m.Value,true);
                }
                first = false; f1 = false;
            }
            if (first) ret += print(n,true,true,0);
            return ret;
        }
        public string print(Many m)
        {
            string ret = "";
            foreach(KeyValuePair<One,Num> o in m.data) {
                ret += print(o.Key, o.Value);
            }
            return ret;
        }
        public string print(Many2 m)
        {
            string ret = "";
            if (Num.isint(m.down.get_Num(),1)) {
                ret += print(m.up);
            } else {
                ret += "(" + print(m.up) + ") / (" + print(m.down) + ")";
            }
            return "(" + ret + ")";
        }
        public string print(Func f, bool inpow)
        {
            string ret = "";
            if (f == null) return ret;
            Action[] p = {
                  () => {
                      ret = ((Vals)(f.data)).get_name();
                  },
                  () => {
                      ret = print((Num)(f.data),false,! inpow,2);
                  },
                  () => {ret = print((Many2)(f.data));},
                  () => {ret = print((Many2)(f.data));},
                  () => {ret = print((Many2)(f.data));},
                  () => {ret = print((Many2)(f.data));},
                  () => {ret = print((Many2)(f.data));},
                  () => {
                      ret = "(" + ((Row)(f.data)).point.ToString();
                      foreach(KeyValuePair<int,Many2> m in ((Row)(f.data)).data) {
                          ret += "," + print(m.Value);
                      }
                      ret += ")";
                  }
                 };
            p[f.type]();
            return Program.root.funcs_name[f.type] + ret;
        }
        public string print(Vars v)
        {
            return v.name + " = " + (v.var == null ? "" : print(v.var,false));
        }
    }


    class Fdim 
    {
        public Vars var;
        public Num now,from,step,to;
        public Fdim(Vars v, Num n)
        {
            var = v;from = n; now = new Num(n); step = IDS.n0; to = IDS.n0;
        }
        public Fdim(Vars v, Num f, Num s, Num t)
        {
            var = v; from = f; now = new Num(f); step = s; to = t;
            Num _t = Num.sub(to,from);
            if ((_t.sign == 0) || (_t.sign != step.sign)) IDS.sys.error("fcalc: wrong direction");
        }
        public bool next()
        {
            bool r = false;
            if (step.sign != 0) {
                now = Num.add(now,step);
                if (to.great(now)) now.set(from); else r = true;
            }
            return r;
        }
    }
    class Ftoint
    {
        Num nfr, ndiv;
        int ifr, isiz;
        public Ftoint(Num _nfr, Num _n, int _ifr, int _i, bool siz)
        {
            nfr = new Num(_nfr); ifr = _ifr;
            if (siz) {
                ndiv = new Num(_n); isiz = _i;
            } else {
                ndiv = Num.sub(_n,_nfr); isiz = _i - _ifr;
            }
            ndiv.div();
        }
        public int get(Num n)
        {
            Num t = Num.sub(n,nfr);
            if (ndiv.sign != t.sign) return ifr;
            t.mul(ndiv);
            if (IDS.n1.great(t)) return ifr + isiz;
            t.mul(isiz);
            return ifr + (int)(t.toint());
        }
    }
    class Fdo
    {
        int type,parm;
        string sb,sa;
        public Vals x,y,r,g,b;
        public Ftoint tx,ty,tr,tg,tb;
        public int ir,ig,ib;
        public Fdo(int p, string b, string a, Vals v, int i0, int i1)
        {
            type = 0; parm = p; sb = b; sa=a;  x = v; ir = i0; ig = i1;
        }
        public Fdo(Vals _x, Ftoint _tx, Vals _y, Ftoint _ty, int _ir, int _ig, int _ib)
        {
            type = 1;
            x = _x; tx = _tx; y = _y; ty = _ty;
            r = null; g = null; b = null; tr = null; tg = null; tb = null; ir = _ir; ig = _ig; ib = _ib;
        }
        public void doit()
        {
            switch(type) {
                case 0: //print x"str" to #parm
                    IDS.sys.wstr(parm,sb + (x == null ? "" : (ir < 0 ? IDS.par.print(x.get_val(),true,true,0) : (ir == 0 ? x.get_val().toint().ToString().Trim() : IDS.par.print(x.get_val(),ir,ig)))) + sa);
                    break;
                case 1:
                    int _x = tx.get(x.get_val());
                    int _y = ty.get(y.get_val());
                    Program.bm1.SetPixel(_x,_y, Color.FromArgb(
                        (r == null ? ir : tr.get(r.get_val())),
                        (g == null ? ig : tg.get(g.get_val())),
                        (b == null ? ib : tr.get(b.get_val()))));
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
    }
    static class Program
    {
        public static Shard0 m0;
        public static System.Drawing.Bitmap bm1;
        public static Parse par;
        public static IDS root;


        [STAThread]
        static int Main(string[] args)
        {
            int sx=0, sy=0, step = 4, exp = 11;
            if (args.Length < 1) return 0;
            string ss = (args.Length < 2 ? "" : args[1]);
            Fileio _f = new Fileio(args[0], ss);

            par = new Parse(_f);
            par.lnext(); sx = par.get_int(); par.next(); sy = par.get_int(); par.next(); step = par.get_int(); par.next(); exp = par.get_int();
            if ((sx < 100) || (sx > 2000) || (sy < 100) || (sy > 2000) || (step < 4) || (step > 11) || (exp < 11) || (exp > 6666)) _f.error("wrong head");
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            m0 = new Shard0(sx, sy);
            bm1 = new System.Drawing.Bitmap(sx, sy);
            for (int i0 = 0; i0 < sx; i0++) for (int i1 = 0; i1 < sy; i1++) bm1.SetPixel(i0, i1, Color.FromArgb(0, 0, 0));
            root = new IDS(step, exp, par.sys,par);
            par.lnext(); root.v_pi = root.findadd_var(par.get(Parse.isname)); root.v_pi.var = new Func(IDS.n_pi);
            par.next(); root.v_e = root.findadd_var(par.get(Parse.isname)); root.v_e.var = new Func(IDS.n_e);
            par.next(); root.v_ln2 = root.findadd_var(par.get(Parse.isname)); root.v_ln2.var = new Func(IDS.n_ln2);
            par.next(); root.v_x = root.findadd_var(par.get(Parse.isname)); par.next(); root.v_n = root.findadd_var(par.get(Parse.isname));
            IDS.v_res = 5;

            Thread calc = new Thread(doit);
            calc.Start();
            Application.Run(m0);

            return 0;
        }
        struct calc_out {
            public readonly string str;
            public readonly int val,nout;
            public calc_out(string s, int v, int o) {
                str = s; val = v; nout = o;
            }
        };
        static Ftoint d_toint(List<Fdim> fdim, Vals v, int fr, int sz) {
            if (par.isequnow('{')) return new Ftoint(par.calc(),par.calc(),fr,fr+sz,false);
            else {
                foreach (Fdim fd in fdim) if ((fd.var == v.var) && (fd.step.sign != 0)) return new Ftoint(fd.from, fd.to,fr,fr+sz,false);
                IDS.sys.error("draw: no interval");
            }
            return null;
        }
        static List<Func> d_vals(Vars v)
        {
            List<Func> r = new List<Func>();
            One o = new One();
            v.var.findvals(o);
            foreach(KeyValuePair<Func,Func> m in o.exps) r.Add(m.Key);
            return r;
        }
        static string[] str_proc(string s)
        {
            List<string> r = new List<string>(); string t0,t1, ac;
            ac = ""; int i1,i0 = 0; while (i0 < s.Length) {
                if ((i1 = s.IndexOf('%',i0)) < 0) break;
                t0 = s.Substring(i0,i1-i0);
                t1 = s.Substring(i1+1) + " ";
                    switch (t1[0]) {
                        case '%': t0 += "%"; i1++; break;
                        case '\'': t0 += '"'; i1++; break;
                        case 'n': t0 += '\n'; i1++; break;
                        default:
                            r.Add(ac+t0); ac = ""; t0 = "";
                            break;
                    }
                ac += t0; i0 = i1+1;
            }
            r.Add(ac + s.Substring(i0));
            return r.ToArray();
        }
        static char[] spl = {'.'};
        static void doit() {
            Vars var0;
            Vals val0,val1;
            List<Func> id;
            string val,name;
            bool wasdraw = false;
            DateTime ddnow = DateTime.Now;
 
 
            while (par.sys.has)
            {
                if (par.lnext()) 
                {
                    if (par.isequnow(Parse.isabc)) {
                        name = par.get(Parse.isname);
                        switch (par.now) 
                        {
                            case '=':
                                if (root.fnames.ContainsKey(name)) IDS.sys.error("reserved name");
                                var0 = root.findadd_var(name);
                                if (var0.ind < IDS.v_res) IDS.sys.error("reserved var");
                                par.next();
                                var0.var = (par.isequnow(Parse.isend) ? null : par.fpars("",false));
                                par.sys.wline(0,par.print(var0));
                            break;
                            case '$':
                                bool _div = false, _exp0 = false, _fast = false;
                                var0 = root.find_var(name);
                                if (var0.var == null) par.sys.error("empty name");
                                par.next();
                                MAO_dict mao = null;
                                if (par.isequnow('!'))
                                {
                                    _fast = true; par.next(); mao = new MAO_dict(par.get_int()); par.next();
                                }
                                if (par.isequnow('*')) {
                                    id = d_vals(var0); par.next();
                                } else {
                                    id = new List<Func>();
                                    while (par.isequnow(Parse.isname)) {
                                        val = par.get(Parse.isname); 
                                        if (par.isequnow(',')) par.next();
                                        val0 = root.find_val(val);
                                        if (val0.var == var0) par.sys.error(name + " $recursion - look recursion");
                                        if (val0.var.var != null) id.Add(new Func(val0));
                                    }
                                }
                                if (par.isequnow('@')) {
                                    _exp0=true; par.next();
                                    if (_fast) IDS.sys.error("cant @ on !");
                                }
                                if (par.isequnow('$')) {_div=true; par.next();}
                                foreach (Func i in id) {
                                    var0.var.revert(i);
                                }
                                if (_fast) mao.val(new Func(var0.vals[0]));
                                foreach (Func i in id) {
                                    if (_fast) mao.expand(0,mao.val(i)); else var0.var.expand(i);
                                }
                                if (_exp0) var0.var.expand();
                                if (_fast) var0.var = mao.mao[0].to_func();
                                else var0.var.simple();
                                if (_div && ((var0.var.type == 2) && (((Many2)(var0.var.data)).down.type_exp() < 2)))
                                {
                                    ((Many2)(var0.var.data)).down.div();
                                    ((Many2)(var0.var.data)).up.mul(((Many2)(var0.var.data)).down.data.ElementAt(0).Key,((Many2)(var0.var.data)).down.data.ElementAt(0).Value);
                                    ((Many2)(var0.var.data)).down = new Many(IDS.n1);
                                }
                                par.sys.wline(0,par.print(var0));
                            break;
                            case '<':
                                var0 = root.find_var(name);
                                if (var0.var == null) par.sys.error("empty name");
                                par.next();
                                val0 = root.find_val(par.get(Parse.isname));
                                var0.var.diff_down(val0);
                                par.sys.wline(0,par.print(var0));
                                break;
                        }
                    } else {
                        switch (par.now) 
                        {
                            case '[':
                                par.next();
                                List<Fdim> fdim = new List<Fdim>();
                                List<Fdo> fdo = new List<Fdo>();
                                do {
                                    if (! par.isequnow(Parse.isabc)) IDS.sys.error("calc: wrong");
                                    var0 = root.find_var(par.get(Parse.isname));
                                    foreach (Fdim fd in fdim) if (fd.var == var0) IDS.sys.error("calc: double");
                                    switch (par.now) 
                                    {
                                        case '[':
                                            par.next(); 
                                            fdim.Add(new Fdim(var0,par.calc(),par.calc(),par.calc()));
                                            par.next();
                                            break;
                                        case '{':
                                            fdim.Add(new Fdim(var0,par.calc()));
                                            break;
                                        default:
                                            IDS.sys.error("calc: wrong");
                                            break;
                                    }
                                    if (par.now == ']') break;
                                    par.next();
                                } while (true);
                                par.next();
                                while(par.more()) {
                                    if (par.isequnow(Parse.isabc)) {
                                        val0 = root.find_val(par.get(Parse.isname));
                                        if (! par.isequnow('"')) IDS.sys.error("wrong do");
                                        String[] sp = str_proc(par.get("\""));
                                        if (sp.Length != 3) IDS.sys.error("wrong do");
                                        int _a = -1, _b = 0;
                                        if (sp[1] == "i") _a = 0; else {
                                            String[] _sp = sp[1].Split(spl);
                                            if (_sp.Length == 2) {
                                                Int32.TryParse(_sp[0], out _a); if (_sp[1].Length > 0) Int32.TryParse(_sp[1], out _b); else _b = -1;
                                            }
                                        }
                                        fdo.Add(new Fdo(par.get_int(),sp[0],sp[2],val0,_a,_b));
                                    } else switch (par.now) {
                                        case '"':
                                            val = par.get("\"");
                                            fdo.Add(new Fdo(par.get_int(),str_proc(val)[0],"",null,-1,0));
                                            break;
                                        case '[':
                                            int _fx,_sx,_fy,_sy;
                                            par.next(); wasdraw = true;
                                            if (par.isequnow('[')) {
                                                par.next(); _fx = par.get_int();
                                                par.next(); _sx = par.get_int();
                                                par.next(); _fy = par.get_int();
                                                par.next(); _sy = par.get_int();
                                                par.next();
                                                if ((_sx < 4) || (_sy < 4) || (_fx + _sx >= m0.sx)  || (_fy + _sy >= m0.sy)) IDS.sys.error("draw: wrong");
                                            } else {
                                                _fx = 0; _sx = m0.sx-1; _fy = 0; _sy = m0.sy-1;
                                            }
                                            val0 = root.find_val(par.get(Parse.isname));
                                            Ftoint ftx = d_toint(fdim,val0,_fx,_sx);
                                            if (! par.isequnow(',')) IDS.sys.error("draw:wrong"); par.next();
                                            val1 = root.find_val(par.get(Parse.isname));
                                            Ftoint fty = d_toint(fdim,val1,_fy,_sy);
                                            Fdo _fd = new Fdo(val0,ftx,val1,fty,0,0,0);
                                            if (! par.isequnow(',')) IDS.sys.error("draw:wrong"); par.next();
                                            if (par.isequnow(Parse.isnum)) _fd.ir = par.get_int(); else {
                                                _fd.r = root.find_val(par.get(Parse.isname));
                                                _fd.tr = d_toint(fdim,_fd.r,0,255);
                                            }
                                            if (! par.isequnow(',')) IDS.sys.error("draw:wrong"); par.next();
                                            if (par.isequnow(Parse.isnum)) _fd.ig = par.get_int(); else {
                                                _fd.g = root.find_val(par.get(Parse.isname));
                                                _fd.tg = d_toint(fdim,_fd.g,0,255);
                                            }
                                            if (! par.isequnow(',')) IDS.sys.error("draw:wrong"); par.next();
                                            if (par.isequnow(Parse.isnum)) _fd.ib = par.get_int(); else {
                                                _fd.b = root.find_val(par.get(Parse.isname));
                                                _fd.tb = d_toint(fdim,_fd.b,0,255);
                                            }
                                            par.next(); fdo.Add(_fd);
                                            break;
                                    }
                                    if (par.isequnow(',')) par.next();
                                }
                                bool w; m0.rp = false;
                                do {
                                    root.uncalc(); foreach (Fdim fd in fdim) fd.var.set_now(fd.now);
                                    foreach(Fdo fd in fdo) fd.doit();
                                    w = false;
                                    foreach (Fdim fd in fdim) {
                                        if (w = fd.next()) break;
                                    }
                                } while (w);
                                m0.Set(1); m0.Set(2); m0.rp = true;
                                break;
                        }
                    }
                }
            }
            TimeSpan ime =  DateTime.Now - ddnow;
            par.sys.wline(0,"finished, vars = " + (root.var.Count()).ToString() + "; time has " + ime.ToString());
            par.sys.close();
            if (! wasdraw) Environment.Exit(-1);
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
