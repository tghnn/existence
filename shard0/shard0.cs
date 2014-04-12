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
    public partial class shard0 : Form
    {
        private System.ComponentModel.IContainer components = null;
        public System.Drawing.Bitmap bm,bp;
        public int sx, sy;
        public int pr_now = 0, pr_was = 0, l_now = 0, l_was = 0;
        delegate void SetCallback(int i);
        System.Drawing.Graphics Gr;
        public bool rp;
        public shard0(int x, int y)
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
    class vals
    {
        public static int _ind = 0;
        public int ind;
        public exps_n val;
        public vars var;
        public int deep;
        public vals(vars vr, num vl, int d)
        {
            val = new exps_n(vl);
            var = vr; deep = d; ind = vals._ind++;
        }
    }
    class vars
    {
        public static int _ind = 0;
        public int ind;
        public vals[] vals;
        public func var;
        public int stat;
        public string name;
        public vars(string n, int valn, num vl)
        {
            name = n; stat = 0; var = null; vals = new vals[valn];
            int i = 0; while (i < valn) {
                vals[i] = new vals(this,vl,i); i++;
            }
            ind = vars._ind++;
        }
        public vars(string n, num[] vl)
        {
            name = n; stat = 0; var = null; vals = new vals[vl.Length];
            int i = 0; while (i < vl.Length) {
                vals[i] = new vals(this,vl[i],i); i++;
            }
            ind = vars._ind++;
        }
        public void move()
        {
            int i = vals.Length-1;
            while (i > 0) {
                vals[i].val = vals[i-1].val;
                i--;
            }
        }
        public void move(num v0)
        {
            move();
            vals[0].val = new exps_n(v0);
        }
    }
    class ids
    {
        public int stat_uncalc, stat_calc, exp_prec;
//        public num prec;
        public BigInteger exp_max;
        public SortedDictionary<string,vars> var;
        public fileio sys;
        public func fzero;
        public one ozero;
        public const int znums = 100;
        public num[] nums;
        public string[] funcs_name = {"","","row","fact","int","sign"};
        public SortedDictionary<string,int> fnames;
        public num[] fact;
        public ids(int _vars, int _vals, BigInteger p, BigInteger e, fileio f)
        {
            int i;
            if ((_vars < 11) || (_vals < _vars) || (_vars + _vals > 6666) || (p < 11)) sys.error("wrong init");
            sys = f; stat_uncalc = 1; stat_calc = 2;
            fnames = new SortedDictionary<string,int>();
            var = new SortedDictionary<string,vars>();
            exp_max = e;
            exp_prec = (int)(BigInteger.Log(e,10)/2);
            ozero = new one();
            nums = new num[ids.znums*2];
            i = 0; while (i < ids.znums*2) { nums[i] = new num(i-ids.znums); i++; }
            fzero = new func(nums[ids.znums]);
            i = 1; while (i < funcs_name.Count()) { fnames.Add(funcs_name[i],i); i++; }
            fact = new num[1000];
            BigInteger b = new BigInteger(1), c = new BigInteger(1); fact[0] = new num(b);
            while (b < fact.Count()) {
                c *= b; fact[(int)b] = new num(c); b++;
            }
        }
        int deep(string n)
        {
            int i = 0; while ((i < n.Length) && (n[i]=='\'')) i++;
            return i;
        }
        public vals find_val(string n)
        {
            int i = deep(n);
            vars f = find_val_var(n.Substring(i));
            if (f.vals.Length < i) sys.error(n + " too deep"); 
            return f.vals[i];
        }
        public vars find_val_var(string n0)
        {
            if (!var.ContainsKey(n0)) sys.error(n0 + " var not found"); 
            return var[n0];
        }
        public vars findadd_var(string n)
        {
            int i = deep(n); string n0 = n.Substring(i);
            if (var.ContainsKey(n0)) {
                if (var[n0].vals.Length != i) sys.error(n0 + " wrong deep");
            } else {
                var.Add(n0,new vars(n0,i,nums[ids.znums]));
            }
            return var[n0];
        }
        public void uncalc() { stat_uncalc+=2; stat_calc+=2;}
//          val           var
//old    uncalc,calc  uncalc,calc
//uncalc  get           recurs
//calc    get             get
        exps_n get_exp(vals v)
        {
           if (v.var.stat == stat_calc) { 
           } else if (v.var.stat == stat_uncalc) {
               if (v.deep == 0) sys.error(v.var.name + " recursion");
           } else {
               if (v.var.var == null) sys.error(v.var.name + " var: non is non");
               v.var.stat = stat_uncalc;
               v.var.move(v.var.var.calc());
               v.var.stat = stat_calc;
           }
            return v.val;
        }
        public num get_val(vals v, num e)
        {
           return get_exp(v).exp(e);
        }
        public num get_val(vals v)
        {
           return get_exp(v).exp();
        }
        public string get_name_var(vars v)
        {
            return v.name;
        }
        public string get_name_val(vals v)
        {
            return new String('\'', v.deep) + v.var.name;
        }
    }
    class exps_n
    {
        public SortedDictionary<num,num> data;
        public num val;
        public exps_n(num v)
        {
            data = new SortedDictionary<num,num>();
            val = v;
            data.Add(Program.root.nums[ids.znums],Program.root.nums[ids.znums+1]);
            data.Add(Program.root.nums[ids.znums+1],v);
        }
        public num exp(num e) //link to
        {
            if (! data.ContainsKey(e)) {
                num v = new num(val); v.exp(e);
                data.Add(e,v);
            }
            return data[e];
        }
        public num exp() //link to
        {
            return val;
        }
    }

    interface ipower{
        void exp2();
    }

    abstract class power<T> where T :ipower, new()
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

    class num : power<num>, ipower, IComparable
    {
        public BigInteger up, down;
        static BigInteger prec_base = 10;
        public int sign;
        public BigInteger get_sup() { return up * sign; }
        public num()
        {
            init(0, 0);
        }
        public num(num n)
        {
            set(n);
        }
        public num(num n, int s)
        {
            set(n,s);
        }
        static public num add(num a0, num a1, int s)
        {
            num r = new num(a0); r.add(a1,s);
            return r;
        }
        static public num add(num a0, num a1)
        {
            num r = new num(a0); r.add(a1,1);
            return r;
        }
        static public num sub(num s0, num s1)
        {
            num r = new num(s0); r.add(s1,-1);
            return r;
        }
        static public num neg(num m0)
        {
            return new num(-m0.sign,m0.up,m0.down);
        }
        static public num mul(num m0, num m1)
        {
            return new num(m0.sign * m1.sign,m0.up * m1.up,m0.down * m1.down);
        }
        static public num div(num d0, num d1)
        {
            return new num(d0.sign * d1.sign,d0.up * d1.down,d0.down * d1.up);
        }
        static public num max(num m0, num m1)//no new 
        {
            if (m0.great(m1)) return m1; else return m0;
        }
        static public num min(num m0, num m1) //no new
        {
            if (m0.great(m1)) return m0; else return m1;
        }
        static public num common(num n0, num n1)//no new
        {
            if (n0.sign != n1.sign) return Program.root.nums[ids.znums];
            if (n0.up*n1.down > n1.up*n0.down) return n1; else return n0;
            
        }
        public num(BigInteger u)
        {
            set(u);
        }
        public num(string s)
        {
            set(s);
        }
        public void set(int n)
        {
            init(n, 1);
        }
        public override void copy(ref num n)
        {
            n.sign = sign;
            n.up = BigInteger.Abs(up);
            n.down = BigInteger.Abs(down);
        }
        public override void set(num n)
        {
            sign = n.sign;
            up = BigInteger.Abs(n.up);
            down = BigInteger.Abs(n.down);
        }
        public void set(num n,int s)
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
        private num(int _s, BigInteger _u, BigInteger _d)
        {
            sign = _s; up = _u; down = _d;
        }

        public void set(BigInteger _u)
        {
            if (_u < 0) { sign = -1; up = -_u; } else { sign = 1; up = _u; }
            down = 1;
        }
        public num(int a, int b)
        {
            init(a, b);
        }
        public num(int a)
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
        public bool great(num a)
        {
            if (sign == a.sign)
            return (a.up*down*sign > up*a.down*sign);
            else return (a.sign > 0);
        }
        public void max(num a)
        {
            if (great(a)) set(a);
        }
        public void min(num a)
        {
            if (a.great(this)) set(a);
        }
        public bool equ(num a)
        {
            return ((sign == a.sign) && (up == a.up) && (down == a.down));
        }

        public bool isint()
        {
            return (down == 1);
        }
        public static bool isint(num n, int i)
        {
            if (n == null) return false; else return n.isint(i);
        }
        public bool isint(int n)
        {
            return (down == 1) && (sign * n >= 0) && (up == BigInteger.Abs(n));
        }
        public num simple() //immutable
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
                    if ((_d == 1) && (_u < ids.znums)) return Program.root.nums[ids.znums + (int)(_u)*sign];
                    else return new num(sign,_u,_d);
                }
            }
            if ((down == 1) && (up < ids.znums)) return Program.root.nums[ids.znums + (int)(up)*sign];
            else return this;
        }
        public void common(num n)
        {
            if (sign != n.sign) set0(); else {
                if (up*n.down > n.up*down) set(n);
            }
        }
        public void extract(num n)
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
        public override void mul(num a)
        {
            sign *= a.sign;
            up *= a.up;
            down *= a.down;
        }
        public void div(num a)
        {
            sign *= a.sign;
            up *= a.down;
            down *= a.up;
        }
        public void mul(BigInteger u, BigInteger d)
        {
            up *= u; down *= d;
        }
        public void mul(num a, int e)
        {
            if (e > 0) mul(a); else div(a);
        }
        public void add_up(BigInteger a)
        {
            up += a;
        }
        public void addmul(num m0, num m1)
        {
            add(num.mul(m0,m1),1);
        }
        public void add(int a)
        {
            add(new num(a),1);
        }
        public void add(num a, int s)
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
        public void add(num a) { add(a, 1); }
        public void sub(num a) { add(a, -1); }
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
        public static num exp(num ex, num p)
        {
            num r = new num(ex); r.exp(p); return r;
        }
        public static num exp(num ex, int p)
        {
            num r = new num(ex); r.exp(p); return r;
        }
        public void exp(num ex)
        {
            int u,d;
            if (ex.up == 0) { set1(); return; }
            if ((ex.up > Program.root.exp_max) || (ex.down > Program.root.exp_max)) {
                num e1 = new num(ex);
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
            return (down != 0 ? sign * up / down : 0);
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
            num k = obj as num;
            if (sign != k.sign) return (sign < k.sign ? -1: +1);
            BigInteger u0 = up * k.down, u1 = k.up * down;
            if (u0 == u1) return 0;
            return (u0 < u1 ? -1 : 1);
        }
    }
    class complex: power<complex>, ipower
    {
        num k, i;
        public complex(num _k, num _i) 
        {
            k = new num(_k);
            i = new num(_i);
        }
        public complex(num _k)
        {
            k = new num(_k);
            i = new num(0);
        }
        public complex()
        {
            k = new num(0);
            i = new num(0);
        }
        public complex(complex _k)
        {
            k = new num(_k.k);
            i = new num(_k.i);
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
        public override void set(complex a)
        {
            k.set(a.k); i.set(a.i);
        }
        public override void copy(ref complex a)
        {
            k.copy(ref a.k); i.copy(ref a.i);
        }
        public bool equ(complex a)
        {
            return k.equ(a.k) && i.equ(a.i);
        }
        public void neg()
        {
            k.neg(); i.neg();
        }
        public void add(complex a)
        {
            k.add(a.k); i.add(a.i);
        }
        public override void div()
        {
            num x0, x1;
            x0 = new num(k); x0.exp2();
            x1 = new num(i); x1.exp2();
            x0.add(x1); x0.div();
            k.mul(x0); i.mul(x0);
        }
        public override void mul(complex a)
        {
            num x,_k;
            _k = new num(k); _k.mul(a.k);
            x = new num(i); x.mul(a.i);
            x.neg(); _k.add(x);
            i.mul(a.k); k.mul(a.i);
            i.add(k); k.set(_k);
        }
        public new void exp2()
        {
            //k^2-i^2 : 2*k*i
            num k2,i2;
            k2 = new num(k); k2.exp2();
            i2 = new num(i); i2.exp2();
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

    class one: IComparable
    {
        public SortedDictionary<func,func> exps;
        public one()
        {
            exps = new SortedDictionary<func,func>();
        }
        public one(func f)
        {
            exps = new SortedDictionary<func,func>();
            exps.Add(new func(f),new func(Program.root.nums[ids.znums+1]));
        }
        public one(one o)
        {
            exps = new SortedDictionary<func,func>();
            set(o);
        }
        public void set(one o)
        {
            exps.Clear();
            foreach(KeyValuePair<func,func> m in o.exps) exps.Add(new func(m.Key),new func(m.Value));
        }

        public int CompareTo(object obj) {
            if (obj == null) return 1;
            one o = obj as one;
            SortedDictionary<func,func>.Enumerator e0 = exps.GetEnumerator();
            SortedDictionary<func,func>.Enumerator  e1 = o.exps.GetEnumerator();
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

        public void addto(func val,func exp)
        {
            if (exps.ContainsKey(val)) exps[val].add(exp,1); else exps.Add(new func(val),new func(exp));
        }
        public void mul(one o)
        {
            foreach(KeyValuePair<func,func> m in o.exps) {
                if (exps.ContainsKey(m.Key)) {
                    exps[m.Key].add(m.Value,1);
                    if (exps[m.Key].isconst(0)) exps.Remove(m.Key);
                }
                else exps.Add(new func(m.Key), new func(m.Value));
            }
        }
        public void div()
        {
            foreach(KeyValuePair<func,func> m in exps) m.Value.neg();
        }
        public void exp(int e)
        {
            exp(Program.root.nums[ids.znums+e]);
        }
        public void exp(num e)
        {
            foreach(KeyValuePair<func,func> f in exps) f.Value.mul(e);
        }
        public void exp(func e)
        {
            foreach(KeyValuePair<func,func> f in exps) f.Value.mul(e);
        }

        public void extract(one from) //common multi
        {
            SortedDictionary<func,func> r = new SortedDictionary<func,func>();
            foreach(KeyValuePair<func,func> m in exps) 
                if (from.exps.ContainsKey(m.Key)) {
                    m.Value.common(from.exps[m.Key]);
                    r.Add(m.Key,m.Value);
                }
            exps = r;
        }
        public bool expand_p0(func val, exps_f exu, exps_f exd)
        {
            bool rt = false;
            foreach(KeyValuePair<func,func> m in exps) if (m.Value.CompareTo(val) == 0)
            {
                rt = true; m.Value.type = 2; m.Value.data = new many2(new many(exu.mvar), new many(exd.mvar));
            }
            return rt;
        }
        public bool expand_2(func val, exps_f exu, exps_f exd)
        {
            bool fv = false, fk = false;
            foreach(KeyValuePair<func,func> m in exps) 
            {
                fk = m.Key.expand(val, exu, exd) || fk;
                fv = m.Value.expand(val, exu, exd) || fv;
            }
            if (fk) {
                SortedDictionary<func,func> r = new SortedDictionary<func,func>();
                foreach(KeyValuePair<func,func> m in exps) 
                    if ((! m.Value.isconst(0)) && (! m.Key.isconst(1))) r.Add(m.Key,m.Value);
                exps = r;
            }
            return fv || fk;
        }
        public void simple()
        {
            SortedDictionary<func,func> r = new SortedDictionary<func,func>();
            foreach(KeyValuePair<func,func> m in exps) {//exp|key dont change, only ^|value
                m.Value.simple();
                if (! m.Value.isconst(0)) r.Add(m.Key, m.Value);
            }
            exps = r;
        }
        public void deeper(int deep) {
            SortedDictionary<func,func> r = new SortedDictionary<func,func>();
            foreach(KeyValuePair<func,func> m in exps) r.Add(func.deeper(m.Key,deep),func.deeper(m.Value,deep));
            exps = r;
        }
        public num calc() {
            num rt = new num(1);
            foreach(KeyValuePair<func,func> f in exps) rt.mul(f.Key.calc(f.Value.calc()));
            return rt;
        }
    }

    class many: power<many>, ipower, IComparable
    {
        public SortedDictionary<one,num> data;
        public many()
        {
            data = new SortedDictionary<one,num>();
        }
        public many(one o,num n)
        {
            data = new SortedDictionary<one,num>();
            data.Add(o,n);
        }
        public many(one o)
        {
            data = new SortedDictionary<one,num>();
            data.Add(o,Program.root.nums[ids.znums+1]);
        }
        public many(num n)
        {
            data = new SortedDictionary<one,num>();
            data.Add(new one(),n);
        }
        public many(many m)
        {
            set(m);
        }
        public static SortedDictionary<one,num> copy(SortedDictionary<one,num> c)
        {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            foreach (KeyValuePair<one,num> o in c) r.Add(new one(o.Key),o.Value);
            return r;
        }
        public override void set(many s)
        {
            data = many.copy(s.data);
        }
        public override void copy(ref many s)
        {
            s.set(this);
        }
        public override void set0()
        {
            data.Clear(); 
            data.Add(new one(), Program.root.nums[ids.znums]);
        }
        public override void set1()
        {
            data.Clear();
            data.Add(new one(), Program.root.nums[ids.znums+1]);
        }
        public override void div()
        {
            if (data.Count != 1) Program.root.sys.error("cant divide many");
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            one o = data.ElementAt(0).Key; num n = new num(data.ElementAt(0).Value);
            o.div(); n.div(); r.Add(o,n); data = r;
        }

        public int sign() {
            bool p = false, m = false;
            foreach (KeyValuePair<one,num> o in data) {
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
            many m = obj as many;
            SortedDictionary<one,num>.Enumerator o0 = data.GetEnumerator();
            SortedDictionary<one,num>.Enumerator o1 = m.data.GetEnumerator();
            bool n0,n1;
            int r;
            while (true) {
                do n0 = o0.MoveNext(); while (n0 && (o0.Current.Value.sign == 0));
                do n1 = o1.MoveNext(); while (n1 && (o1.Current.Value.sign == 0));
                if (n0 ^ n1) return (n0 ? o0.Current.Value.CompareTo(Program.root.nums[ids.znums]) : Program.root.nums[ids.znums].CompareTo(o1.Current.Value));
                else {
                    if (! n0) return 0;
                    if ((r = o0.Current.Key.CompareTo(o1.Current.Key)) == 0) {
                        if ((r = o0.Current.Value.CompareTo(o1.Current.Value)) != 0) return r;
                    } else {
                        if (r > 0) return o0.Current.Value.CompareTo(Program.root.nums[ids.znums]);
                        else return Program.root.nums[ids.znums].CompareTo(o1.Current.Value);
                    }
                }
            }
        }
        public num get_num()
        {
            num r = null;
            if (data.Count == 0) r = new num(0);
            if ((data.Count == 1) && (data.ElementAt(0).Key.CompareTo(Program.root.ozero) == 0))
                r = new num(data.ElementAt(0).Value);
            return r;
        }

        public void add(one o, num n, int s) {
            if (data.ContainsKey(o)) {
                data[o] = num.add(data[o],n,s).simple();
            } else data.Add(new one(o),new num(n,s));
        }
        public void add(many from, int s)
        {
            foreach (KeyValuePair<one,num> o in from.data) add(o.Key,o.Value,s);
        }

        public void mul(one o, num n)
        {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            one _o; num _n;
            foreach (KeyValuePair<one,num> m0 in data) {
                _o = new one(m0.Key);
                _o.mul(o);
                _n = num.mul(m0.Value,n);
                if (r.ContainsKey(_o)) r[_o].add(_n); //in r any num is new
                else r.Add(_o,_n);
            }
            data = r;
        }
        public override void mul(many _m)
        {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            one o = new one();
            foreach (KeyValuePair<one,num> m0 in data) {
                foreach (KeyValuePair<one,num> m1 in _m.data) {
                    o = new one(m0.Key);
                    o.mul(m1.Key);
                    if (r.ContainsKey(o)) r[o].addmul(m0.Value,m1.Value); //in r any num is new
                    else r.Add(o, num.mul(m0.Value,m1.Value));
                }
            }
            foreach (KeyValuePair<one,num> d in r) r[d.Key] = d.Value.simple();
            data = r;
        }
        public void mul(num n)
        {
            foreach (KeyValuePair<one,num> m in data) data[m.Key] = num.mul(m.Value,n);
        }

        public void expand(many from, one o, num n, func val)
        {
            one to = new one(o); to.exps[val].set0();
            one _o; num _n;
            foreach (KeyValuePair<one,num> on in from.data) {
                _o = new one(to); _o.mul(on.Key);
                _n = num.mul(n,on.Value);
                add(_o,_n,1);
            }
        }
        public bool expand_p0(func val, exps_f exu, exps_f exd)
        {
            bool rt = false;
            foreach (KeyValuePair<one,num> o in data) 
                rt = o.Key.expand_p0(val, exu, exd) || rt;
            return rt;
        }
        public bool expand_2(func val, exps_f exu, exps_f exd)
        {
            bool rt = false;
            foreach (KeyValuePair<one,num> o in data) rt = o.Key.expand_2(val,exu,exd) || rt;
            return rt;
        }
        //e0,e2,p0,p2
        public bool expand_e0(func val, exps_f exu, exps_f exd)
        {
            bool rt = false;
            SortedDictionary<func,many> ml = new SortedDictionary<func,many>();
            many res = new many();
            exu.add(this,val,+1); exu.calc();
            exd.add(this,val,-1); exd.calc();
            int type = (exu.type > exd.type ? exu.type : exd.type);
            func eu, ed, e;
            foreach (KeyValuePair<one,num> o in data) 
            {
//[+1]/[-1]
//[-up] *= [+1]^min*[-1]^max 
//[+1]^(up + (-min))*[-1]^(max - up)
                if (o.Key.exps.ContainsKey(val)) {
                    if (o.Key.exps[val].type_pow() + type < 3) {
                        e = o.Key.exps[val];
                        eu = (exu.type > 1 ? new func(num.add(exu.min,(num)(e.data))) : e);
                        ed = (exd.type > 1 ? new func(num.sub(exd.max,(num)(e.data))) : e);
                        if (exu.data.ContainsKey(eu) && exd.data.ContainsKey(ed)) {
                            if (! ml.ContainsKey(e)) {
                                ml.Add(new func(e),new many(exu.data[eu]));
                                ml[e].mul(exd.data[ed]);
                            }
                            res.expand(ml[e],o.Key,o.Value,val);
                        } else res.add(o.Key,o.Value,1);
                    } else res.add(o.Key,o.Value,1);
                }
            }
            data = res.data;
            return rt;
        }

        public int type_exp() 
        {
            if (data.Count == 1) {
                if (data.ElementAt(0).Value.isint(1)) return 0; else return 1;
            } else return 2;
        }
        public void exp(func e)
        {
            int te = type_exp(),tp = e.type_pow();
            if (te + tp > 2) Program.root.sys.error("exp: cant many to many");
            if (te > 1) exp((int)(((num)(e.data)).up));
            else {
                one to = data.ElementAt(0).Key;
                if (tp < 2) {
                    data[to] = num.exp(data[to],(num)(e.data));
                    to.exp((num)(e.data));
                } else to.exp(e);
            }
        }

        public new void exp(int e)
        {
            if (data.Count > 1) base.exp(e); else if (data.Count == 1) {
                one to = data.ElementAt(0).Key;
                data[to] = num.exp(data[to],e);
                to.exp(e);
            }
        }
        public KeyValuePair<one,num> extract()
        {
            if (data.Count == 0) return new KeyValuePair<one,num>(new one(), new num(1)); else 
            {
                one ro = null; num rn = null;
                foreach (KeyValuePair<one,num> m in data) {
                    if (ro == null) {
                        ro = new one(m.Key); rn = new num(m.Value);
                    } else {
                        ro.extract(m.Key);
                        rn.extract(m.Value);
                        if (ro.exps.Count < 1) new KeyValuePair<one,num>(ro, new num(0));
                        if (rn.sign == 0) new KeyValuePair<one,num>(new one(), rn);
                    }
                }
                return new KeyValuePair<one,num>(ro, rn);
            }
        }

        public void neg() {
            foreach (KeyValuePair<one,num> o in data) data[o.Key] = num.neg(o.Value);
        }

        public void deeper(int deep) {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            one o;
            foreach (KeyValuePair<one,num> m in data)
            {
                o = new one(m.Key); o.deeper(deep);
                r.Add(o,m.Value); //deeper dont remove val from one
            }
            data = r;
        }

        public void add_toexp(func val, func _e) 
        {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            one o;
            foreach (KeyValuePair<one,num> m in data)
            {
                o = new one(m.Key); o.addto(val,_e);
                if (r.ContainsKey(o)) r[o].add(m.Value); //in r any num is new
                else r.Add(o, new num(m.Value));
            }
            data = r;
        }

        public num find_minexp(func val) //_val^(-x) -> /_val^(x)
        {
            num _min = new num(0), t;
            foreach (KeyValuePair<one,num> o in data)
                if (o.Key.exps.ContainsKey(val)) {
                    t = o.Key.exps[val].get_num_part();
                    if (t.CompareTo(_min) < 0) _min.set(t);
                }
            return _min;
        }

        public void common(many m)
        {
            bool ch = false;
            foreach (KeyValuePair<one,num> on in data) {
                if (m.data.ContainsKey(on.Key)) {
                    data[on.Key] = num.common(on.Value,m.data[on.Key]);
                    if (on.Value.sign == 0) ch = true;
                } else {ch = true; on.Value.set0();}
            }
            if (ch) {
                SortedDictionary<one,num> r = new SortedDictionary<one,num>();
                foreach (KeyValuePair<one,num> on in data) if (on.Value.sign != 0) r.Add(on.Key,on.Value);
                data = r;
            }
        }
/* ?????? common ?????
        public void extract(many m)
        {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            foreach (KeyValuePair<one,num> d in data)
            {
                if (m.data.ContainsKey(d.Key)) {
                    if (d.Value.sign == m.data[d.Key].sign)
                        r.Add(new one(d.Key),num.mul(d.Value,m.data[d.Key]));
                }
            }
            data = r;
        }
*/
        public void simple()
        {
            if (data.Count < 2) return;
            one k; num v;
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            foreach (KeyValuePair<one,num> d in data)
            {
                if (d.Value.up != 0) {
                    k = new one(d.Key); k.simple(); 
                    v = new num(d.Value.simple());
                    if (r.ContainsKey(k)) r[k].add(v,1); else r.Add(k,v);
                }
            }
            data = r;
        }
        public num calc() {
            num rt = new num(0);
            foreach(KeyValuePair<one,num> o in data) rt.add(num.mul(o.Key.calc(),o.Value),1);
            return rt;
        }

    }

    
    class many2: power<many2>, ipower, IComparable
    {
        public many up,down;
        public many2()
        {
            up = new many(); down = new many();
        }
        public many2(many u, many d) //no new 
        {
            up = u; down = d;
        }
        public many2(many u) //no new 
        {
            up = u; 
            down = new many();
            down.data.Add(new one(), Program.root.nums[ids.znums+1]);
        }
        public many2(int n)
        {
            up = new many(); down = new many();
            up.data.Add(new one(), Program.root.nums[ids.znums+n]);
            down.data.Add(new one(), Program.root.nums[ids.znums+1]);
        }
        public many2(num n)
        {
            up = new many(); down = new many();
            up.data.Add(new one(), n);
            down.data.Add(new one(), Program.root.nums[ids.znums+1]);
        }
        public many2(one o, num n)
        {
            up = new many(); down = new many();
            up.data.Add(new one(o), n);
            down.data.Add(new one(), Program.root.nums[ids.znums+1]);
        }

        public many2(many2 m)
        {
            up = new many(m.up); down = new many(m.down);
        }
        public override void set(many2 s)
        {
            up.set(s.up); down.set(s.down);
        }
        public override void copy(ref many2 s)
        {
            s.set(this);
        }
        public override void set0()
        {
            up.data.Clear(); down.data.Clear();
            up.data.Add(new one(), Program.root.nums[ids.znums]);
            down.data.Add(new one(), Program.root.nums[ids.znums+1]);
        }
        public override void set1()
        {
            up.data.Clear(); down.data.Clear();
            up.data.Add(new one(), Program.root.nums[ids.znums+1]);
            down.data.Add(new one(), Program.root.nums[ids.znums+1]);
        }
        public override void div()
        {
            many t;
            if ((up.data.Count == 1) && (up.data.ElementAt(0).Value.sign == 0)) Program.root.sys.error("div0");
            t = up; up = down; down = t;
        }

        public int sign() {
            return up.sign()*down.sign();
        }


        public int CompareTo(object obj) {
            if (obj == null) {int s = sign(); return (s != 0 ? s : down.data.ElementAt(0).Value.sign); }
            many2 m = obj as many2;
            int r = up.CompareTo(m.up);
            if (r != 0) return r;
            return -down.CompareTo(m.down);
        }

        public void add(num n, int s) 
        {
            many t = new many(n); t.mul(down); up.add(t,s);
        }

        public void add(many2 m, int s) 
        {
            if (down.CompareTo(m.down) == 0) up.add(m.up,s); else {
                  many t = new many(m.down);
                  t.mul(down); up.mul(m.down); up.add(t,s);
                  down.mul(m.down);
            }
        }

        public override void mul(many2 m)
        {
            up.mul(m.up); down.mul(m.down);
        }
        public void mul(num n)
        {
            up.mul(n); 
        }
        public void mul(one o, num n)
        {
            up.mul(o,n);
        }

        public void revert(func val) //_val^(-x) -> /_val^(x)
        {
            num min = num.min(up.find_minexp(val),down.find_minexp(val));
            if (min.sign == 0) return;
            min.neg(); func f = new func(min);
            up.add_toexp(val,f);
            down.add_toexp(val,f);
        }

        public bool expand(func fv)
        {
            vals v = (vals)(fv.data);
            func f_exp = v.var.var;
            if ((f_exp == null) || ((f_exp.type != 1) && (f_exp.type != 2))) Program.root.sys.error("wrong expand");
            func val = new func(); val.type = 0; val.data = v;
            if (f_exp.type == 2) {
                exps_f exu = new exps_f(((many2)(f_exp.data)).up,v.deep);
                exps_f exd = new exps_f(((many2)(f_exp.data)).down,v.deep);
                return expand(val, exu, exd);
            } else {
                exps_f exu = new exps_f(new many((num)(f_exp.data)),0);
                exps_f exd = new exps_f(new many(Program.root.nums[ids.znums+1]),0);
                return expand(val, exu, exd);
            }
        }
        public bool expand(func val, exps_f exu, exps_f exd)
        {
            bool rt0,rt02;
            rt02 = up.expand_2(val,exu,exd);
            rt02 = down.expand_2(val,exu,exd) || rt02;

            rt02 = up.expand_p0(val,exu,exd) || rt02;
            rt02 = down.expand_p0(val,exu,exd) || rt02;

            rt0 = up.expand_e0(val,exu,exd);
            num minup = new num(exu.min), maxdw = new num(exd.max);
            rt0 = down.expand_e0(val,exu,exd) || rt0;
            if (rt0) {
                up.mul(exu.data[new func(exu.min)]);
                up.mul(exd.data[new func(exd.max)]);
                down.mul(exu.data[new func(minup)]);
                down.mul(exd.data[new func(maxdw)]);
            }
            if (rt0 || rt02) simple();
            return rt0 || rt02;
        }

        public int type_exp() 
        {
            int u = up.type_exp();
            int d = down.type_exp();
            return (u > d ? u : d);
        }
        public void exp(func e)
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

        public num get_num()
        {
            num u = up.get_num();
            if (u == null) return null;
            num d = down.get_num();
            if (d == null) return null;
            d.div(); u.mul(d); return u;
        }
        public num get_num_part()
        {
            num d = down.get_num();
            if ((d == null) || (! up.data.ContainsKey(Program.root.ozero))) return new num(0);
            d.div(); d.mul(up.data[Program.root.ozero]); return d;
        }
        public void common(many2 m)
        {
            if (down.CompareTo(m.down) == 0) up.common(m.up); else {
                many t = new many(m.up);
                t.mul(down); up.mul(m.down); down.mul(m.down);
                up.common(t);                
            }
        }
        public num simple()
        {
            many _up = up, _dw = down;
            KeyValuePair<one,num> f_up, f_down, f_both;
            f_up = up.extract();
            f_down = down.extract();
            f_both = new KeyValuePair<one,num>(new one(f_up.Key), new num(f_up.Value));
            f_both.Key.extract(f_down.Key);
            f_both.Value.extract(f_down.Value);
            f_both.Key.div(); f_both.Value.div();
            up.mul(f_both.Key,f_both.Value);
            down.mul(f_both.Key,f_both.Value);

            f_up.Value.set1();
            foreach(KeyValuePair<func,func> u in f_up.Key.exps) if (u.Value.sign() >= 0) u.Value.set0();
            f_up.Key.simple();

            f_down.Value.set1();
            foreach(KeyValuePair<func,func> d in f_down.Key.exps) if (d.Value.sign() >= 0) d.Value.set0();
            f_down.Key.simple();

            f_up.Key.div();
            up.mul(f_up.Key,f_up.Value);
            down.mul(f_up.Key,f_up.Value);
            f_down.Key.div();
            up.mul(f_down.Key,f_down.Value);
            down.mul(f_down.Key,f_down.Value);
            up.simple();
            down.simple();
            num nd = down.get_num();
            if (nd == null) return null;
            num nu = up.get_num();
            if (nu != null) {nd.div(); nu.mul(nd); return nu;}
            if ((nd != null) && (! nd.isint(1))) {
                down.data.ElementAt(0).Value.div();
                down.mul(down.data.ElementAt(0).Value);
                down.data.ElementAt(0).Value.set1();
            }
            return null;
        }
        public num calc() {
            num d = down.calc();
            if (d.sign == 0) Program.root.sys.error("div0");
            d.div(); d.mul(up.calc()); return d.simple();
        }

    }
    class exps_f 
    {
        public num max, min;
        int deep;
        public int type; 
        //0 - 1*one, 1 - n*one, 2 - many
        //0,1 - sign, 2 - no sign
        public SortedDictionary<func,many> data;
        public many mvar;
        func e1;
        public exps_f(many m, int _deep){
            min = new num(0); max = new num(0);
            data = new SortedDictionary<func,many>();
            func e0 = new func(Program.root.nums[ids.znums]);
            data.Add(e0, new many());
            data[e0].data.Add(new one(),new num(1));
            deep = _deep;
            e1 = new func(Program.root.nums[ids.znums+1]);
            mvar = new many(m); mvar.deeper(deep);
            type = mvar.type_exp();
            data.Add(e1, mvar);
        }
        void add(func e) {
            if (! data.ContainsKey(e)) data.Add(e,new many());
        }

        public void add(many m, func val, int updown){
            num t; 
            min.set0(); max.set0(); 
            func te;
            if (type > 1) {
                foreach (KeyValuePair<one,num> on in m.data) {
                    if (on.Key.exps.ContainsKey(val)) {
                        te = on.Key.exps[val];
                        if (te.type_pow() + type < 3) {
                            t = (num)(te.data);
                            min.min(t); max.max(t);
                        }
                    }
                }
                min.neg();
                add(new func(min)); add(new func(max));
            }
            t = new num(0);
/*
[+1]^(pow - min)*[-1]^(max - pow)
(a+b)/(c+d)
 -1, -2, +1
 (a+b)^2*(c+d)^1
 [+1]^(-1 - (-2))*[-1]^(+1 - (-1)) (+1) (+2)
 [+1]^(-2 - (-2))*[-1]^(+1 - (-2)) (0) (+3)
 [+1]^(+1 - (-2))*[-1]^(+1 - (+1)) (+3) (0)
*/
            foreach (KeyValuePair<one,num> on in m.data) {
                if (on.Key.exps.ContainsKey(val)) {
                    te = on.Key.exps[val]; 
                    if (te.type_pow() + type < 3) {
                        if (type < 2) add(new func(te)); else {
                            if (updown == +1) { //pow + (-min)
                                t.set((num)(te.data)); t.add(min,+1);
                                add(new func(t));
                            }
                            if (updown == -1) {//max - pow
                                t.set(max); t.add((num)(te.data),-1);
                                add(new func(t));
                            }
                        }
                    }
                }
            }
        }
        public void calc(){
            func dl = new func(Program.root.nums[ids.znums]);
            foreach (KeyValuePair<func,many> d in data) {
/*                if (d.Value.data.Count == 0) {
                    if (d.Key.type == 0) {
                        dl.data = new num((num)(d.Key.data));
                        num c = new num(1);
                        while (((num)(dl.data)).up != 0) {
                            c.up <<= 1; ((num)(dl.data)).up >>= 1;
                            if (data.ContainsKey(dl)) {
                                c.up--; c.up &= ((num)(d.Key.data)).up;

                                d.Value.set(data[dl]);
                                d.Value.exp(new func(c));
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

    class row: IComparable {
        public int point;
        public SortedDictionary<int,many2> data;
        public many2 calc;
        public vals v0,v1;
        public row() {
            point = 0;
            data = new SortedDictionary<int,many2>();
            calc = null;
        }
        public row(row r) {
            point = r.point;
            data = new SortedDictionary<int,many2>();
            calc = null;
            foreach(KeyValuePair<int,many2> m in r.data) data.Add(m.Key,new many2(m.Value));
        }
        public void prep_calc()
        {
            int len = data.Count - point;
            if (len < 1) Program.root.sys.error("row: empty");


        }
        public bool expand(func val, exps_f exu, exps_f exd)
        {
            bool r = false;
            foreach(KeyValuePair<int,many2> m in data) r = m.Value.expand(val,exu,exd) || r;
            return r;
        }
        public void simple()
        {
            foreach(KeyValuePair<int,many2> m in data) m.Value.simple();
        }

        public void deeper(int d) 
        {
            foreach(KeyValuePair<int,many2> m in data) m.Value.deeper(d);
        }
        public int CompareTo(object obj) {
            if (obj == null) return data.ElementAt(0).Value.CompareTo(null);
            row r = obj as row; int t = 0;
            if (data.Count != r.data.Count) return (data.Count < r.data.Count ? -1 : +1);
            foreach(KeyValuePair<int,many2> m in data) 
            {
                t = m.Value.CompareTo(r.data[m.Key]);
                if (t != 0) return t;
            }
            return t;
        }

    }

    class func: power<func>, ipower, IComparable
    {
        public const int types = 7;
        public Object data; //SortedDictionary<int,many> data;
        public int type; //0: &val, 1: &num {imutable}, 2: &many2, 3: row(many2[]), 4: fact(many2), 5: int(many2), 6: sign(many2)
        public func()
        {
            type = -1; data = null;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public func(int t, num n)
        {
            func.set_num[t](this, n);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public func(vals v)
        {
            type = 0; data = v;
        }
        public func(num n)
        {
            type = 1; data = n;
        }
        public func (KeyValuePair<one,num> on)
        {
            type = 1; data = new many2(on.Key,on.Value);
        }
        public func (int t, many2 m)
        {
            type = t; data = m;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public func(func f)
        {
            func.set_func[f.type](this,f);
        }
        static public Action<func,num>[] set_num = {
              (func t, num n) => {},
              (func t, num n) => {t.type = 1; t.data = n;},
              (func t, num n) => {t.type = 2; t.data = new many2(n);},
              (func t, num n) => {t.type = 3; t.data = new row();},
              (func t, num n) => {t.type = 4; t.data = new many2(n);},
              (func t, num n) => {t.type = 5; t.data = new many2(n);},
              (func t, num n) => {t.type = 6; t.data = new many2(n);},
                         };
        static public Action<func,func>[] set_func = {
              (func t, func f) => {t.type = 0; t.data = f.data;},
              (func t, func f) => {t.type = 1; t.data = f.data;},
              (func t, func f) => {t.type = 2; t.data = new many2((many2)(f.data));},
              (func t, func f) => {t.type = 3; t.data = new row((row)(f.data));},
              (func t, func f) => {t.type = 4; t.data = new many2((many2)(f.data));},
              (func t, func f) => {t.type = 5; t.data = new many2((many2)(f.data));},
              (func t, func f) => {t.type = 6; t.data = new many2((many2)(f.data));},
                         };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void set(func f)
        {
            func.set_func[f.type](this,f);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void copy(ref func f)
        {
            func.set_func[type](f,this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void set0()
        {
            func.set_num[type](this,Program.root.nums[ids.znums]);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void set1()
        {
            func.set_num[type](this,Program.root.nums[ids.znums+1]);
        }
        static public Func<func,num>[] get_num_part_func = {
                (func t) => {return Program.root.nums[ids.znums];},
                (func t) => {return (num)(t.data);},
                (func t) => {return ((many2)(t.data)).get_num_part();},
                (func t) => {return Program.root.nums[ids.znums];},
                (func t) => {return Program.root.nums[ids.znums];},
                (func t) => {return Program.root.nums[ids.znums];},
                (func t) => {return Program.root.nums[ids.znums];}
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public num get_num_part()
        {
            return func.get_num_part_func[type](this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isconst(int n) { return (type == 1 ? ((num)data).isint(n): false); }
        static public Func<func,int>[] type_pow_func = {
                (func t) => {return 2;},
                (func t) => {return ((((num)(t.data)).isint()) ? 0 : 1);},
                (func t) => {return 2;},
                (func t) => {return 2;},
                (func t) => {return 2;},
                (func t) => {return 2;},
                (func t) => {return 2;}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int type_pow()
        {
            return func.type_pow_func[type](this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void revert(func val)
        {
            if (type == 2) ((many2)data).revert(val);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void expand(func val)
        {
            if (type == 2) ((many2)data).expand(val);
        }
        static public Action<func>[] neg_func = {
                (func t) => {
                    t.data = new many2(new many(new one(t)));
                    t.type = 2; ((many2)(t.data)).neg();
                },
                (func t) => {((num)(t.data)).neg();},
                (func t) => {((many2)(t.data)).neg();},
                (func t) => {},
                (func t) => {},
                (func t) => {},
                (func t) => {}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void neg()
        {
            func.neg_func[type](this);
        }
        static public Action<func>[] div_func = {
                (func t) => {
                    t.data = new many2(new many(new one(t)));
                    t.type = 2; ((many2)(t.data)).div();
                },
                (func t) => {((num)(t.data)).div();},
                (func t) => {((many2)(t.data)).div();},
                (func t) => {},
                (func t) => {},
                (func t) => {},
                (func t) => {}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void div()
        {
            func.div_func[type](this);
        }
        static public Action<func,func>[] mul_func = {
                (func t, func f) => {//0:0
                    one _o = new one(t); _o.addto(f,new func(Program.root.nums[ids.znums+1]));
                    t.type = 2; t.data = new many2(new many(_o));
                },
                (func t, func f) => {
                    one _o = new one(t);
                    t.type = 2; t.data = new many2(new many(_o,(num)(f.data)));
                },
                (func t, func f) => {
                    one _o = new one(t);
                    t.type = 2; t.data = new many2((many2)(f.data));
                    ((many2)(t.data)).mul(_o,Program.root.nums[ids.znums+1]);
                },
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {//1 *= 0
                    one _o = new one(f);
                    t.type = 2; t.data = new many2(new many(_o,(num)(t.data)));
                },
                (func t, func f) => {  //1 *= 1
                    t.data = num.mul((num)(t.data),(num)(f.data));
                },
                (func t, func f) => { //1 *= 2
                        t.type = 2; t.data = new many2((num)(t.data));
                        ((many2)(t.data)).mul((many2)f.data);
                },
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {//2 *= 0
                    ((many2)(t.data)).mul(new one(f),Program.root.nums[ids.znums+1]);
                },
                (func t, func f) => {  //2 *= 1
                    ((many2)(t.data)).mul((num)f.data);
                },
                (func t, func f) => { //2 *= 2
                    ((many2)(t.data)).mul((many2)f.data);
                },
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {},//3 *= 0
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {},//4 *= 0
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {},//5 *= 0
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {},//6 *= 0
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void mul(func f)
        {
            func.mul_func[type*func.types + f.type ](this,f);
        }
        static public Action<func,num>[] muln_func = {
                (func t, num n) => {
                    one _o = new one(t);
                    t.type = 2; t.data = new many2(new many(_o,n));
                },
                (func t, num n) => { 
                    t.data = num.mul((num)(t.data),n);
                },
                (func t, num n) => { 
                    ((many2)(t.data)).mul(n);
                },
                (func t, num n) => {},
                (func t, num n) => {},
                (func t, num n) => {},
                (func t, num n) => {}
                };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void mul(num n)
        {
            func.muln_func[type](this,n);
        }

        static public Action<func,func>[] common_func = {
                (func t, func f) => {//0:0
                    if (t.data != f.data) {
                        t.type = 1; t.data = Program.root.nums[ids.znums];
                    }
                },
                (func t, func f) => {
                        t.type = 1; t.data = Program.root.nums[ids.znums];
                },
                (func t, func f) => {
                    one _o = new one(t); 
                    num _nd = ((many2)(f.data)).down.get_num(); _nd.div();
                    if ((_nd != null) && ((many2)(f.data)).up.data.ContainsKey(_o) && (((many2)(f.data)).up.data[_o].sign * _nd.sign > 0)) {
                        _nd.mul(((many2)(f.data)).up.data[_o]);
                        if (_nd.great(Program.root.nums[ids.znums+1])) {
                            many _u = new many(); _u.data.Add(_o,_nd);
                            t.type = 2; t.data = new many2(_u,new many(Program.root.nums[ids.znums+1]));
                        }
                    } else {t.type = 1; t.data = Program.root.nums[ids.znums];}
                },
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {
                        t.type = 1; t.data = Program.root.nums[ids.znums];
                }, //1:0
                (func t, func f) => { //1 1
                        t.data = num.common((num)(t.data),(num)(f.data)); //no new - just select
                },
                (func t, func f) => { //1 2
                        t.data = num.common((num)(t.data),((many2)(f.data)).get_num_part());
                },
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {//2:0
                    one _o = new one(f);
                    num _nd = ((many2)(t.data)).down.get_num(); _nd.div();
                    if ((_nd != null) && ((many2)(t.data)).up.data.ContainsKey(_o) && (((many2)(t.data)).up.data[_o].sign * _nd.sign > 0)) {
                        _nd.mul(((many2)(t.data)).up.data[_o]);
                        if (_nd.great(Program.root.nums[ids.znums+1])) {
                            ((many2)(t.data)).up = new many(); ((many2)(t.data)).up.data.Add(_o,_nd);
                            ((many2)(t.data)).down = new many(Program.root.nums[ids.znums+1]);
                        } else {
                            t.type = 0; t.data = f.data;
                        }
                    } else {t.type = 1; t.data = Program.root.nums[ids.znums];}
                },
                (func t, func f) => { //2:1
                        t.type = 1; t.data = num.common(((many2)(t.data)).get_num_part(),(num)(f.data)); 
                },
                (func t, func f) => { //2:2
                        ((many2)(t.data)).common((many2)(f.data));
                        num r = ((many2)(t.data)).get_num();
                        if (r != null) {t.data = r; t.type = 1;}
                },
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {}, //3:0
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {}, //4:0
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {}, //5:0
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},

                (func t, func f) => {}, //6:0
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {},
                (func t, func f) => {}
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void common(func f)
        {
            func.common_func[type*func.types + f.type ](this,f);
        }



        static public Func<func, func,exps_f,exps_f,bool>[] expand2_func = {
                (func t, func v, exps_f u, exps_f d) => {return false;},
                (func t, func v, exps_f u, exps_f d) => {return false;},
                (func t, func v, exps_f u, exps_f d) => {return ((many2)(t.data)).expand(v,u,d);},
                (func t, func v, exps_f u, exps_f d) => {return ((row)(t.data)).expand(v,u,d);},
                (func t, func v, exps_f u, exps_f d) => {return ((many2)(t.data)).expand(v,u,d);},
                (func t, func v, exps_f u, exps_f d) => {return ((many2)(t.data)).expand(v,u,d);},
                (func t, func v, exps_f u, exps_f d) => {return ((many2)(t.data)).expand(v,u,d);}
            };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool expand(func val, exps_f exu, exps_f exd)
        {
            return func.expand2_func[type](this, val, exu, exd);
        }

        static public Action<func,func,int>[] add_func = {
                (func t, func f, int s) => {//0:0
                    one _o = new one(t);
                    many _u = new many(); _u.data.Add(_o,Program.root.nums[ids.znums+1]);
                    _o = new one(); _o.exps.Add(new func(f),new func(Program.root.nums[ids.znums+1]));
                    _u.add(_o,Program.root.nums[ids.znums+1],s);
                    t.type = 2; t.data = new many2(_u,new many(Program.root.nums[ids.znums+1]));                
                },
                (func t, func f, int s) => {
                    one _o = new one(t);
                    many _u = new many(); _u.data.Add(_o,Program.root.nums[ids.znums+1]);
                    _o = new one(); _u.data.Add(_o,(num)(f.data));
                    t.type = 2; t.data = new many2(_u,new many(Program.root.nums[ids.znums+1]));                
                },
                (func t, func f, int s) => {
                    one _o = new one(t);
                    many2 _fm = new many2((many2)(f.data)), _tm = new many2(0);
                    _tm.up.data.Add(_o,Program.root.nums[ids.znums+1]); _tm.add(_fm,s);
                    t.type = 2; t.data = _tm;
                },
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},

                (func t, func f, int s) => {//1:0
                    one _o = new one(f);
                    many _u = new many(); _u.data.Add(_o,Program.root.nums[ids.znums+1]);
                    _o = new one(); _u.add(_o,(num)(t.data),s);
                    t.type = 2; t.data = new many2(_u,new many(Program.root.nums[ids.znums+1]));                
                },
                (func t, func f, int s) => { //1:1
                        t.data = num.add((num)(t.data),(num)(f.data),s);
                },
                (func t, func f, int s) => { //1:2
                        t.type = 2; t.data = new many2((num)(t.data));
                        ((many2)(t.data)).add((many2)f.data,s);
                },
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},

                (func t, func f, int s) => {//2:0
                    one _o = new one(f);
                    many _u = new many(); _u.data.Add(_o,Program.root.nums[ids.znums+1]);
                    ((many2)(t.data)).add(new many2(_u,new many(Program.root.nums[ids.znums+1])),s);
                },
                (func t, func f, int s) => { //2:1
                        ((many2)(t.data)).add((num)f.data,s);
                },
                (func t, func f, int s) => { //2:2
                        ((many2)(t.data)).add((many2)(f.data),s);
                },
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},

                (func t, func f, int s) => {}, //3:0
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},

                (func t, func f, int s) => {}, //4:0
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},

                (func t, func f, int s) => {}, //5:0
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},

                (func t, func f, int s) => {}, //6:0
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {},
                (func t, func f, int s) => {}
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void add(func f, int s)
        {
            func.add_func[type*func.types + f.type ](this,f,s);
        }

        static public Func<func,func,int>[] comp_func = {
                (func t, func f) => {return (t.data == f.data ? 0 : (((vals)(t.data)).ind < ((vals)(f.data)).ind ? -1 : 1));}, //0:0
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},

                (func t, func f) => {return -1;}, //1:0
                (func t, func f) => {return ((num)(t.data)).CompareTo((num)(f.data));},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return ((num)(t.data)).CompareTo(null);},

                (func t, func f) => {return -1;}, //2:0
                (func t, func f) => {return -1;},
                (func t, func f) => {return ((many2)(t.data)).CompareTo((many2)(f.data));},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return ((many2)(t.data)).CompareTo(null);},

                (func t, func f) => {return -1;}, //3:0
                (func t, func f) => {return -1;},
                (func t, func f) => {return -1;},
                (func t, func f) => {return ((row)(t.data)).CompareTo((row)(f.data));},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},

                (func t, func f) => {return -1;}, //4:0
                (func t, func f) => {return -1;},
                (func t, func f) => {return -1;},
                (func t, func f) => {return -1;},
                (func t, func f) => {return ((many2)(t.data)).CompareTo((many2)(f.data));},
                (func t, func f) => {return 1;},
                (func t, func f) => {return 1;},
                (func t, func f) => {return ((many2)(t.data)).CompareTo(null);},

                (func t, func f) => {return -1;}, //5:0
                (func t, func f) => {return -1;},
                (func t, func f) => {return -1;},
                (func t, func f) => {return -1;},
                (func t, func f) => {return -1;},
                (func t, func f) => {return ((many2)(t.data)).CompareTo((many2)(f.data));},
                (func t, func f) => {return 1;},
                (func t, func f) => {return ((many2)(t.data)).CompareTo(null);},

                (func t, func f) => {return -1;}, //6:0
                (func t, func f) => {return -1;},
                (func t, func f) => {return -1;},
                (func t, func f) => {return -1;},
                (func t, func f) => {return -1;},
                (func t, func f) => {return -1;},
                (func t, func f) => {return ((many2)(t.data)).CompareTo((many2)(f.data));},
                (func t, func f) => {return ((many2)(t.data)).CompareTo(null);}
               };
        public int CompareTo(object obj) {
            func f = obj as func;
            int t = (f == null ? func.types : f.type);
            return func.comp_func[type*(func.types+1) + f.type ](this,f);
        }

        static public Action<func>[] simple_func = {
                (func t) => {},
                (func t) => {t.data = ((num)(t.data)).simple();},
                (func t) => {
                    num r = ((many2)(t.data)).simple();
                    if (r != null) {t.data = r; t.type = 1;}
                },
                (func t) => {((row)(t.data)).simple();},
                (func t) => {},
                (func t) => {},
                (func t) => {}
                              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void simple()
        {
            func.simple_func[type](this);
        }

        static public Func<func,int>[] sign_func = {
                (func t) => {return 1;},
                (func t) => {return ((num)(t.data)).sign;},
                (func t) => {return ((many2)(t.data)).sign();},
                (func t) => {return 1;},
                (func t) => {return 1;},
                (func t) => {return 1;},
                (func t) => {return 1;}
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int sign()
        {
            return func.sign_func[type](this);
        }


        static public Action<func,int>[] deeper_func = {
                (func t, int d) => {
                    int _i; if ((_i = ((vals)(t.data)).deep + d) >= ((vals)(t.data)).var.vals.Length) Program.root.sys.error("too deep");
                    t.data = ((vals)(t.data)).var.vals[_i];
                },
                (func t, int d) => {},
                (func t, int d) => {((many2)(t.data)).deeper(d);},
                (func t, int d) => {((row)(t.data)).deeper(d);},
                (func t, int d) => {((many2)(t.data)).deeper(d);},
                (func t, int d) => {((many2)(t.data)).deeper(d);},
                (func t, int d) => {((many2)(t.data)).deeper(d);}
               };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void deeper(int d) {
            func.deeper_func[type](this,d);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static func deeper(func f, int d) {
            func r = new func(f);
            func.deeper_func[r.type](r,d);
            return r;
        }

        static public Func<func,num>[] calc_func = {
                (func t) => {return Program.root.get_val((vals)(t.data)); },
                (func t) => {return (num)(t.data);},
                (func t) => {return ((many2)(t.data)).calc();},
                (func t) => {
                    if (((row)(t.data)).calc == null) ((row)(t.data)).prep_calc();
                    return ((row)(t.data)).calc.calc();
                },
                (func t) => {
                    num r = ((many2)(t.data)).calc();
                    if ((! r.isint()) || (r.sign <= 0) || (r.up >= Program.root.fact.Count())) Program.root.sys.error("fact: wrong");
                    return Program.root.fact[(int)r.up];
                },
                (func t) => {
                    num r = ((many2)(t.data)).calc();
                    if (r.down != 1) {r = new num(r.sign*r.up/r.down);}
                    return r;
                },
                (func t) => {
                    return Program.root.nums[((many2)(t.data)).calc().sign + ids.znums];
                }
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public num calc()
        {
            return func.calc_func[type](this);
        }

        static public Func<func,num,num>[] calce_func = {
                (func t, num e) => {return Program.root.get_val((vals)(t.data),e); },
                (func t, num e) => {return num.exp((num)(t.data),e);},
                (func t, num e) => {return num.exp(((many2)(t.data)).calc(),e);},
                (func t, num e) => {
                    if (((row)(t.data)).calc == null) ((row)(t.data)).prep_calc();
                    return num.exp(((row)(t.data)).calc.calc(),e);
                },
                (func t, num e) => {
                    num r = ((many2)(t.data)).calc();
                    if ((! r.isint()) || (r.sign <= 0) || (r.up >= Program.root.fact.Count())) Program.root.sys.error("fact: wrong");
                    return num.exp(Program.root.fact[(int)r.up],e);
                },
                (func t, num e) => {
                    num r = ((many2)(t.data)).calc();
                    if (r.down != 1) {r = new num(r.sign*r.up/r.down);}
                    return num.exp(r,e);
                },
                (func t, num e) => {
                    int s = ((many2)(t.data)).calc().sign;
                    return Program.root.nums[((e.up & 1) == 0 ? s*s : s) + ids.znums];
                }
              };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public num calc(num e)
        {
            return (e == Program.root.nums[ids.znums+1] ? func.calc_func[type](this) : func.calce_func[type](this,e));
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
            lexp = 2; exps[0] = new num(0); exps[1] = new num(1);
            lval = 0;
        }
        public ushort exp(num e)
        {
            ushort i;
            for (i = 0; i < lexp; i++) if (e.equ(exps[i])) return i;
            if (lexp > mexp-2) Program.root.sys.error("too many exp");
            exps[lexp++] = new num(e);
            return i;
        }
        public int val(int v)
        {
            if (to_val[v] < 0)
            {
                if (lval >= nvals) Program.root.sys.error(" ! too many var");
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
                num sum = new num(exps[e0]);
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
                num neg = new num(exps[e]);
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
    class many_as_one {
        mao_dict dict;
        public SortedDictionary<mao_key,num>[] data;

        public many_as_one(mao_dict d)
        {
            dict = d;
            _data_i();
        }
        void _data_i(){
            data = new SortedDictionary<mao_key,num>[2];
            data[0] = new SortedDictionary<mao_key, num>();
            data[1] = new SortedDictionary<mao_key, num>();
        }
        public KeyValuePair<mao_key,num> fr_one(KeyValuePair<one,num> o)
        {
            int i0,v0;
            KeyValuePair<mao_key, num> ret = new KeyValuePair<mao_key,num>(new mao_key(dict), new num(o.Value));
            for (i0 = 0; i0 < dict.nvals; i0++) ret.Key.key[i0] = 0;
            foreach (KeyValuePair<int,func> f in o.Key.exps)
            {
                if (f.Value.type_pow() > 1) Program.root.sys.error("cant fast on complex exp");
                v0 = dict.val(f.Key);
                ret.Key.key[v0] = dict.exp((num)(f.Value.data));
            }
            return ret;
        }
        public one to_one(mao_key fr)
        {
            int i;
            one ret = new one();
            for (i = 0; i < dict.nvals; i++)
                if (fr.key[i] != 0) ret.exps.Add(dict.vals[i],new func(dict.exps[fr.key[i]]));
            return ret;
        }

        public void add(int ud, ref KeyValuePair<mao_key,num> a)
        {
            if (data[ud].ContainsKey(a.Key)) data[ud][a.Key].add(a.Value); else data[ud].Add(new mao_key(a.Key), new num(a.Value));
        }
        public void add(int ud, KeyValuePair<mao_key, num> a)
        {
            if (data[ud].ContainsKey(a.Key)) data[ud][a.Key].add(a.Value); else data[ud].Add(new mao_key(a.Key), new num(a.Value));
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
            KeyValuePair<mao_key,num> tmp = new KeyValuePair<mao_key,num>(new mao_key(dict), new num(0));
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
        void set(many_as_one fr)
        {
            for (int i = 0; i < 2; i++)
            {
                data[i].Clear();
                foreach (KeyValuePair<mao_key, num> d in fr.data[i]) data[i].Add(new mao_key(d.Key), new num(d.Value));
            }
        }
        public many_as_one(func f, mao_dict d)
        {
            dict = d;
            _data_i();
            foreach (KeyValuePair<one,num> o in ((many2)f.data).up.data) add(0, fr_one(o));
            foreach (KeyValuePair<one,num> o in ((many2)f.data).down.data) add(1, fr_one(o));
        }
        public func to_func(ids h)
        {
            int i=0, cn = data[0].Count + data[1].Count;
            many _u = new many(); many _d = new many();
            foreach (KeyValuePair<mao_key, num> d in data[0]) {_u.data.Add(to_one(d.Key),new num(d.Value)); Program.root.sys.progr(i++,cn);}
            foreach (KeyValuePair<mao_key, num> d in data[1]) {_d.data.Add(to_one(d.Key), new num(d.Value)); Program.root.sys.progr(i++,cn);}
            return new func(1,new many2(_u,_d));
        }

        public many_as_one(many_as_one _m, int _e)
        {
            dict = _m.dict;
            many_as_one tmp = new many_as_one(dict);
            many_as_one _tmp = new many_as_one(dict);
            many_as_one fr = new many_as_one(dict);
            num exp = dict.exps[_e], nexp = new num(0);
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
            data[0].Add(new mao_key(dict), new num(1));
            data[1].Add(new mao_key(dict), new num(1));

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
        public bool expand(int n, many_as_one e, int val)
        {
            bool ret = false;
            int ex = dict.val(val),ee; ushort tex;
            num max_u = new num(0), max_d = new num(0), now_u = new num(0), now_d = new num(0);
            many_as_one[] me = new many_as_one[254], ae = new many_as_one[254];
            mao_key z = new mao_key(dict);
            KeyValuePair<mao_key, num> tu = new KeyValuePair<mao_key,num>(new mao_key(dict), new num(0));
            me[0] = new many_as_one(e,0);
            me[1] = new many_as_one(e,1);
            if ((e.data[0].Count < 1) || (e.data[1].Count < 1)) Program.root.sys.error("wrong");
            int pnow = 0;
            foreach (KeyValuePair<mao_key, num> u in data[n]) 
            {
                tex=u.Key.key[ex];
                tu.Key.set(u.Key);
                tu.Value.set(u.Value);
                if (me[tex] == null) {
                    me[tex] = new many_as_one(e,tex);
                    if (me[tex].data == null) me[tex] = null; else 
                    {
                        if (max_u.great(dict.exps[tex])) max_u.set(dict.exps[tex]);
                        if (!max_d.great(dict.exps[tex])) max_d.set(dict.exps[tex]);
                    }
                }
                if (me[tex] != null) tu.Key.key[ex] = 0; else tex = 0;
                if (ae[tex] == null) ae[tex] = new many_as_one(dict);
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
                    if (me[ee] == null) me[ee] = new many_as_one(e,ee);
                    ae[tex].mul(0, ref me[ee].data[1]);
                    ee = dict.exp(now_d);
                    if (me[ee] == null) me[ee] = new many_as_one(e,ee);
                    ae[tex].mul(0, ref me[ee].data[0]);
                }
                Program.root.sys.progr(tex,254);
            }
            ee = dict.exp(max_u);
            if (me[ee] == null) me[ee] = new many_as_one(e,ee);
            mul(1-n, ref me[ee].data[1]);
            ee = dict.exp(max_d);
            if (me[ee] == null) me[ee] = new many_as_one(e,ee);
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
        public bool expand(many_as_one e, int id)
        {
            bool r0 =  expand(0,e,id);
            bool r1 =  expand(1,e,id);
            return r0 | r1;
        }
    }
*/

    class fileio: IDisposable
    {
        StreamReader fin, f611;
        StreamWriter[] fout;
        int nline,ncline, lines, clines;
        string buf,nout,xout;
        public Boolean has, quit;
        public fileio(string nin, string _nout)
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
            if (fout[n] == null) fout[n] = new StreamWriter(nout + parse.m_n_to_c[n] + xout);
            fout[n].WriteLine(s);
            fout[n].Flush();
        }
        public void wstr(int n, ref string s)
        {
            if (fout[n] == null) fout[n] = new StreamWriter(nout + parse.m_n_to_c[n] + xout);
            fout[n].Write(s);
            fout[n].Flush();
        }
        public void wstr(int n, string s)
        {
            wstr(n, ref s);
        }
        public void Dispose()
        {
            fin.Close();
            for (int n = 0; n < 40; n++) if (fout[n] != null) fout[n].Close();
        }
    }
    class deep {
        public char pair,oper;
        public int pos;
        public deep(char p, char o) {
            pair = p; oper = o;
        }
    }
    class mbody {
        public int nparm;
        public string body;
        public mbody(int n, string s)
        { nparm = n; body = s; }
    }

    class parse: IDisposable
    {
        static public char[] m_n_to_c = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        static int[] m_c_to_n = {
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
        SortedDictionary<string,mbody> macro;
        List<deep> deep;
        List<List<deep>> stack;
        public fileio sys;
        public parse(fileio s)
        {
            sys = s;
            val = ""; pos = 0;
            macro = new SortedDictionary<string,mbody>();
            deep = new List<deep>();
            stack = new List<List<deep>>();
        }
        string _parm() {
            string s1;
                    if (isequnow('{')) s1 = calc().get_sup().ToString(); else {
                        s1 = get(isall,",<>");
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
            stack.Add(new List<deep>(deep));
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
            deep.Clear(); deep.Add(new deep(isend,isend));
            val = sys.rline(); val = val.Replace(" ",""); pos = 0;
            if ((val.Length == 0) || (val[0] == '`')) return false;
            if (val.Substring(0,2) == "##")
            {
                pos = 2; name = "#" + get(isname,"") + "("; if (! isequnow('#')) sys.error("macro: wrong num");
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
                } else macro.Add(name, new mbody(_np,_m));
                return false;
            }
            while ((pos = val.LastIndexOf("#")) > -1) {
                sf = val.Substring(0, pos); name = get(isall,"(") + "("; 
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
        public void next() 
        {
            int _tp = m_c_type[now];
            if (_tp == 2) oper = now;
            if (_tp == 4) deep.Add(new deep(now,oper));
            if (_tp == 5) {
                if ((deep.Count < 1) || (deep.Last().pair > now) || (now - deep.Last().pair > 2)) sys.error("parse: nonpair");
                deep.RemoveAt(deep.Count - 1);
            }
            pos++; 
            if (more()) now = val[pos]; else {
                now = isend;
                if (deep.Count != 1) sys.error("parse: nonpair");
            }
        }
        public string get(char tst, string delim)
        {
            string ret = ""; int nowdeep = deep.Count;
            while (true) {
                if (isequ(now,isend)) return ret;
                if ((nowdeep == deep.Count) && (isequ(now,isclose) || (! isequ(now,tst)) || (delim.IndexOf(now) > -1))) return ret;
                ret += now.ToString(); next();
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

        public KeyValuePair<one,num> opars()
        {
            bool repeat;
            func eval = null;
            func ex = new func(Program.root.nums[ids.znums+1]);
            KeyValuePair<one,num> on = new KeyValuePair<one,num>(new one(),new num(1));
            one ro = new one(); num rn = new num(now == '-' ? -1: +1);
            if ((now == '-') || (now == '+')) next();
            bool l = true;
            Action eset = () => {
                if ((eval.type == 1)  && (ex.type_pow() == 0)) {
                    num nval = new num((num)(eval.data));
                    nval.exp((num)(ex.data)); rn.mul(nval); 
                } else {
                    if (ro.exps.ContainsKey(eval)) ro.exps[eval].add(ex,1); else ro.exps.Add(eval,ex);
                }
                eval = null; ex = new func(Program.root.nums[ids.znums+1]);
            };
            char[] pc = {isabc,isnum,'{','('};
            Action[] pf = {
                      () => { 
                          on.Key.exps.Clear(); 
                          on.Key.exps.Add(new func(Program.root.find_val(get(isname,""))),new func(Program.root.nums[ids.znums+1]));
                          ex.mul(new func(on));
                      },
                      () => ex.mul(new num(get(isnum,""))),
                      () => ex.mul(calc()),
                      () => ex.mul(fpars(true)),
                      () => sys.error("nonum in calc")
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
                () => sys.error("nonum in calc")
                };
            char[] nc = {isabc,isnum,'{','('};
            Action[] nf = {
                () => {string _n = get(isname,"");
                        eval = (isequnow('(') ? fpars(true,_n) : new func(Program.root.find_val(_n)));
                      },
                () => {eval = new func(new num(get(isnum,"")));},
                () => {eval = new func(calc());},
                () => {eval = fpars(true);},
                () => sys.error("nonum in calc")
                };
            char[] oc = {isoper,isclose,isend};
            Action[] of = {
                () => branchnow("+-*/^",oof),
                () => {l = false; },
                () => {l = false; },
                () => sys.error("nonum in calc")
                };
            while (l) {
                branchnow(nc,nf);
                do {
                    repeat = false;
                    branchnow(oc,of);
                } while (repeat);
            }
            if (eval != null) eset();
            ro.simple(); return new KeyValuePair<one,num>(ro,rn.simple());
        }
        public many mpars()
        {
            many m = new many();
            KeyValuePair<one,num> on;
            int d = deep.Count;
            if (isequnow('/')) next();
            if (isequnow(isopen)) next();
            while ((! isequnow(isclose)) && (!isequnow(isend))) {
                on = opars();
                if (m.data.ContainsKey(on.Key)) m.data[on.Key].add(on.Value); else m.data.Add(on.Key,on.Value);
            }
            if (d < deep.Count) next();
            return m;
        }
        public many2 m2pars()
        {
            bool isdown = false;
            if (isequnow('(')) {
                push(); next(); get(isall,"");
                if (isequnow(")/("))
                {
                    get(isall,"");
                    if (isequnow(')')) {
                        next(); 
                        if (isequnow(')') || isequnow(isend)) isdown = true;
                    }
                }
                pop();
            }
            return new many2(mpars(),(isdown ? mpars() : new many(Program.root.nums[ids.znums+1])));
        }
        public func fpars(bool flag)
        {
            string _fn = "";
            if (flag) {
                if (isequnow(isabc)) {
                    _fn = get(isname,"");
                }
            }
            return fpars(flag,_fn);
        }
        public func fpars(bool flag, string _fn)
        {
            int t = 1, d = deep.Count;
            func r = null;
            if (flag) {
                if (! Program.root.fnames.ContainsKey(_fn)) sys.error("parse: func");
                t = Program.root.fnames[_fn];
                if (! isequnow('(')) sys.error("parse: func");
                next();
            }
            switch (t) {
                case 1:
                    r = new func(2,m2pars());
                    break;
                case 2:
                    r = new func(3,Program.root.nums[ids.znums]);
                    if (! isequnow(isnum)) sys.error("parse:row");
                    Int32.TryParse(get(isnum,""),out ((row)(r.data)).point);
                    if (! isequnow(',')) sys.error("parse:row");
                    next();
                    int i = 0; while(true) {
                        if (! isequnow('(')) sys.error("parse:row");
                        ((row)(r.data)).data.Add(i,m2pars());
                        if (! isequnow(',')) break;
                        next(); i++;
                    }
                    break;
                default:
                    r = new func(t+1,m2pars());
                    break;
            }
            if (d < deep.Count) next();
            r.simple(); return r;
        }
        public num calc()
        {
            char[] lo = {'+', ' ', ' ', ' '};
            num[] ln = {new num(0),new num(0),new num(0),new num(0)};
            int lp = 0;
            if (isequnow(isnum)) {
                num r = new num(get(isnum,""));
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
                      () => sys.error("nonum in calc")
                     };
            char[] nc = {isopen,isnum};
            Action[] nf = {
                              () => ln[++lp].set(calc()),
                              () => ln[++lp].set(get(isnum,"")),
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

        public string print(num v, bool plus, bool minus, int pair) //0 - none, 1 - sign, 2 - div
        {
            string ret = ""; bool s = false, d = false;
            if ((v.sign < 0) && minus) {s = true; ret = "-";}
            if ((v.sign > 0) && plus) {s = true; ret = "+";}
            ret += v.up.ToString().Trim();
            if (! v.isint()) {d = true; ret += "/" + v.down.ToString().Trim();}
            if (((pair > 0) && s) || ((pair > 1) && d)) ret = "(" + ret + ")";
            return ret;
        }
        public string print(one o, num n)
        {
            string ret = "", s;
            bool first = true, f1 = false, neg = false;
            int ptyp;
            foreach(KeyValuePair<func,func> m in o.exps) {
                ptyp = m.Value.type_pow();
                if ((ptyp > 1) || (((num)(m.Value.data)).sign != 0)) 
                {
                    if ((ptyp < 2) && (((num)(m.Value.data)).sign < 0)) neg = true; else neg = false;
                    if (first) {
                        s = print(n,true,true,0);
                        if ((s == "+1") || (s == "-1")) {
                            if  (! neg) {
                                ret += s[0]; f1 = true;
                            } else ret += s;
                        } else ret += s;
                    }
                    ret += (neg ? "/" : (f1 ? "" : "*")) + print(m.Key,false);
                    if ((ptyp > 0) || (((num)(m.Value.data)).up > 1)) ret += "^" + print(m.Value,true);
                }
                first = false; f1 = false;
            }
            if (first) ret += print(n,true,true,0);
            return ret;
        }
        public string print(many m)
        {
            string ret = "";
            foreach(KeyValuePair<one,num> o in m.data) {
                ret += print(o.Key, o.Value);
            }
            return ret;
        }
        public string print(many2 m)
        {
            string ret = "";
            if (num.isint(m.down.get_num(),1)) {
                ret += print(m.up);
            } else {
                ret += "(" + print(m.up) + ")/(" + print(m.down) + ")";
            }
            return "(" + ret + ")";
        }
        public string print(func f, bool inpow)
        {
            string ret = "";
            if (f == null) return ret;
            Action[] p = {
                  () => {
                      ret = Program.root.get_name_val((vals)(f.data));
                  },
                  () => {
                      ret = print((num)(f.data),false,! inpow,2);
                  },
                  () => {ret = print((many2)(f.data));},
                  () => {
                  
                  
                  },
                  () => {ret = print((many2)(f.data));},
                  () => {ret = print((many2)(f.data));},
                  () => {ret = print((many2)(f.data));}
                 };
            p[f.type]();
            return Program.root.funcs_name[f.type] + ret;
        }

        public BigInteger get_parm()
        {
            num tmp;
            if (isequnow(isnum) || isequnow('{')) tmp = calc(); else tmp = Program.root.get_val(Program.root.find_val(get(isname,"")),Program.root.nums[ids.znums+1]);
            return tmp.toint();
        }
    }



    static class Program
    {
        public static shard0 m0;
        public static System.Drawing.Bitmap bm1;
        public static parse par;
        public static ids root;


        [STAThread]
        static int Main(string[] args)
        {
            int sx=0, sy=0;
            if (args.Length < 1) return 0;
            string ss = (args.Length < 2 ? "" : args[1]);
            fileio _f = new fileio(args[0], ss);

            par = new parse(_f);
            par.lnext();
            sx = (int)par.get_parm();
            sy = (int)par.get_parm();
            if ((sx < 100) || (sy < 100)) return -1;
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            m0 = new shard0(sx, sy);
            bm1 = new System.Drawing.Bitmap(sx, sy);
            for (int i0 = 0; i0 < sx; i0++) for (int i1 = 0; i1 < sy; i1++) bm1.SetPixel(i0, i1, Color.FromArgb(0, 0, 0));
            par.lnext(); root = new ids((int)par.get_parm(),(int)par.get_parm(),par.get_parm(),par.get_parm(), par.sys);
            par.lnext(); root.findadd_var(par.get(parse.isabc,"")); par.next(); root.findadd_var(par.get(parse.isabc,""));
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
            vars var0;
            vals val0;
            string val,name,fnam;
            bool flag;
            int x0,x1,f0,f1,c0,c1;
            int[] xid = new int[99];
            int _r0,_r1,_r2; double _d0,_d1,x2;
            while (par.sys.has)
            {
                if (par.lnext()) 
                {
                    if (par.isequnow(parse.isabc)) {
                        name = par.get(parse.isname,"");
                        switch (par.now) 
                        {
                            case '=':
                                var0 = root.findadd_var(name);
                                if (var0.ind < 2) root.sys.error("reserved var");
                                par.next();
                                par.push(); fnam = "";flag = false;
                                if (par.isequnow(parse.isabc)) fnam = par.get(parse.isname,"");
                                if (par.isequnow('(')) {
                                    par.next(); par.get(parse.isall,"");
                                    if (par.isequnow(')')) {
                                        par.next(); if (par.isequnow(parse.isend)) flag = true;
                                    }
                                }
                                par.pop();
                                var0.var = (par.isequnow(parse.isend) ? null : par.fpars(flag));
                                par.sys.wline(0,name + " = " + par.print(var0.var,false));
                            break;
                            case '$':
                                bool _div = false;
                                var0 = root.find_val_var(name);
                                if (var0.var == null) par.sys.error("empty name");
                                par.next();
                                if (! par.isequnow('!'))
                                {
                                List<func> _id = new List<func>();
                                if (par.isequnow('*')) {
                                    foreach (KeyValuePair<string,vars> v in root.var) {
                                        if ((v.Value != var0) && (v.Value.var != null)) {
                                            int i = 0; while (i < v.Value.vals.Length) {
                                                _id.Add(new func(v.Value.vals[i]));
                                            }
                                        }
                                    }
                                    par.next();
                                    if (par.isequnow('$')) _div=true;
                                } else while (par.more()) {
                                    val = par.get(parse.isname,""); if (val.Length < 1) break;
                                    val0 = root.find_val(val);
                                    if (val0.var == var0) par.sys.error(name + " $recursion - look recursion");
                                    if (val0.var.var != null) _id.Add(new func(val0));
                                    if (par.isequnow('$')) _div=true;
                                    par.next();
                                }
                                foreach (func i in _id) {
                                    var0.var.revert(i);
                                    var0.var.expand(i);
                                }
                                if (_div && ((var0.var.type == 1) && (((many2)(var0.var.data)).down.type_exp() < 2)))
                                {
                                    ((many2)(var0.var.data)).down.div();
                                    ((many2)(var0.var.data)).up.mul(((many2)(var0.var.data)).down.data.ElementAt(0).Key,((many2)(var0.var.data)).down.data.ElementAt(0).Value);
                                    ((many2)(var0.var.data)).down = new many(Program.root.nums[ids.znums+1]);
                                }
                                par.sys.wline(0,par.print(var0.var,false));
                           }
                                break;
                        }
                    }
                }
            }
            par.sys.wline(0,"finished, vars free = " + (root.var.Count()).ToString());
            par.sys.close();
        }
    }
}
/*
                    switch (par.now) 
                    {
                     case ':':
                        var0 = root.find_var_ex(par.name);
                        for (int i = 0; par.more() && (i < root.deep); i++) root.set_val(root.var_to_val(var0) + i,par.nnext(true));
                            break;
                     case '$':
                        bool _div = false;
                        var0 = root.find_var_ex(par.name);
                        if (root.values[var0] == null) par.sys.error("empty name");
                        par.snext(false);
                        if (par.now() != '!')
                        {
                        List<int> _id = new List<int>();
                        if (par.now() == '*') {
                            for (int ii = root.last-1; ii > -1; ii--) {
                                if (ii != var0) _id.Add(root.var_to_val(ii));
                            }
                            par.snext(false);
                            if (par.now() == '$') _div=true;
                        } else while (par.more()) {
                            val = par.snext(false); if (val.Length < 1) break;
                            val0 = root.find_val(val);
                            if (root.val_to_var(val0) == var0) par.sys.error(root.get_name(var0) + " $recursion - look recursion");
                            root.values[var0].revert(val0);
                            _id.Add(val0);
                            if (par.now() == '$') _div=true;
                            par.snext(false);
                        }
                        foreach (int i in _id) root.values[var0].expand(i);
                        if (_div && (root.values[var0].data[1].Count == 1))
                        {
                            root.values[var0].data[1][0].div();
                            root.values[var0].mul(0, root.values[var0].data[1][0]);
                            root.values[var0].data[1][0] = new one(root.values[var0], 1);
                        }
                        }
                        else
                        {
                        List<int> _id = new List<int>();
                        many_as_one[] mao_fr = new many_as_one[root.vars];
                        mao_dict mdict;
                        par.snext(false); if (par.now() == '*') {
                            par.snext(false);
                            mdict = new mao_dict(root.last,root);
                            for (int ii = 0; ii < root.last; ii++) {
                                mdict.val(ii);
                            }
                        } else mdict = new mao_dict((int)par.nnext(false).get_up(),root);
                        while (par.more())
                        {
                            val = par.snext(true); if (val.Length < 1) break;
                            val0 = root.find_val(val); var1 = root.val_to_var(val0);
                            if (var1 == var0) par.sys.error(root.get_name(var0) + " $recursion - look recursion");
                            if (root.values[var1] != null) {
                                root.values[var0].revert(val0);
                                _id.Add(val0);
                                mao_fr[_id.Count - 1] = new many_as_one(root.values[var1],mdict);
                            }
                            if (par.now() == '$') _div = true;
                        }
                        many_as_one mao_to = new many_as_one(root.values[var0],mdict);
                        bool r = true;
                        while (r)
                        {
                            r = false;
                            for (int i = 0; i < _id.Count; i++)
                            {
                                r = mao_to.expand(ref mao_fr[i], _id[i]) || r;
                            }
                            
                        }
                        root.values[var0] = mao_to.to_many(root,var0);
                        if (_div && (root.values[var0].data[1].Count == 1))
                        {
                            root.values[var0].data[1][0].div();
                            root.values[var0].mul(0, root.values[var0].data[1][0]);
                            root.values[var0].data[1][0] = new one(root.values[var0], 1);
                            root.values[var0].simple();
                        }
                        }                      
                        root.values[var0].print(0); 
                        GC.Collect();
                        break;
                     case '"':
                         {
                            var0 = root.find_var_ex(par.name);
                            if (root.values[var0] == null) par.sys.error("\" empty");
                            val0 = root.find_val(par.snext(true));
                            many tmp = new many(root.values[var0]);
                            root.values[var0].diff(0,val0);
                            root.values[var0].mul(1, tmp.data[1]); //d^2
                            root.values[var0].mul(0, tmp.data[1]); //u'*d
                            tmp.diff(1, val0);
                            tmp.mul(0, ref tmp.data[1]); //u*d'
                            tmp.neg(0);
                            root.values[var0].add(0,tmp.data[0]);//u'*d-u*d'
                            root.values[var0].simple();
                            root.values[var0].print(0);
                         }
                            break;
                     case '@':
                        {
                            bool combo = false;
                            char _c;
                            string nn, s_val;
                            one _ml = null, odv = null;
                            par.snext(false);
                            var0 = root.find_var_ex(par.name);
                            if (root.values[var0] == null) par.sys.error("@ empty");
                            if (par.now() == '/')
                            {
                                nn = par.name + "p1"; if (root.find_var(nn) > -1) par.sys.error("@ overwrite " + nn);
                                var1 = root.set_empty(nn);
                                nn = par.name + "m1"; if (root.find_var(nn) > -1) par.sys.error("@ overwrite " + nn);
                                var2 = root.set_empty(nn);
                                if (root.values[var0].data[1].Count > 1)
                                {
                                    root.values[var1] = new many(root.values[var0], 0);
                                    root.values[var2] = new many(root.values[var0], 1);
                                }
                                else
                                {
                                    root.values[var1] = new many(root.values[var0]);
                                    root.values[var1].revert();
                                    root.values[var2] = new many(root, 1);
                                }
                                root.values[var1].id = var1; root.values[var2].id = var2;
                                root.values[var1].print(0); root.values[var2].print(0);
                                break;
                            }
                            x0 = -1; if (par.now() == '!') x0 = (int)par.nnext(true).get_up();
                            mao_dict mdict = new mao_dict((x0 > 0 ? x0 : 6), root);
                            many_as_one mdv = (x0 > 0 ? new many_as_one(root.values[var0], mdict) : new many_as_one(mdict));
                            many dv = (x0 > 0 ? new many(root, 0) : new many(root.values[var0]));
                            if (par.now() == '=')
                            {
                                val = par.snext(true);
                                odv = new one(dv, new num(par.isnum(val) ? val : "1"));
                                if (!par.isnum(val)) odv.exps[root.find_val(val)].non.set(1);
                                odv.mult.neg();
                            }
                            _c = par.now(); s_val = par.snext(true);
                            if (_c == '@') {
                                combo = true; _c = '$';
                                _ml = new one(dv,1);
                                while (s_val != "") {
                                    val0 = root.find_val(s_val);
                                    if (!_ml.exps[val0].iszero()) par.sys.error("@ double @");
                                    _ml.exps[val0].non.one();
                                    s_val = par.snext(true);
                                }
                            } else if (_c == '&')
                            {
                                    _ml = new one(dv, 1);
                                    _ml.exps[root.find_val(s_val)].non.set(1);
                            } else if (_c == '$') {
                                    var1 = root.val_to_var(root.find_val(s_val));
                                    if (root.values[var1] == null) par.sys.error("@ empty $");
                                    if ((root.values[var1].data[0].Count != 1) || (root.values[var1].data[1].Count != 1)) par.sys.error("one only");
                                    one tmp = root.values[var1].data[0][0];
                                    tmp.mul(root.values[var1].data[1][0]);
                                    _ml = new one(tmp);
                                    if (_ml.mult.isone())
                                    {
                                        for (int ii = 0; ii < root.size; ii++)
                                        {
                                            if (!_ml.exps[ii].iszero())
                                            {
                                                if ((_ml.exps[ii].vars != null) || (!_ml.exps[ii].non.isone())) { combo = false; break; }
                                            }
                                        }
                                    }
                            } else par.sys.error("@ wrong shard opt");
                            _ml.simple();
                            if (x0 < 0)
                            {
                                if (odv != null)
                                {
                                    if (odv.mult.nonzero())
                                    {
                                        dv.mul(1, odv);
                                        dv.add(0, dv.data[1]);
                                        dv.simple(0);
                                    }
                                    dv.data[1].RemoveRange(0, dv.data[1].Count); 
                                    dv.data[1].Add(new one(dv, 1));
                                }
                                else
                                {
                                    if (!dv.revert()) par.sys.error("@ can't shard this many");
                                }
                                if (!combo) slice(root, dv, _ml, s_val);
                                else
                                {
                                    s_val = "";
                                    r_slice(root, dv, _ml);
                                }
                                nn = s_val +  (combo ? "_0_" : "0_") + par.name;
                                if ((var1 = root.find_var(nn)) < 0)
                                {
                                    var1 = root.set_empty(nn);
                                    dv.id = var1;
                                    root.values[var1] = dv;
                                }
                                else par.sys.error("@ overwrite " + nn);
                                root.values[var1].print(0);
                            }
                            else
                            {
                                one _dv = new one(_ml); _dv.div();
                                num e = new num(); int ip = root.last;
                                SortedDictionary<num, many_as_one> res = new SortedDictionary<num, many_as_one>();
                                KeyValuePair<mao_key, num> tkey, kml,kdv;
                                if (odv != null)
                                {
                                    tkey = mdv.fr_one(odv);
                                    if (odv.mult.nonzero())
                                    {
                                        mdv.mul(1, ref tkey);
                                        mdv.add(0, ref mdv.data[1]);
                                    }
                                }
                                else
                                {
                                    if (mdv.data[1].Count > 1) par.sys.error("@ can't shard this many");
                                    tkey = mdv.data[1].First();
                                    mdv.mul(0, ref tkey);
                                }
                                mdv.data[1].Clear();
                                mdv.add(1,mdv.fr_one(odv));
                                kml = mdv.fr_one(_ml); kdv = mdv.fr_one(_dv);
                                int i1 = mdv.data[0].Count;
                                while (mdv.data[0].Count > 0)
                                {
                                    e.zero();
                                    tkey = mdv.data[0].First();
                                    mdv.data[0].Remove(tkey.Key);
                                    if (tkey.Key.test_m(kml.Key))
                                    {
                                        while (tkey.Key.mul(kml.Key)) { e.add(-1); tkey.Value.mul(kml.Value); }
                                        tkey.Key.mul(kdv.Key);
                                    }
                                    else if (tkey.Key.test_m(kdv.Key))
                                    {
                                        while (tkey.Key.mul(kdv.Key)) { e.add(1); tkey.Value.mul(kdv.Value); }
                                        tkey.Key.mul(kml.Key);
                                    }
                                    if (!res.ContainsKey(e)) res.Add(new num(e), new many_as_one(mdict));
                                    res[e].add(0,ref tkey);
                                    e.zero();
                                    root.sys.progr(i1 - mdv.data[0].Count, i1);
                                }
                                e = null;
                                foreach (KeyValuePair<num, many_as_one> _d in res)
                                {
                                    nn = s_val + _d.Key.toname(false) + "_" + par.name;
                                    if ((var1 = root.find_var(nn)) < 0)
                                    {
                                        var1 = root.set_empty(nn);
                                        root.values[var1] = _d.Value.to_many(root,var1);
                                        root.values[var1].data[1].Add(new one(root.values[var1],1));
                                    }
                                    else par.sys.error("@ overwrite " + nn);
                                }
                                for (int i0 = ip; i0 < root.last; i0++) root.values[i0].print(0);
                            }
                        }
                        GC.Collect();
                        break;
                     case '~':
                        {
                        f0 = (int)(par.nnext(true).get_up());
                        f1 = (int)(par.nnext(true).get_up());
                        c0 = (int)(par.nnext(true).get_up());
                        c1 = (int)(par.nnext(true).get_up());
                        if ((f0 + c0 > m0.sx) || (f1 + c1 > m0.sy) || (c0 < 2) || (c1 < 2)) par.sys.error("wrong size");
                        int _all=0, _nul = -1;
                        while(par.more() && (_all < 6))
                        {
                            val = par.snext(true); if (val.Length < 1) break;
                            xid[_all] = root.find_val(val);
                            if (root.values[root.val_to_var(xid[_all])] == null) _nul = _all;
                            _all++;
                        }
                        m0.rp = false;
                        if ((_nul == 0) && (_all == 3)) for (x0 = 0; x0 < c0 * 6; x0++)
                            {
                                root.uncalc();
                                root.set_var_onval(xid[0],new num(x0));
                                _r0 = (int)root.get_val(xid[1]).toint();
                                _r1 = (int)root.get_val(xid[2]).toint();
                                if (_r0 < 0) _r0 = 0; if (_r1 < 0) _r1 = 0;
                                bm1.SetPixel((int)(_r0 % c0) + f0, (int)(_r1 % c1) + f1, Color.FromArgb(255, 255, 255));
                            }
                        else if (_nul == 1) {
                            if (_all == 4)  for (x0 = 22; x0 < c0-22; x0 += 11)
                            {
                                for (x1 = 22; x1 < c1-22; x1 += 11)
                                {
                                    root.uncalc();
                                    root.set_var_onval(xid[0],new num(x0));
                                    root.set_var_onval(xid[1],new num(x1));
                                    _d0 = root.get_val(xid[2]).todouble();
                                    _d1 = root.get_val(xid[3]).todouble();
                                    for (x2 = 0; x2 < 10; x2++) m0.bm.SetPixel(f0 + x0 + (int)(_d0 * x2), f1 + x1 + (int)(_d1 * x2), Color.FromArgb(255,2,2));
                                    bm1.SetPixel(f0 + x0, f1 + x1, Color.FromArgb(255,255,255));
                                }
                                m0.Set(1);
                            }
                            else for (x0 = 0; x0 < c0; x0++)
                        {
                            for (x1 = 0; x1 < c1; x1++)
                            {
                                root.uncalc();
                                root.set_var_onval(xid[0],new num(x0));
                                root.set_var_onval(xid[1],new num(x1));
                                _r0 = (int)root.get_val(xid[2]).toint(); _r1 = _r0; _r2 = _r0;
                                if (_all > 3)
                                {
                                    _r1 = (int)root.get_val(xid[3]).toint(); _r2 = 0;
                                    if (_all == 5)
                                    {
                                        _r2 = (int)root.get_val(xid[4]).toint();
                                    }
                                }
                                if (_r0 < 0) _r0 = 0; if (_r1 < 0) _r1 = 0; if (_r2 < 0) _r2 = 0;
                                if (_r0 > 255) _r0 = 255; if (_r1 > 255) _r1 = 255; if (_r2 > 255) _r2 = 255;
                                bm1.SetPixel(f0 + x0, f1 + x1, Color.FromArgb(_r0,_r2,_r1));
                            }
                            if (c1 > 90) m0.Set(1);
                        }
                        }
                        m0.Set(1);
                        m0.Set(2);
                        m0.rp = true;
                        }
                     break;
                     case '&':
                         {
                        List<int> no_calc = new List<int>();
                        List<calc_out> c_out = new List<calc_out>();
                        BigInteger _fr = 0,_fr0,_to = 0,_one = 1,_res1;
                        int _typ = 0, i0;
                        bool _singl = false, l_add = true;
                        par.snext(false); 
                        switch (par.now()) {
                            case 'i':
                                _typ = 0;
                                break;
                            case 'r':
                                _typ = 1;
                                break;
                            default:
                                root.sys.error("wrong & type");
                                break;
                        }
                        par.pos++;
                        if (par.now() == '=') _singl = true;
                        else
                        {
                            _fr = par.get_parm(root);
                            switch (par.now())
                            {
                                case '<':
                                    l_add = true;
                                    break;
                                case '>':
                                    l_add = false;
                                    break;
                                default:
                                    par.sys.error("loop: wrong");
                                    break;
                            }
                            par.snext(false);
                            _to = par.get_parm(root);
                            val = par.snext(true);
                            if ((xid[0] = root.find_var(val)) < 0) par.sys.error("loop: no name");
                            if (root.values[xid[0]] != null) par.sys.error("loop: must non");
                            while (par.now() != ')') {
                                if ((var1 = root.find_var(par.snext(true))) < 0) par.sys.error("loop: no name");
                                if (no_calc.Contains(var1)) par.sys.error("loop: double var");
                                if (root.values[var1] == null) par.sys.error("loop: must not non");
                                no_calc.Add(var1);
                            }
                        }
                        while (par.more())
                        {
                            par.snext(false); 
                            if (par.now() != '"') {
                                val = par.snext(false); if (val.Length < 1) break;
                                val0 = root.find_val(val);
                            } else val0 = -1;
                            val = par.snext(false); if (val.Length < 1) break;
                            c_out.Add(new calc_out(val.Substring(1),val0,parse.m_char_to_num(ref par.val, par.pos)));
                            par.snext(false);
                        }
                        string[] _out = new string[11];
                        _fr0 = _fr; while (true)
                        {
                            if (!_singl)
                            {
                                if (l_add) { if (_fr > _to) break; } else { if (_fr < _to) break; }
                                root.uncalc();
                                foreach (int nc in no_calc) root.calc_stat[nc] = root.stat_calc;
                                root.set_var(xid[0], new num(_fr, _one));
                            }
                            i0 = 0; while (i0 < 10) _out[i0++]="";
                            foreach (calc_out _c in c_out) {
                                if (_c.val > -1) {
                                    if (_c.str == "$") _out[_c.nout] += root.values[root.val_to_var(_c.val)].print(-1);
                                    else {
                                        if (_typ == 0) {
                                            _res1 = root.get_val(_c.val).toint();
                                            _out[_c.nout] += _res1.ToString(_c.str);
                                        } else {
                                            _out[_c.nout] += root.get_val(_c.val).print("","-","");
                                        }
                                    }
                                } else {
                                    _out[_c.nout] += _c.str;
                                }
                                i0++;
                            }
                            i0 = 0; while (i0 < 10) {
                                if (_out[i0]!="") par.sys.wline(i0,_out[i0]);
                                i0++;
                            }
                            if (_singl) break; else { if (l_add) _fr++; else _fr--; }
                            root.sys.progr((int)(_fr - _fr0), (int)(_to - _fr0));
                        }
                         }
                        break;
                    }
                }
            }
 */







/*
    class vlist: IEnumerator,  IEnumerable
    {
        public ids root;
        public UInt64[] data;
        private int dsize, num, first, pos, rest, rest_ext;
        private vlist ext;
        void init()
        {
            dsize = (root.size >> 6) + 1;
            data = new UInt64[dsize];
            for (int i = 0; i < dsize; i++) data[i] = 0;
        }
        public vlist(ids h)
        {
            root = h; init();
            num = 0; ext = null;
            Reset();
        }
        public vlist(vlist v)
        {
            root = v.root; init();
            set(v); Reset();
        }
        public void set(vlist v) 
        {
            ext = null; num = v.num; first = v.first;
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
        public bool isempty() {return num < 1;}
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
                if ((num == 0) || (first > n)) first = n;
                data[n >> 6] |= f;
                num++; if (pos < n) rest++;
            }
        }
        public void del(int n)
        {
            UInt64 f = ((UInt64)1 << (n & 63));
            if ((data[n >> 6] & f) != 0) {
                if (num < 1) throw new NotImplementedException();
                data[n >> 6] ^= f;
                num--; if (pos < n) rest--;
                if ((n == first) && (num > 0)) first = dnext(n+1);
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
                            num tn = new num(head.get_val(val)); tn.exp(fe);
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
        static void slice(ids root, many dv, one _ml, string s_val)
        {
            num e = new num(); int ip = root.last;
            one odv = null;
            one _dv = new one(_ml); _dv.div();
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
                if (!res.ContainsKey(e)) res.Add(new num(e), new many(root, -1));
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
        static void r_slice(ref ids root, ref many dv, ref one _ml)
        {
            num e = new num(); int ip = root.last, var1;
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
                    if (!res.ContainsKey(_k)) res.Add(_k, new many(root, -1));
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
