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
            data.Add(Program.root.nums[IDS.znums],Program.root.nums[IDS.znums+1]);
            data.Add(Program.root.nums[IDS.znums+1],v);
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
                    if (deep == 0) Program.root.sys.error(var.name + " recursion");
                } else {
                    if (var.var == null) Program.root.sys.error(var.name + " var: non is non");
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
        public int stat_uncalc, stat_calc, exp_prec;
//        public Num prec;
        public BigInteger exp_max;
        public SortedDictionary<string,Vars> var;
        public Vars var0,var1;
        public Fileio sys;
        public Func fzero;
        public One ozero;
        public const int znums = 100;
        public Num[] nums, e10;
        public string[] funcs_name = {"","","","row","fact","int","sign"};
        public SortedDictionary<string,int> fnames;
        public Num[] fact;
        public IDS(BigInteger e, Fileio f)
        {
            int i;
            sys = f; stat_uncalc = 1; stat_calc = 2;
            fnames = new SortedDictionary<string,int>();
            var = new SortedDictionary<string,Vars>();
            exp_max = e;
            exp_prec = (int)(BigInteger.Log(e,10)/2);
            ozero = new One();
            nums = new Num[IDS.znums*2];
            i = 0; while (i < IDS.znums*2) { nums[i] = new Num(i-IDS.znums); i++; }
            fzero = new Func(nums[IDS.znums]);
            i = 2; while (i < funcs_name.Count()) { fnames.Add(funcs_name[i],i); i++; }
            fact = new Num[1000];
            BigInteger b = new BigInteger(1), c = new BigInteger(1); fact[0] = new Num(b);
            while (b < fact.Count()) {
                c *= b; fact[(int)b] = new Num(c); b++;
            }
            e10 = new Num[1000]; b = 1; i = 0;
            while (i < e10.Count()) {
                e10[i] = new Num(b); b *= 10; i++;
            }
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
    }

    class Num : Power<Num>, IPower, IComparable
    {
        public BigInteger up, down;
        static BigInteger prec_base = 10;
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
            if (n0.sign != n1.sign) return Program.root.nums[IDS.znums];
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
        public void set(string s)
        {
            down = 1;
            BigInteger.TryParse(s, out up);
            sign = (up > 0 ? 1 : 0);
        }
        private Num(int _s, BigInteger _u, BigInteger _d)
        {
            sign = _s; up = _u; down = _d;
        }

        public void set(BigInteger _u)
        {
            if (_u < 0) { sign = -1; up = -_u; } else { sign = 1; up = _u; }
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
                    if ((_d == 1) && (_u < IDS.znums)) return Program.root.nums[IDS.znums + (int)(_u)*sign];
                    else return new Num(sign,_u,_d);
                }
            } else if (down == 0) Program.root.sys.error("div0");
            if ((down == 1) && (up < IDS.znums)) return Program.root.nums[IDS.znums + (int)(up)*sign];
            else return this;
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
        {
            if (sign == 0) set(a,s); else {
            if (sign * (a.sign * s) < 0)
            {
                up = up * a.down - a.up * down;
            }
            else
            {
                up = up * a.down + a.up * down;
            }
            down *= a.down;
            if (up < 0) { up = -up; sign = -sign; } else if (up == 0) { sign = 0; down = 1; }
            }
        }
        public void add(Num a) { add(a, 1); }
        public void sub(Num a) { add(a, -1); }
        void prec(int l)
        {
            double lu, ld; int _l;
            lu = BigInteger.Log(up,10);
            ld = BigInteger.Log(down,10);
            if (lu > ld) lu = ld;
            _l = (int)lu - l; if (_l > 1)
            {
                BigInteger _d, _au, _ad, _bu, _bd, _cu, _cd; 
                _d = BigInteger.Pow(prec_base,_l);
                _au = up / _d; _bu = up % _d; _cu = _bu / _au;
                _ad = down / _d; _bd = down % _d; _cd = _bd / _ad;
                _d += (_cu + _cd) / 2;
                up /= _d; down /= _d;
            }
        }

        public BigInteger _sq(BigInteger a, int b)
        {
            BigInteger r0=0,r1;
            if (a>0) {
                r1 = BigInteger.Pow((BigInteger)(b), (int)(BigInteger.Log(a, (double)b) / b + 0.5)); r0 = 0;
                while (r0 != r1) { r0 = r1; r1 = r0 - (BigInteger.Pow(r0, b) - a) / BigInteger.Pow(r0, b-1) / b; }
            }
            return r0;
        }
        public bool root(int s)
        {
            if (s < 2) return true;
            if ((sign < 0) && (s % 2 == 0)) return false;
            BigInteger u = _sq(up,s), d = _sq(down,s);
            if ((BigInteger.Pow(u, s) == up) && (BigInteger.Pow(d, s) == down))
            {
                up = u; down = d; return true;
            }
            return false;
        }
        public static Num exp(Num ex, Num p)
        {
            Num r = new Num(ex); r.exp(p); return r;
        }
        public static Num exp(Num ex, int p)
        {
            Num r = new Num(ex); r.exp(p); return r;
        }
        public void exp(Num ex)
        {
            int u,d;
            if (ex.up == 0) { set1(); return; }
            if ((ex.up > Program.root.exp_max) || (ex.down > Program.root.exp_max)) {
                Num e1 = new Num(ex);
                e1.prec(Program.root.exp_prec);
                if (e1.down > Program.root.exp_max) { set1(); return; }
                if (e1.up > Program.root.exp_max) Program.root.sys.error("exp too large");
                u = (int)e1.get_sup(); d = (int)e1.down;
            } else {u = (int)ex.get_sup(); d = (int)ex.down;}
            exp(u);
            if (! root(d)) Program.root.sys.error("sqr: wrong");
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
            exps.Add(new Func(f),new Func(Program.root.nums[IDS.znums+1]));
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

        public void addto(Func val,Func exp)
        {
            if (exps.ContainsKey(val)) exps[val].add(exp,1); else exps.Add(new Func(val),new Func(exp));
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
            exp(Program.root.nums[IDS.znums+e]);
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
            foreach(KeyValuePair<Func,Func> m in exps) if ((m.Key.type == 2) && (m.Value.type_pow() == 0))
            {
                if (r == null) r = new Many2(1);
                t = new Many2((Many2)(m.Key.data)); t.exp(m.Value); r.mul(t);
            } else o.addto(m.Key,m.Value);
            if (r != null) r.mul(o,n);
            return r;
        }
        public bool expand_p0(Func val, Exps_f exu, Exps_f exd)
        {
            bool rt = false;
            foreach(KeyValuePair<Func,Func> m in exps) if (m.Value.CompareTo(val) == 0)
            {
                rt = true; m.Value.type = 2; m.Value.data = new Many2(new Many(exu.mvar), new Many(exd.mvar));
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
                SortedDictionary<Func,Func> r = new SortedDictionary<Func,Func>();
                foreach(KeyValuePair<Func,Func> m in exps) 
                    if ((! m.Value.isconst(0)) && (! m.Key.isconst(1))) r.Add(m.Key,m.Value);
                exps = r;
            }
            return fv || fk;
        }
//        static public Action<Func,Func,func,num>[] reple = {
//              (Func e, Func p, Func f, , Num n) => {},

        public void replace(Vals v, Func f) {
            foreach(KeyValuePair<Func,Func> ff in exps) {
                ff.Key.replace(v,f); ff.Value.replace(v,f);
            }
        }

        public Num simple()
        {
            Num rt = new Num(1);
            SortedDictionary<Func,Func> r = new SortedDictionary<Func,Func>();
//            KeyValuePair<Func,Func> ff;
//            for(int i = 0; i < exps.Count; i++) {
//                ff = exps.ElementAt(i);
            foreach(KeyValuePair<Func,Func> ff in exps) {
                ff.Value.simple();
                if (! ff.Value.isconst(0)) {
                    ff.Key.simple();
                    if ((ff.Key.type == 1) && (ff.Value.type_pow() == 0)) rt.mul(Num.exp((Num)(ff.Key.data),(Num)(ff.Value.data)));
                    else r.Add(ff.Key, ff.Value);
                }
            }
            exps = r;
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
            data.Add(new One(f),Program.root.nums[IDS.znums+1]);
        }
        public Many(One o)
        {
            data = new SortedDictionary<One,Num>();
            data.Add(o,Program.root.nums[IDS.znums+1]);
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
            data.Add(new One(), Program.root.nums[IDS.znums]);
        }
        public override void set1()
        {
            data.Clear();
            data.Add(new One(), Program.root.nums[IDS.znums+1]);
        }
        public override void div()
        {
            if (data.Count != 1) Program.root.sys.error("cant divide many");
            SortedDictionary<One,Num> r = new SortedDictionary<One,Num>();
            One o = data.ElementAt(0).Key; Num n = new Num(data.ElementAt(0).Value);
            o.div(); n.div(); r.Add(o,n); data = r;
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
                if (n0 ^ n1) return (n0 ? o0.Current.Value.CompareTo(Program.root.nums[IDS.znums]) : Program.root.nums[IDS.znums].CompareTo(o1.Current.Value));
                else {
                    if (! n0) return 0;
                    if ((r = o0.Current.Key.CompareTo(o1.Current.Key)) == 0) {
                        if ((r = o0.Current.Value.CompareTo(o1.Current.Value)) != 0) return r;
                    } else {
                        if (r > 0) return o0.Current.Value.CompareTo(Program.root.nums[IDS.znums]);
                        else return Program.root.nums[IDS.znums].CompareTo(o1.Current.Value);
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
                data[o] = Num.add(data[o],n,s).simple();
            } else data.Add(new One(o),new Num(n,s));
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
            for (int _i = 0; _i < r.Count; _i++) r.ElementAt(_i).Value.simple();
            data = r;
        }
        public void mul(Num n)
        {
            for (int _i = 0; _i < data.Count; _i++) data[data.ElementAt(_i).Key] = Num.mul(data.ElementAt(_i).Value,n);
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
            foreach (KeyValuePair<One,Num> o in data) 
                rt = o.Key.expand_p0(val, exu, exd) || rt;
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
            if (te + tp > 2) Program.root.sys.error("exp: cant Many to many");
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
                } else {ch = true; data[on.Key] = Program.root.nums[IDS.znums];}
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
            down.data.Add(new One(), Program.root.nums[IDS.znums+1]);
        }
        public Many2(int n)
        {
            up = new Many(); down = new Many();
            up.data.Add(new One(), Program.root.nums[IDS.znums+n]);
            down.data.Add(new One(), Program.root.nums[IDS.znums+1]);
        }
        public Many2(Num n)
        {
            up = new Many(); down = new Many();
            up.data.Add(new One(), n);
            down.data.Add(new One(), Program.root.nums[IDS.znums+1]);
        }
        public Many2(One o, Num n)
        {
            up = new Many(); down = new Many();
            up.data.Add(new One(o), n);
            down.data.Add(new One(), Program.root.nums[IDS.znums+1]);
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
            up.data.Add(new One(), Program.root.nums[IDS.znums]);
            down.data.Add(new One(), Program.root.nums[IDS.znums+1]);
        }
        public override void set1()
        {
            up.data.Clear(); down.data.Clear();
            up.data.Add(new One(), Program.root.nums[IDS.znums+1]);
            down.data.Add(new One(), Program.root.nums[IDS.znums+1]);
        }
        public override void div()
        {
            Many t;
            if ((up.data.Count == 1) && (up.data.ElementAt(0).Value.sign == 0)) Program.root.sys.error("div0");
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
            Many _up = up, _dw = down;
            KeyValuePair<One,Num> f_up, f_down, f_both;
            up.simple();
            down.simple();
            f_up = up.extract();
            f_down = down.extract();
            f_both = new KeyValuePair<One,Num>(new One(f_up.Key), new Num(f_up.Value));
            f_both.Key.extract(f_down.Key);
            f_both.Value.extract(f_down.Value);
            f_both.Key.div(); f_both.Value.div();
            up.mul(f_both.Key,f_both.Value);
            down.mul(f_both.Key,f_both.Value);

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
            up.simple();
            down.simple();
            Func t = up.get_Func();
            if ((t != null) && (t.type == 2) && ((Many2)(t.data)).down.isint(1)) up = ((Many2)(t.data)).up;
            t = down.get_Func();
            if ((t != null) && (t.type == 2) && ((Many2)(t.data)).down.isint(1)) down = ((Many2)(t.data)).up;
            Num nd = down.get_Num();
            if (nd == null) return null;
            Num nu = up.get_Num();
            if (nu != null) {
                nd.div(); nu.mul(nd); return nu;
            }
            return null;
        }
        public Num calc() {
            Num d = down.calc();
            if (d.sign == 0) Program.root.sys.error("div0");
            d.div(); d.mul(up.calc()); return d.simple();
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
            Func e0 = new Func(Program.root.nums[IDS.znums]);
            data.Add(e0, new Many());
            data[e0].data.Add(new One(),new Num(1));
            deep = _deep;
            e1 = new Func(Program.root.nums[IDS.znums+1]);
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
            Func dl = new Func(Program.root.nums[IDS.znums]);
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
            r.replace(Program.root.var0.vals[0],new Func(val));
            r.replace(Program.root.var1.vals[0],new Func(Program.root.nums[IDS.znums+exp]));
            Num n = r.simple();
            if (n != null) return new Many2(n); else return r;
        }
        public Many2 prep_calc(Vals val, int steps)
        {
            int len = data.Count - point;
            if (len < 1) Program.root.sys.error("row: empty");
            Many2 calc = new Many2(Program.root.nums[IDS.znums]), t;
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

    }

    class Func: Power<Func>, IPower, IComparable
    {
        public const int types = 7;
        public Object data; //SortedDictionary<int,many> data;
        public int type; //0: &val, 1: &Num {imutable}, 2: &many2, 3: row(many2[]), 4: fact(Many2), 5: int(Many2), 6: sign(Many2)
        public Func()
        {
            type = -1; data = null;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Vals v)
        {
            type = 0; data = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Num n)
        {
            type = 1; data = n;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(One o, Num n)
        {
            type = 2; data = new Many2(o,n);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Row r)
        {
            type = 3; data = r;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func(Many2 m)
        {
            type = 2; data = m;
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
              (Func t, Func f) => {t.data = new Row((Row)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
              (Func t, Func f) => {t.data = new Many2((Many2)(f.data));},
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
            type = 1; data = Program.root.nums[IDS.znums];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void set1()
        {
            type = 1; data = Program.root.nums[IDS.znums+1];
        }
        static public Func<Func,Num>[] get_num_part_func = {
                (Func t) => {return Program.root.nums[IDS.znums];},
                (Func t) => {return (Num)(t.data);},
                (Func t) => {return ((Many2)(t.data)).get_num_part();},
                (Func t) => {return Program.root.nums[IDS.znums];},
                (Func t) => {return Program.root.nums[IDS.znums];},
                (Func t) => {return Program.root.nums[IDS.znums];},
                (Func t) => {return Program.root.nums[IDS.znums];}
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Num get_num_part()
        {
            return Func.get_num_part_func[type](this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isconst(int n) { return (type == 1 ? ((Num)data).isint(n): false); }
        static public Func<Func,int>[] type_pow_func = {
                (Func t) => {return 2;},
                (Func t) => {return ((((Num)(t.data)).isint()) ? 0 : 1);},
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
            if (type == 2) ((Many2)data).revert(val);
        }
        static public Action<Func,Vals,Func>[] repl_func = {
                (Func t, Vals v, Func f) => {
                    if (((Vals)(t.data) == v)) {t.type = f.type; t.data = f.data;}
                },
                (Func t, Vals v, Func f) => {},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);},
                (Func t, Vals v, Func f) => {((Many2)(t.data)).replace(v,f);}
         };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void replace(Vals v, Func f)
        {
            Func.repl_func[type](this,v,f);
        }

        static public Func<Func, Func, Exps_f, Exps_f,bool>[] expand2_func = {
                (Func t, Func v, Exps_f u, Exps_f d) => {return false;},
                (Func t, Func v, Exps_f u, Exps_f d) => {return false;},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Row)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);},
                (Func t, Func v, Exps_f u, Exps_f d) => {return ((Many2)(t.data)).expand(v,u,d);}
            };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool expand(Func val, Exps_f exu, Exps_f exd)
        {
            return Func.expand2_func[type](this, val, exu, exd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool expand(Func fv)
        {
            if (fv.type != 0) return false;
            Vals v = (Vals)(fv.data);
            Func f_exp = v.var.var;
            if ((f_exp == null) || ((f_exp.type != 1) && (f_exp.type != 2))) Program.root.sys.error("wrong expand");
            Exps_f exu,exd;
            if (f_exp.type == 2) {
                exu = new Exps_f(((Many2)(f_exp.data)).up,v.deep);
                exd = new Exps_f(((Many2)(f_exp.data)).down,v.deep);
            } else {
                exu = new Exps_f(new Many(f_exp),0);
                exd = new Exps_f(new Many(Program.root.nums[IDS.znums+1]),0);
            }

            return Func.expand2_func[type](this,fv,exu,exd);
        }
        static public Action<Func>[] expand_func = {
                (Func t) => {},
                (Func t) => {},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Row)(t.data)).expand();},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Many2)(t.data)).expand();},
                (Func t) => {((Many2)(t.data)).expand();}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void expand()
        {
            Func.expand_func[type](this);
        }
        static public Action<Func>[] neg_func = {
                (Func t) => {
                    t.data = new Many2(new Many(new One(t)));
                    t.type = 2; ((Many2)(t.data)).neg();
                },
                (Func t) => {t.data = Num.neg((Num)(t.data));},
                (Func t) => {((Many2)(t.data)).neg();},
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
                (Func t) => {}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void div()
        {
            Func.div_func[type](this);
        }
        static public Action<Func,Func>[] mul_func = {
                (Func t, Func f) => {//0:0
                    One _o = new One(t); _o.addto(f,new Func(Program.root.nums[IDS.znums+1]));
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
                    ((Many2)(t.data)).mul(_o,Program.root.nums[IDS.znums+1]);
                },
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

                (Func t, Func f) => {//2 *= 0
                    ((Many2)(t.data)).mul(new One(f),Program.root.nums[IDS.znums+1]);
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

                (Func t, Func f) => {},//3 *= 0
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

                (Func t, Func f) => {},//5 *= 0
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
                        t.type = 1; t.data = Program.root.nums[IDS.znums];
                    }
                },
                (Func t, Func f) => {
                        t.type = 1; t.data = Program.root.nums[IDS.znums];
                },
                (Func t, Func f) => {
                    One _o = new One(t); 
                    Num _nd = ((Many2)(f.data)).down.get_Num(); _nd.div();
                    if ((_nd != null) && ((Many2)(f.data)).up.data.ContainsKey(_o) && (((Many2)(f.data)).up.data[_o].sign * _nd.sign > 0)) {
                        _nd.mul(((Many2)(f.data)).up.data[_o]);
                        if (_nd.great(Program.root.nums[IDS.znums+1])) {
                            Many _u = new Many(); _u.data.Add(_o,_nd);
                            t.type = 2; t.data = new Many2(_u,new Many(Program.root.nums[IDS.znums+1]));
                        }
                    } else {t.type = 1; t.data = Program.root.nums[IDS.znums];}
                },
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},
                (Func t, Func f) => {},

                (Func t, Func f) => {
                        t.type = 1; t.data = Program.root.nums[IDS.znums];
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

                (Func t, Func f) => {//2:0
                    One _o = new One(f);
                    Num _nd = ((Many2)(t.data)).down.get_Num(); _nd.div();
                    if ((_nd != null) && ((Many2)(t.data)).up.data.ContainsKey(_o) && (((Many2)(t.data)).up.data[_o].sign * _nd.sign > 0)) {
                        _nd.mul(((Many2)(t.data)).up.data[_o]);
                        if (_nd.great(Program.root.nums[IDS.znums+1])) {
                            ((Many2)(t.data)).up = new Many(); ((Many2)(t.data)).up.data.Add(_o,_nd);
                            ((Many2)(t.data)).down = new Many(Program.root.nums[IDS.znums+1]);
                        } else {
                            t.type = 0; t.data = f.data;
                        }
                    } else {t.type = 1; t.data = Program.root.nums[IDS.znums];}
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

                (Func t, Func f) => {}, //3:0
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

                (Func t, Func f) => {}, //5:0
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
                (Func t, Func f) => {}
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void common(Func f)
        {
            Func.common_func[type*Func.types + f.type ](this,f);
        }



        static public Action<Func,Func,int>[] add_func = {
                (Func t, Func f, int s) => {//0:0
                    One _o = new One(t);
                    Many _u = new Many(); _u.data.Add(_o,Program.root.nums[IDS.znums+1]);
                    _o = new One(); _o.exps.Add(new Func(f),new Func(Program.root.nums[IDS.znums+1]));
                    _u.add(_o,Program.root.nums[IDS.znums+1],s);
                    t.type = 2; t.data = new Many2(_u,new Many(Program.root.nums[IDS.znums+1]));                
                },
                (Func t, Func f, int s) => {
                    if (((Num)(f.data)).sign != 0) {
                        One _o = new One(t);
                        Many _u = new Many(); _u.data.Add(_o,Program.root.nums[IDS.znums+1]);
                        _o = new One(); _u.data.Add(_o,(Num)(f.data));
                        t.type = 2; t.data = new Many2(_u,new Many(Program.root.nums[IDS.znums+1]));                
                    }
                },
                (Func t, Func f, int s) => {
                    One _o = new One(t);
                    Many2 _fm = new Many2((Many2)(f.data)), _tm = new Many2(0);
                    _tm.up.data.Add(_o,Program.root.nums[IDS.znums+1]); _tm.add(_fm,s);
                    t.type = 2; t.data = _tm;
                },
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},
                (Func t, Func f, int s) => {},

                (Func t, Func f, int s) => {//1:0
                    if (((Num)(t.data)).sign == 0) {
                        t.type = f.type; t.data = f.data;
                    } else {
                        One _o = new One(f);
                        Many _u = new Many(); _u.data.Add(_o,Program.root.nums[IDS.znums+1]);
                        _o = new One(); _u.add(_o,(Num)(t.data),s);
                        t.type = 2; t.data = new Many2(_u,new Many(Program.root.nums[IDS.znums+1]));                
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

                (Func t, Func f, int s) => {//2:0
                    One _o = new One(f);
                    Many _u = new Many(); _u.data.Add(_o,Program.root.nums[IDS.znums+1]);
                    ((Many2)(t.data)).add(new Many2(_u,new Many(Program.root.nums[IDS.znums+1])),s);
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

                (Func t, Func f, int s) => {}, //3:0
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

                (Func t, Func f, int s) => {}, //5:0
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

                (Func t, Func f) => {return -1;}, //1:0
                (Func t, Func f) => {return ((Num)(t.data)).CompareTo(f.data);},
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
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(null);},

                (Func t, Func f) => {return -1;}, //3:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Row)(t.data)).CompareTo(f.data);},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},
                (Func t, Func f) => {return 1;},

                (Func t, Func f) => {return -1;}, //4:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(f.data);},
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
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(null);},

                (Func t, Func f) => {return -1;}, //6:0
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return -1;},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo((Many2)(f.data));},
                (Func t, Func f) => {return ((Many2)(t.data)).CompareTo(null);}
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
                (Func t) => {((Row)(t.data)).simple();},
                (Func t) => {
                    ((Many2)(t.data)).simple();
                    Num n = ((Many2)(t.data)).get_Num();
                    if (n != null) {
                        t.type = 1; t.data = Func.f_fact(n);
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
                }
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
                (Func t) => {return 1;}
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int sign()
        {
            return Func.sign_func[type](this);
        }


        static public Action<Func,int>[] deeper_func = {
                (Func t, int d) => {
                    int _i; if ((_i = ((Vals)(t.data)).deep + d) >= ((Vals)(t.data)).var.vals.Length) Program.root.sys.error("too deep");
                    t.data = ((Vals)(t.data)).var.vals[_i];
                },
                (Func t, int d) => {},
                (Func t, int d) => {((Many2)(t.data)).deeper(d);},
                (Func t, int d) => {((Row)(t.data)).deeper(d);},
                (Func t, int d) => {((Many2)(t.data)).deeper(d);},
                (Func t, int d) => {((Many2)(t.data)).deeper(d);},
                (Func t, int d) => {((Many2)(t.data)).deeper(d);}
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
        public static Num f_fact(Num n)
        {
            if ((! n.isint()) || (n.sign < 0) || (n.up >= Program.root.fact.Count())) Program.root.sys.error("fact: wrong");
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
            return Program.root.nums[n.sign + IDS.znums];
        }
        static public Func<Func,Num>[] calc_func = {
                (Func t) => {return ((Vals)(t.data)).get_val(); },
                (Func t) => {return (Num)(t.data);},
                (Func t) => {return ((Many2)(t.data)).calc();},
                (Func t) => {
                    Program.root.sys.error("row:not prep");
                    return new Num(0);
                },
                (Func t) => {return Func.f_fact(((Many2)(t.data)).calc());},
                (Func t) => {return Func.f_int(((Many2)(t.data)).calc());},
                (Func t) => {return Func.f_sign(((Many2)(t.data)).calc());}
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
                    Program.root.sys.error("row: not prep");
                    return new Num(0);
                },
                (Func t, Num e) => {
                    Num r = ((Many2)(t.data)).calc();
                    if ((! r.isint()) || (r.sign <= 0) || (r.up >= Program.root.fact.Count())) Program.root.sys.error("fact: wrong");
                    return Num.exp(Program.root.fact[(int)r.up],e);
                },
                (Func t, Num e) => {
                    Num r = ((Many2)(t.data)).calc();
                    if (r.down != 1) {r = new Num(r.sign*r.up/r.down);}
                    return Num.exp(r,e);
                },
                (Func t, Num e) => {
                    int s = ((Many2)(t.data)).calc().sign;
                    return Program.root.nums[((e.up & 1) == 0 ? s*s : s) + IDS.znums];
                }
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Num calc(Num e)
        {
            return (e == Program.root.nums[IDS.znums+1] ? Func.calc_func[type](this) : Func.calce_func[type](this,e));
        }

    }
/*
    class mao_dict {
        static short bmexp = 11;
        static int mexp = 1 << bmexp;
        public int nvals;
        public num[] exps;
        public int[] vals, to_val;
        ushort[] eneg,eadd; bool[] eflg_a;
        ushort lexp, lval;
        public mao_dict(int v)
        {
            nvals = v;
            exps = new num[mexp];
            vals = new int[nvals];
            to_val = new int[Program.root.size()];
            for (int i = 0; i < Program.root.size(); i++) to_val[i] = -1;
            eneg = new ushort[mexp * mexp]; eadd = new ushort[mexp * mexp]; eflg_a = new bool[mexp * mexp];
            for (uint i = 0; i < mexp * mexp; i++) { eneg[i] = 0xFFFF; eadd[i] = 0xFFFF; eflg_a[i] = true; }
            lexp = 2; exps[0] = new Num(0); exps[1] = new Num(1);
            lval = 0;
        }
        public ushort exp(Num e)
        {
            ushort i;
            for (i = 0; i < lexp; i++) if (e.equ(exps[i])) return i;
            if (lexp > mexp-2) Program.root.sys.error("too Many exp");
            exps[lexp++] = new Num(e);
            return i;
        }
        public int val(int v)
        {
            if (to_val[v] < 0)
            {
                if (lval >= nvals) Program.root.sys.error(" ! too Many var");
                to_val[v] = lval;
                vals[lval++] = v;
            }
            return to_val[v];
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
                sum.simple();
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
    class mao_key :IComparable {
        public mao_dict dict;
        public ushort[] key;
        public mao_key(mao_dict d)
        {
            dict = d;
            key = new ushort[d.nvals];
            for (int i = 0; i < d.nvals; i++) key[i]=0;
        }
        public mao_key(mao_dict d, ushort[] k)
        {
            dict = d;
            key = new ushort[dict.nvals];
            for (int i = 0; i < d.nvals; i++) key[i] = k[i];
        }
        public mao_key(mao_key k)
        {
            dict = k.dict;
            key = new ushort[dict.nvals];
            set(k);
        }
        public void set(mao_key k)
        {
            for (int i = 0; i < dict.nvals; i++) key[i] = k.key[i];
        }
        public void neg()
        {
            for (int i = 0; i < dict.nvals; i++) key[i] = dict.neg(key[i]);
        }
        public bool test_m(mao_key a) {
            bool ret = true; ushort tmp;
            for (int i = 0; i < dict.nvals; i++)
            {
                tmp = dict.add(key[i], a.key[i]);
                ret = ret && dict.test_a(key[i], a.key[i]);
            }
            return ret;
        }
        public bool mul(mao_key a) {
            bool ret = true; ushort tmp;
            for (int i = 0; i < dict.nvals; i++)
            {
                tmp = dict.add(key[i], a.key[i]);
                ret = ret && dict.test_a(key[i], a.key[i]);
                key[i] = tmp;
            }
            return ret;
        }
        public bool mul(mao_key a0, mao_key a1) {
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
            mao_key k = obj as mao_key;
            for (int i = 0; i < dict.nvals; i++) {
                if (key[i] > k.key[i]) return -1;
                if (key[i] < k.key[i]) return 1;
            }
            return 0;
        }
    }
    class many_as_One {
        mao_dict dict;
        public SortedDictionary<mao_key,num>[] data;

        public many_as_One(mao_dict d)
        {
            dict = d;
            _data_i();
        }
        void _data_i(){
            data = new SortedDictionary<mao_key,num>[2];
            data[0] = new SortedDictionary<mao_key, num>();
            data[1] = new SortedDictionary<mao_key, num>();
        }
        public KeyValuePair<mao_key,num> fr_One(KeyValuePair<One,Num> o)
        {
            int i0,v0;
            KeyValuePair<mao_key, num> ret = new KeyValuePair<mao_key,num>(new mao_key(dict), new Num(o.Value));
            for (i0 = 0; i0 < dict.nvals; i0++) ret.Key.key[i0] = 0;
            foreach (KeyValuePair<int,func> f in o.Key.exps)
            {
                if (f.Value.type_pow() > 1) Program.root.sys.error("cant fast on complex exp");
                v0 = dict.val(f.Key);
                ret.Key.key[v0] = dict.exp((Num)(f.Value.data));
            }
            return ret;
        }
        public One to_One(mao_key fr)
        {
            int i;
            One ret = new One();
            for (i = 0; i < dict.nvals; i++)
                if (fr.key[i] != 0) ret.exps.Add(dict.vals[i],new Func(dict.exps[fr.key[i]]));
            return ret;
        }

        public void add(int ud, ref KeyValuePair<mao_key,num> a)
        {
            if (data[ud].ContainsKey(a.Key)) data[ud][a.Key].add(a.Value); else data[ud].Add(new mao_key(a.Key), new Num(a.Value));
        }
        public void add(int ud, KeyValuePair<mao_key, num> a)
        {
            if (data[ud].ContainsKey(a.Key)) data[ud][a.Key].add(a.Value); else data[ud].Add(new mao_key(a.Key), new Num(a.Value));
        }
        public void add(int ud, ref SortedDictionary<mao_key, num> fr)
        {
            KeyValuePair<mao_key, num> tmp;
            foreach (KeyValuePair<mao_key, num> d in fr) { tmp = d; add(ud, ref tmp); }
        }
        public void mul(int ud, ref KeyValuePair<mao_key, num> m)
        {
            foreach (KeyValuePair<mao_key, num> d in data[ud])
            {
                d.Key.mul(m.Key);
                d.Value.mul(m.Value);
            }
        }
        public void muladd(int ud, ref SortedDictionary<mao_key, num> fr, ref KeyValuePair<mao_key, num> a)
        {
            KeyValuePair<mao_key,num> tmp = new KeyValuePair<mao_key,num>(new mao_key(dict), new Num(0));
            foreach (KeyValuePair<mao_key,num> d in fr) {
                tmp.Key.mul(a.Key,d.Key);
                tmp.Value.set(a.Value);
                tmp.Value.mul(d.Value);
                add(ud,ref tmp);
            }
        }

        public void mul(int ud, ref SortedDictionary<mao_key,num> m0)
        {
            KeyValuePair<mao_key, num> tmp0;
            SortedDictionary<mao_key, num> tmp1 = data[ud];
            data[ud] = new SortedDictionary<mao_key, num>();
            foreach (KeyValuePair<mao_key, num> d in m0) { tmp0 = d;  muladd(ud, ref tmp1, ref tmp0); }
        }
        public void mul(int ud, ref SortedDictionary<mao_key, num> m0, ref SortedDictionary<mao_key, num> m1)
        {
            KeyValuePair<mao_key, num> tmp;
            data[ud].Clear();
            foreach (KeyValuePair<mao_key, num> d in m0) { tmp = d; muladd(ud, ref m1, ref tmp); }
        }
        void set(many_as_One fr)
        {
            for (int i = 0; i < 2; i++)
            {
                data[i].Clear();
                foreach (KeyValuePair<mao_key, num> d in fr.data[i]) data[i].Add(new mao_key(d.Key), new Num(d.Value));
            }
        }
        public many_as_One(Func f, mao_dict d)
        {
            dict = d;
            _data_i();
            foreach (KeyValuePair<One,Num> o in ((Many2)f.data).up.data) add(0, fr_One(o));
            foreach (KeyValuePair<One,Num> o in ((Many2)f.data).down.data) add(1, fr_One(o));
        }
        public Func to_Func(IDS h)
        {
            int i=0, cn = data[0].Count + data[1].Count;
            Many _u = new Many(); Many _d = new Many();
            foreach (KeyValuePair<mao_key, num> d in data[0]) {_u.data.Add(to_One(d.Key),new Num(d.Value)); Program.root.sys.progr(i++,cn);}
            foreach (KeyValuePair<mao_key, num> d in data[1]) {_d.data.Add(to_One(d.Key), new Num(d.Value)); Program.root.sys.progr(i++,cn);}
            return new Func(1,new Many2(_u,_d));
        }

        public many_as_One(many_as_One _m, int _e)
        {
            dict = _m.dict;
            many_as_One tmp = new many_as_One(dict);
            many_as_One _tmp = new many_as_One(dict);
            many_as_One fr = new many_as_One(dict);
            Num exp = dict.exps[_e], nexp = new Num(0);
            int i0,_eu = (int)(exp.up);
            fr.set(_m);
            if (exp.down > 1) {
                if ((fr.data[0].Count > 1) || (fr.data[1].Count > 1)) return;
                if (! fr.data[0].ToArray()[0].Value.root((int)exp.down)) return;
                if (! fr.data[1].ToArray()[0].Value.root((int)exp.down)) return;
                for (i0 = 0; i0 < dict.nvals; i0++) 
                {
                    nexp.set(dict.exps[fr.data[0].ToArray()[0].Key.key[i0]]);
                    nexp.mul((BigInteger)1,exp.down);
                    fr.data[0].ToArray()[0].Key.key[i0] = dict.exp(nexp);
                }
            }
            _data_i();
            data[0].Add(new mao_key(dict), new Num(1));
            data[1].Add(new mao_key(dict), new Num(1));

            tmp.set(fr);
            for (int i = _eu; i > 0; i >>= 1) { 
                if ((i&1) != 0) {
                     mul(0,ref tmp.data[0]);
                     mul(1,ref tmp.data[1]);
                }
                if (i > 1) {
                    _tmp.set(tmp);
                    tmp.mul(0,ref _tmp.data[0], ref _tmp.data[0]);
                    tmp.mul(1,ref _tmp.data[1], ref _tmp.data[1]);
                }
            }

            if (exp.sign < 0) {tmp.data[0] = data[0]; data[0] = data[1]; data[1] = tmp.data[0];}
        }
        public bool expand(int n, many_as_One e, int val)
        {
            bool ret = false;
            int ex = dict.val(val),ee; ushort tex;
            Num max_u = new Num(0), max_d = new Num(0), now_u = new Num(0), now_d = new Num(0);
            many_as_one[] me = new many_as_one[254], ae = new many_as_one[254];
            mao_key z = new mao_key(dict);
            KeyValuePair<mao_key, num> tu = new KeyValuePair<mao_key,num>(new mao_key(dict), new Num(0));
            me[0] = new many_as_One(e,0);
            me[1] = new many_as_One(e,1);
            if ((e.data[0].Count < 1) || (e.data[1].Count < 1)) Program.root.sys.error("wrong");
            int pnow = 0;
            foreach (KeyValuePair<mao_key, num> u in data[n]) 
            {
                tex=u.Key.key[ex];
                tu.Key.set(u.Key);
                tu.Value.set(u.Value);
                if (me[tex] == null) {
                    me[tex] = new many_as_One(e,tex);
                    if (me[tex].data == null) me[tex] = null; else 
                    {
                        if (max_u.great(dict.exps[tex])) max_u.set(dict.exps[tex]);
                        if (!max_d.great(dict.exps[tex])) max_d.set(dict.exps[tex]);
                    }
                }
                if (me[tex] != null) tu.Key.key[ex] = 0; else tex = 0;
                if (ae[tex] == null) ae[tex] = new many_as_One(dict);
                ae[tex].add(0,ref tu);
                Program.root.sys.progr(pnow++,data[n].Count);
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
                        for (int tex0 = 0; tex0 < 254; tex0++) if ((ae[tex0] != null) && (tex0 != tex)) ae[tex0].mul(0, ref me[tex].data[1]);
                        mul(1-n, ref me[tex].data[1]);
                    }
                    ee = dict.exp(now_u);
                    if (me[ee] == null) me[ee] = new many_as_One(e,ee);
                    ae[tex].mul(0, ref me[ee].data[1]);
                    ee = dict.exp(now_d);
                    if (me[ee] == null) me[ee] = new many_as_One(e,ee);
                    ae[tex].mul(0, ref me[ee].data[0]);
                }
                Program.root.sys.progr(tex,254);
            }
            ee = dict.exp(max_u);
            if (me[ee] == null) me[ee] = new many_as_One(e,ee);
            mul(1-n, ref me[ee].data[1]);
            ee = dict.exp(max_d);
            if (me[ee] == null) me[ee] = new many_as_One(e,ee);
            mul(1-n, ref me[ee].data[0]);
            data[n].Clear();
            for (tex = 0; tex < 254; tex++) 
            {
                if (ae[tex] != null) {
                  me[tex].mul(0, ref ae[tex].data[0]);
                  add(n, ref me[tex].data[0]);
                }
                Program.root.sys.progr(tex,254);
            }
            return ret;
        }
        public bool expand(many_as_One e, int id)
        {
            bool r0 =  expand(0,e,id);
            bool r1 =  expand(1,e,id);
            return r0 | r1;
        }
    }
*/

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
        public void error(string e, int pos)
        {
            fout[0].WriteLine("Line {0:G} Pos {0:G}: " + e, nline+1, pos);
            fout[0].Flush();
            Environment.Exit(-1);
        }
        public void error(string e)
        {
            fout[0].WriteLine("Line {0:G}: " + e, nline+1);
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

        -1, 3, 6, 6,  3, 3, 3, 1,   4, 5, 2, 2,  3, 2, 3, 2,
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
            val = sys.rline(); val = val.Replace(" ",""); pos = 0;
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
                    next(); s1 = _parm();
                    if (isequnow('<') || isequnow('>')) { 
                        l_add = isequnow('<'); next();
                        if (ploop > -1) sys.error("macro: wrong loop");
                        if (! int.TryParse(s1, out floop)) sys.error("macro: wrong loop");
                        if (! int.TryParse(_parm(), out tloop)) sys.error("macro: wrong loop");
                        ploop = i2;
                    } else s0 = s0.Replace("#" + m_n_to_c[i2], s1);
                    if (i1 != deep.Count) sys.error("macro: call nparm");
                    i2++;
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
            val = val.Replace("&%","#");
            val = val.Replace("&^"," ");
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
            int _tp = m_c_type[now];
            if (_tp == 2) oper = now;
            if (_tp == 4) deep.Add(new Deep(now,oper));
            if (_tp == 5) {
                if ((deep.Count < 1) || (deep.Last().pair > now) || (now - deep.Last().pair > 2)) sys.error("parse: nonpair");
                deep.RemoveAt(deep.Count - 1);
            }
            pos++; setnow();
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
            if (! (isequnow(isnum) && Int32.TryParse(get(isnum),out r))) Program.root.sys.error("not int");
            return r;
        }
        public string get(string delim)
        {
            string ret = "";
            while (true) {
                if ((! more()) || (delim.IndexOf(val[pos]) > -1)) {setnow(); return ret;}
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

        public KeyValuePair<One,Num> opars()
        {
            bool repeat;
            Func eval = null;
            Func ex = new Func(Program.root.nums[IDS.znums+1]);
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
                eval = null; ex = new Func(Program.root.nums[IDS.znums+1]);
                }
            };
            char[] pc = {isabc,isnum,'{','('};
            Action[] pf = {
                      () => { 
                          Vals _v = Program.root.find_val(get(isname));
                          ex.mul(new Func(new One(new Func(_v)),Program.root.nums[IDS.znums+1]));
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
                () => {string _n = get(isname);
                        if (isequnow('(')) {
                            if (Program.root.fnames.ContainsKey(_n)) eval = fpars(_n,true);
                            else {
                                Vars vr = Program.root.find_var(_n);
                                if ((vr.var == null) || (vr.var.type != 3)) Program.root.sys.error("not row");
                                next(); if (!isequnow(isabc)) Program.root.sys.error("worng row call");
                                Vals vl = Program.root.find_val(get(isname));
                                if (!isequnow(',')) Program.root.sys.error("worng row call");
                                next(); if (!isequnow(isnum)) Program.root.sys.error("worng row call");
                                int st; Int32.TryParse(get(isnum),out st);
                                eval = new Func(((Row)(vr.var.data)).prep_calc(vl,st));
                            }
                        } else eval = new Func(Program.root.find_val(_n));
                      },
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
                on = opars();
                if (m.data.ContainsKey(on.Key)) m.data[on.Key].add(on.Value); else m.data.Add(on.Key,on.Value);
            }
            if (d < deep.Count) next();
            return m;
        }
        public Many2 m2pars()
        {
            return new Many2(mpars(),new Many(Program.root.nums[IDS.znums+1]));
        }
        public Func fpars(string _fn, bool _pair)
        {
            int tp, d = deep.Count;
            Func r = null;
            if (! Program.root.fnames.ContainsKey(_fn)) sys.error("parse: func");
            tp = Program.root.fnames[_fn];
            if (_pair) { if (isequnow('(')) next(); else sys.error("parse: func"); }
            switch (tp) { 
                case 3:
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
                    break;
                default:
                    r = new Func(tp,m2pars());
                    break;
            }
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
                  () => {
                      ret = "(" + ((Row)(f.data)).point.ToString();
                      foreach(KeyValuePair<int,Many2> m in ((Row)(f.data)).data) {
                          ret += "," + print(m.Value);
                      }
                      ret += ")";
                  },
                  () => {ret = print((Many2)(f.data));},
                  () => {ret = print((Many2)(f.data));},
                  () => {ret = print((Many2)(f.data));}
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
        public Num now,from,step,to;
        public Fdim(Num n)
        {
            from = n; now = new Num(n); step = Program.root.nums[IDS.znums]; to = Program.root.nums[IDS.znums];
        }
        public Fdim(Num f, Num s, Num t)
        {
            from = f; now = new Num(f); step = s; to = t;
            Num _t = Num.sub(to,from);
            if ((_t.sign == 0) || (_t.sign != step.sign)) Program.root.sys.error("fcalc: wrong direction");
        }
        public bool next()
        {
            bool r = false;
            if (step.sign != 0) {
                now = Num.add(now,step).simple();
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
            if (t.great(Program.root.nums[IDS.znums+1])) return ifr + isiz;
            t.mul(isiz);
            return ifr + (int)(t.toint());
        }
    }
    class Fdo
    {
        int type,parm;
        string str;
        Vals x,y,r,g,b;
        Ftoint tx,ty,tr,tg,tb;
        int ir,ig,ib;
        public Fdo(int p, string s, Vals v)
        {
            type = 0; parm = p; str = s; x = v;
        }
        public Fdo(Vals _x, Ftoint _tx, Vals _y, Ftoint _ty, int _ir, int _ig, int _ib)
        {
            type = 1;
            x = _x; tx = _tx; y = _y; ty = _ty;
            r = null; g = null; b = null; ir = _ir; ig = _ig; ib = _ib;
        }
        public void doit()
        {
            switch(type) {
                case 0: //print x"str" to #parm
                    string s = "";
                    if ((x == null) || (str == "")) s = str; else switch (str[0]) {
                        case '$':
                            s = Program.par.print(x.get_val(),true,true,0);
                            break;
                        case '.':
                            int p = 0; Int32.TryParse((str + "   ").Substring(1,3), out p);
                            Num tn = new Num(x.get_val().toint());
                            s = tn.up.ToString().Trim() + ".";
                            if (p > 0) {
                                tn = Num.sub(x.get_val(),tn); tn.add(1); tn.mul(Program.root.e10[p]);
                                s += tn.toint().ToString().Trim().Substring(1);
                            }
                            break;
                    }
                    Program.root.sys.wstr(parm,s);
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
            int sx=0, sy=0;
            if (args.Length < 1) return 0;
            string ss = (args.Length < 2 ? "" : args[1]);
            Fileio _f = new Fileio(args[0], ss);

            par = new Parse(_f);
            par.lnext(); sx = par.get_int(); par.next(); sy = par.get_int();
            if ((sx < 100) || (sy < 100)) return -1;
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            m0 = new Shard0(sx, sy);
            bm1 = new System.Drawing.Bitmap(sx, sy);
            for (int i0 = 0; i0 < sx; i0++) for (int i1 = 0; i1 < sy; i1++) bm1.SetPixel(i0, i1, Color.FromArgb(0, 0, 0));
            par.lnext(); 
            BigInteger e; if (! BigInteger.TryParse(par.get(Parse.isnum),out e)) par.sys.error("no");
            root = new IDS(e, par.sys);
            par.lnext(); root.var0 = root.findadd_var(par.get(Parse.isabc)); 
            par.next(); root.var1 = root.findadd_var(par.get(Parse.isabc));
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
        static void doit() {
            Vars var0;
            Vals val0;
            string val,name,fnam;
            bool flag;
            int x0,x1,f0,f1,c0,c1;
            int[] xid = new int[99];
            int _r0,_r1,_r2; double _d0,_d1,x2;
            while (par.sys.has)
            {
                if (par.lnext()) 
                {
                    if (par.isequnow(Parse.isabc)) {
                        name = par.get(Parse.isname);
                        switch (par.now) 
                        {
                            case '=':
                                if (root.fnames.ContainsKey(name)) root.sys.error("reserved name");
                                var0 = root.findadd_var(name);
                                if (var0.ind < 2) root.sys.error("reserved var");
                                par.next();
                                var0.var = (par.isequnow(Parse.isend) ? null : par.fpars("",false));
                                par.sys.wline(0,par.print(var0));
                            break;
                            case '$':
                                bool _div = false, _exp0 = false;
                                var0 = root.find_var(name);
                                if (var0.var == null) par.sys.error("empty name");
                                par.next();
                                if (! par.isequnow('!'))
                                {
                                    List<Func> _id = new List<Func>();
                                    if (par.isequnow('*')) {
                                        foreach (KeyValuePair<string,Vars> v in root.var) {
                                            if ((v.Value != var0) && (v.Value.var != null)) {
                                                int i = 0; while (i < v.Value.vals.Length) {
                                                    _id.Add(new Func(v.Value.vals[i]));
                                                }
                                            }
                                        }
                                        par.next();
                                    } else while (par.isequnow(Parse.isname)) {
                                        val = par.get(Parse.isname); 
                                        if (par.isequnow(',')) par.next();
                                        val0 = root.find_val(val);
                                        if (val0.var == var0) par.sys.error(name + " $recursion - look recursion");
                                        if (val0.var.var != null) _id.Add(new Func(val0));
                                    }
                                    if (par.isequnow('@')) {_exp0=true; par.next();}
                                    if (par.isequnow('$')) {_div=true; par.next();}
                                    foreach (Func i in _id) {
                                        var0.var.revert(i);
                                        var0.var.expand(i);
                                    }
                                    if (_exp0) var0.var.expand();
                                    var0.var.simple();
                                    if (_div && ((var0.var.type == 2) && (((Many2)(var0.var.data)).down.type_exp() < 2)))
                                    {
                                        ((Many2)(var0.var.data)).down.div();
                                        ((Many2)(var0.var.data)).up.mul(((Many2)(var0.var.data)).down.data.ElementAt(0).Key,((Many2)(var0.var.data)).down.data.ElementAt(0).Value);
                                        ((Many2)(var0.var.data)).down = new Many(Program.root.nums[IDS.znums+1]);
                                    }
                                    par.sys.wline(0,par.print(var0));
                                }
                            break;
                        }
                    } else {
                        switch (par.now) 
                        {
                            case '[':
                                par.next();
                                SortedDictionary<Vars,Fdim> fdim = new SortedDictionary<Vars,Fdim>();
                                List<Fdo> fdo = new List<Fdo>();
                                do {
                                    if (! par.isequnow(Parse.isabc)) root.sys.error("calc: wrong");
                                    var0 = root.find_var(par.get(Parse.isname));
                                    if (fdim.ContainsKey(var0)) root.sys.error("calc: double");
                                    switch (par.now) 
                                    {
                                        case '[':
                                            par.next(); 
                                            fdim.Add(var0,new Fdim(par.calc(),par.calc(),par.calc()));
                                            if (par.now != ']') root.sys.error("calc: wrong");
                                            par.next();
                                            break;
                                        case '{':
                                            fdim.Add(var0,new Fdim(par.calc()));
                                            break;
                                        default:
                                            root.sys.error("calc: wrong");
                                            break;
                                    }
                                    if (par.now == ']') break;
                                    par.next();
                                } while (true);
                                par.next();
                                while(par.more()) {
                                    if (par.isequnow(Parse.isabc)) {
                                        val0 = root.find_val(par.get(Parse.isname));
                                        if (! par.isequnow('"')) root.sys.error("wrong do");
                                        par.next(); val = par.get("\""); par.next();
                                        fdo.Add(new Fdo(par.get_int(),val,val0));
                                    } else switch (par.now) {
                                        case '"':
                                            par.next(); val = par.get("\""); par.next();
                                            fdo.Add(new Fdo(par.get_int(),val,null));
                                            break;
                                        case '[':
                                            break;
                                    }
                                    if (par.isequnow(',')) par.next();
                                }
                                bool w;
                                do {
                                    root.uncalc(); foreach (KeyValuePair<Vars,Fdim> vf in fdim) vf.Key.set_now(vf.Value.now);
                                    foreach(Fdo fd in fdo) fd.doit();
                                    w = false;
                                    foreach (KeyValuePair<Vars,Fdim> vf in fdim) {
                                        if (w = vf.Value.next()) break;
                                    }
                                } while (w);
                                break;
                        }
                    }
                }
            }
            par.sys.wline(0,"finished, vars = " + (root.var.Count()).ToString());
            par.sys.close();
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
