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
            // 
            // shard0
            // 
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
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }


    class ids
    {
        public int vars, deep,size, last, stat_uncalc, stat_calc;
        public num prec;
        string[] names;
        public func[] values;
        num[] calc;
        public int[] calc_stat;
        public fileio sys;
        public ids(BigInteger _v, BigInteger _d, BigInteger p, fileio f)
        {
            if ((_v < 11) || (_v > 6666) || (_d < 1) || (_d > 6) || (p < 11)) sys.error("wrong init");
            vars = (int)_v; deep = (int)_d; size = vars*deep;
            prec = new num(1,p,p); sys = f; last = 0; stat_uncalc = 1; stat_calc = 2;
            names = new string[vars];
            values = new func[vars];
            calc = new num[size];
            calc_stat = new int[vars];
        }
        public int set_empty(string nam)
        {
            if (last >= vars) sys.error("too many vars");
            while ((nam.Length > 0) && (nam[0] == '\'')) nam = nam.Substring(1);
            names[last] = nam;
            values[last] = null; calc_stat[last] = 0;
            for (int i = 0; i < deep; i++) calc[last*deep + i] = new num();
            last++; return last - 1;
        }
        public void uncalc() { stat_uncalc+=2; stat_calc+=2;}
        private void uncalc_var(int var)
        {
            for (int ii = deep - 1; ii > 0; ii--) calc[var*deep + ii].set(calc[var*deep + ii-1]);
            calc[var*deep].unset(); calc_stat[var] = stat_uncalc;
        }
        public int find_var(string n)
        {
            int i;
            for (i = 0; i < last; i++) if (n == names[i]) return i;
            return -1;
        }
        public int find_var_ex(string n)
        {
            int rt = find_var(n);
            if (rt < 0) sys.error(n + " var not found");
            return rt;
        }
        public int find_val(string n)
        {
            int i,f;
            i = 0; while ((i < deep-1) && (i < n.Length) && (n[i]=='\'')) i++;
            if ((f = find_var(n.Substring(i))) < 0) sys.error(n.Substring(i) + " var not found"); 
            return f*deep+i;
        }
//          val           var
//old    uncalc,calc  uncalc,calc
//uncalc  get           recurs
//calc    get             get
        public num get_val(int val)
        {
           int var = val_to_var(val); 
           int nowdeep = val - var_to_val(var); 
           bool isvar = (nowdeep == 0);
           if (var >= last) sys.error("nonexisted var");
           if (calc_stat[var] == stat_calc) { 
           } else if (calc_stat[var] == stat_uncalc) {
               if (isvar) sys.error(names[var] + " recursion");
           } else {
               if (values[var] == null) sys.error(names[var] + " var: non is non");
               uncalc_var(var); calc[var*deep].set(values[var].calc()); calc_stat[var] = stat_calc;
           }
           if (!calc[val].exist()) sys.error(names[var] + " val: non is non");
           return (calc[val]);
        }
        public num get_var(int var)
        {
            return get_val(var*deep);
        }
        public void set_var(int var, num n)
        {
           if (calc_stat[var] != stat_calc) {
               if (calc_stat[var] != stat_uncalc) uncalc_var(var);
               calc_stat[var] = stat_calc;
           }
           calc[var*deep].set(n);
        }
        public void set_val(int val, num n)
        {
           calc[val].set(n);
        }
        public void set_var_onval(int val, num n)
        {
            set_var(val_to_var(val),n);
        }
        public string get_name(int var)
        {
            return names[var];
        }
        public string get_name_onval(int val)
        {
            return new String('\'', val % deep) + names[val_to_var(val)];
        }
        public int val_to_var(int n) {return n/deep;}
        public int val_to_deep(int n) {return n - val_to_var(n);}
        public int var_to_val(int n) {return n*deep;}
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
        private BigInteger up, down;
        private int sign;
        private bool exs;
        public void set_sign(int _s) { sign = _s;}
        public int get_sign() { return sign;}
        public BigInteger get_up() { return up;}
        public BigInteger get_sup() { return up * sign; }
        public BigInteger get_down() { return down; }
        public num()
        {
            init(0, 0); exs = false;
        }
        public num(num n)
        {
            set(n);
        }
        public num(num n, int s)
        {
            set(n,s);
        }
        public num(num m0, num m1)
        {
            sign = m0.sign * m1.sign;
            up = m0.up * m1.up;
            down = m0.down * m1.down;
            exs = true;
        }
        public num(BigInteger u)
        {
            set(u);
        }
        public num(string s)
        {
            set(s);
        }
        public void unset()
        {
            exs = false;
        }
        public void set(int n)
        {
            init(n, 1);
        }
        public override void copy(ref num n)
        {
            n.sign = sign;
            n.up = up;
            n.down = down;
            n.exs = exs;
        }
        public override void set(num n)
        {
            sign = n.sign;
            up = BigInteger.Abs(n.up);
            down = BigInteger.Abs(n.down);
            exs = n.exs;
        }
        public void set(num n,int s)
        {
            sign = n.sign * s;
            up = BigInteger.Abs(n.up);
            down = BigInteger.Abs(n.down);
            exs = n.exs;
        }
        public void set(string s)
        {
            down = 1;
            exs = BigInteger.TryParse(s, out up);
            sign = (up > 0 ? 1 : 0);
            exs = true;
        }
        public num(int _s, BigInteger _u, BigInteger _d)
        {
            set(_s,_u,_d);
        }
        public void set_up(BigInteger _u)
        {
            up = (_u > 0 ? _u: 0-_u); exs = true;
        }
        public void set_down(BigInteger _d)
        {
            down = (_d > 0 ? _d: 0-_d);
        }
        public void set(int _s, BigInteger _u, BigInteger _d)
        {
            sign = _s; up = _u; down = _d; exs = true;
        }
        public void set(BigInteger _u, BigInteger _d)
        {
            if (_u < 0) { sign = -1; up = -_u; } else { sign = 1; up = _u; }
            down = _d; exs = true;
        }
        public void set(BigInteger _u)
        {
            if (_u < 0) { sign = -1; up = -_u; } else { sign = 1; up = _u; }
            down = 1; exs = true;
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
            exs = true;
        }
        public void neg()
        {
            sign *= -1;
        }
        public bool exist()
        {
            return exs;
        }
        public bool nonzero()
        {
            return (up > 0);
        }
        public bool iszero()
        {
            return (up == 0);
        }
        public bool great(num a)
        {
            if (sign == a.sign)
            {
                return (a.up*down*sign > up*a.down*sign);
            } else return (a.sign > 0);
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

        public bool isone()
        {
            return (down == 1) && (up == 1);
        }
        public bool isint()
        {
            return (down == 1);
        }
        public void simple()
        {
            BigInteger a;
                if (up == 0) { sign = 0; down = 1; } else do
                {
                    a = BigInteger.GreatestCommonDivisor(up, down);
                    up = BigInteger.Divide(up, a);
                    down = BigInteger.Divide(down, a);
                } while (a != 1);
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
            sign = 0;
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
        public void add_up(BigInteger a)
        {
            up += a;
        }
        public void addmul(num m0, num m1)
        {
            num m = new num(m0,m1);
            add(m,1);
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
        public void exp(num ex)
        {
            int e = (int)ex.up;
            if (e < 10000) exp(e*ex.sign);
            e = (int)ex.down;
            if (e < 10000) exs = root(e);
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
            if ((non_one == "") || (!isone())) {
                s0 += up.ToString().Trim();
                if (down > 1) s0 += "/" + down.ToString().Trim();
                s0 +=  non_one;
            }
            return s0;
        }
        public string toname(bool group) {
            if (iszero()) return "0";
            if (isone() && (get_sign() > 0) && group) return "";
            return (get_sign() < 0 ? "_" : "") + get_up().ToString().Trim() + (get_down() > 1 ? ("_" + get_down().ToString().Trim()) : "");
        }
        public int CompareTo(object obj) {
            if (obj == null) return 1;
            num k = obj as num;
            if (sign != k.sign) return sign;
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
            k.simple(); i.simple();
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
            if (i.iszero()) return k.print(plus,minus,non_one);
            return "[" + k.print("","-","") + "," + i.print("","-","") + "]" + non_one;
        }
    }
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
            ext = v.ext; num = v.num; first = v.first;
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

    class exp :IComparable
    {
        public num non;
        public num[] vars;
        public ids root;
        public vlist list;
        public exp(ids h)
        {
            root = h;
            non = new num(0);
        }
        public exp(ids h, int n)
        {
            root = h;
            non = new num(n);
        }
        public exp(exp e)
        {
            int _sz = e.root.size;
            root = e.root;
            non = new num(e.non);
            if (e.vars != null)
            {
                vars = new num[_sz];
                for (int i = 0; i < _sz; i++) vars[i] = new num(e.vars[i]);
            }
            if (e.list != null) list = new vlist(e.list);
        }
        public void set(exp e)
        {
            int _sz = root.size;
            non.set(e.non);
            if (e.vars != null)
            {
                vars = new num[_sz];
                for (int i = 0; i < _sz; i++) vars[i] = new num(e.vars[i]);
            } else vars = null;
            if (e.list != null) list = new vlist(e.list); else list = null;
        }

        void set_vars()
        {
            int _sz = root.size;
            if (vars == null)
            {
                vars = new num[_sz];
                for (int i = 0; i < _sz; i++) vars[i] = new num(0);
            }
            if (list == null) list = new vlist(root);
        }
        public void delvar(int val) {
            vars[val].set0(); list.del(val);
        }
        public void setvar(int val, num n) {
            list.set(val, !n.iszero());
            vars[val].set(n);
        }
        public void mulvar(int val, num n) {
            if (n.iszero()) {
                vars[val].set0(); list.del(val);
            } else {
                vars[val].mul(n);
            }
        }
        public void addvar(int val, num n, int sign) {
            bool z = vars[val].iszero();
            vars[val].add(n,sign);
            vars[val].simple();
            if (z != vars[val].iszero()) list.set(val,z);
        }
        public void addvar(string s, num n, int sign) {
            addvar(root.find_val(s),n, sign);
        }
        public void addvar(string s, int n, int sign) {
            addvar(s,new num(n), sign);
        }
        public bool test_add(exp e, int sign)
        {
            num tmp = new num(non);
            tmp.add(e.non, sign);
            if ((e.non.get_sign() != 0) && (e.non.get_sign() == tmp.get_sign())) return false;
            if (e.vars != null)
            {
                set_vars();
                foreach(int i in e.list)
                {
                    tmp.set(vars[i]);
                    tmp.add(e.vars[i],sign);
                    if ((e.vars[i].get_sign() != 0) && (e.vars[i].get_sign() == tmp.get_sign())) return false;
                }
            }
            return true;
        }
        public bool add(exp e, int sign)
        {
            bool r = true; //non greater, false - become greater
            non.add(e.non, sign); non.simple();
            if ((e.non.get_sign() != 0) && (e.non.get_sign() == non.get_sign())) r = false;
            if (e.vars != null)
            {
                set_vars();
                foreach(int i in e.list)
                {
                    addvar(i,e.vars[i],sign);
                    if ((e.vars[i].get_sign() != 0) && (e.vars[i].get_sign() == vars[i].get_sign())) r = false;
                }
                if (list.isempty()) {vars = null; list = null;}
            }
            return r;
        }
        public void mul(num n)
        {
            non.mul(n);
            if (vars != null) foreach (int i in list) mulvar(i,n);
        }
        public void min(exp e)
        {
            if (!non.great(e.non)) non.set(e.non);
            if (e.vars == null) {
                if (vars != null) foreach (int i in list) if (vars[i].get_sign() > 0) delvar(i);
            } else {
                set_vars(); list.link(e.list);
                foreach (int i in list) if (!vars[i].great(e.vars[i])) setvar(i,e.vars[i]);
                list.link();
            }
        }
        public void neg()
        {
            non.neg();
            if (vars != null) foreach (int i in list) vars[i].neg();
        }
        public void set0()
        {
            non.set0(); vars = null; list = null;
        }
        public void set1()
        {
            non.set1(); vars = null; list = null;
        }
        public bool iszero()
        {
            return (non.iszero() && (vars == null));
        }
        public bool isnum()
        {
            return ((! non.iszero()) && (vars == null));
        }
        public bool equ(exp e)
        {
            if (!non.equ(e.non)) return false;
            if (vars == null) { if (e.vars == null) return true; else return false; }
            if (e.vars == null) return false;
            list.link(e.list);
            foreach (int i in list) if (!vars[i].equ(e.vars[i])) {list.link(); return false;}
            list.link(); return true;
        }
        public bool ispos() //?????????????
        {
            if (non.get_sign() > 0) return true;
            if (vars == null) {
                if (non.get_sign() >= 0) return true;
            } else {
                foreach (int i in list) if (vars[i].get_sign() > 0) return true;
            }
            return false;
        }

        public void extract(exp e)
        {
           if (non.get_sign() == e.non.get_sign())
           {
                if (e.non.great(non)) non.set(e.non);
           } else non.set0();
           if (vars != null) {
                if (e.vars == null) vars = null; else {
                    list.link(e.list);
                    foreach (int i in list) {
                        if (e.vars[i].get_sign() == vars[i].get_sign())
                        {
                            if (e.vars[i].great(vars[i])) setvar(i,e.vars[i]);
                        } else delvar(i);
                    }
                    list.link();
                }
            }
        }
        public string print(string name)
        {
            string r;
            if (vars == null)
            {
                if (non.iszero()) return "";
                r = ((non.get_sign() > -1) ? "" : "/") + name;
                if (non.isone()) return r;
                if (non.get_down() > 1) return r + "^(" + non.print("","","") + ")"; else return r + "^" + non.print("","","");
            }
            else
            {
                r = "" + name + "^("; bool first = true;
                foreach (int i in list) {
                    r += vars[i].print((first ? "" : "+"),"-","*") + root.get_name_onval(i);
                    first = false;
                }
                return r + (non.iszero() ? "" : non.print("+","-","")) + ")";
            }
        }
        public int CompareTo(object obj) { 
            if (obj == null) return 1;
            exp e = obj as exp;
            if (non.equ(e.non)) {
                if (vars == null) {
                    if (e.vars == null) return 0;
                    return 0 - e.vars[e.list.getfirst()].get_sign();
                }
                if (e.vars == null) return vars[list.getfirst()].get_sign();
                else {
                    list.link(e.list);
                    foreach(int i in list) if (!vars[i].equ(e.vars[i])) {list.link(); return vars[i].CompareTo(e.vars[i]);}
                    list.link(); return 0;
                }
            } else return non.CompareTo(e.non);
        }
    }
    class one: IComparable
    {
        public exp[] exps;
        public vlist list;
        public one(ids h)
        {
            init_z(h);
        }
        public one(one o)
        {
            init(o);
        }
        public void set(one o)
        {
            list.link(o.list);
            foreach (int i in list) exps[i].set(o.exps[i]);
            list.set(o.list);
        }
        void init(one o)
        {
            init(o.list.root);
            for (int i0 = 0; i0 < list.root.size; i0++) exps[i0] = new exp(o.exps[i0]);
            list = new vlist(o.list);
        }
        void init(ids h)
        {
            exps = new exp[h.size];
            list = new vlist(h);
        }
        void init_z(ids h)
        {
            init(h);
            for (int i0 = 0; i0 < h.size; i0++) exps[i0] = new exp(list.root);
        }

        public int CompareTo(object obj) {
            if (obj == null) return 1;
            one o = obj as one;
            int h;
            list.link(o.list);
            foreach(int n in list) {
                h = exps[n].CompareTo(o.exps[n]);
                if (h != 0) {list.link(); return h;}
            }
            list.link(); return 0;
        }

        public bool test_mul(one a)
        {
            if (list.root != a.list.root) return false;
            list.link(a.list); foreach (int i in list) { 
                if (!exps[i].test_add(a.exps[i],1)) {list.link(); return false;}
            }
            list.link(); return true;
        }
        public bool mul_t(one a)
        {
            bool rt = true;
            if (list.root != a.list.root) return false;
            list.link(a.list);
            foreach (int i in list) {
                if (!exps[i].add(a.exps[i],1)) rt = false;
                list.set(i,! exps[i].iszero());
            }
            list.link(); return rt;
        }
        public void mul(one a)
        {
            if (list.root != a.list.root) return;
            list.link(a.list); foreach (int i in list) { 
                exps[i].add(a.exps[i],1);
                list.set(i,! exps[i].iszero());
            }
            list.link();
        }
        public void div()
        {
            foreach (int i0 in list) exps[i0].neg();
        }
        public void exp(int e)
        {
            exp(new num(e));
        }
        public void exp(num e)
        {
            foreach (int i in list) exps[i].mul(e);
        }

        public void extract(one from)
        {
            list.link(from.list);
            foreach (int i in list) {
                exps[i].extract(from.exps[i]);
                list.set(i,! exps[i].iszero());
            }
            list.link();
        }
        public void exp_zero(int val) {
            exps[val].set0();
            list.del(val);
        }
        public void go_deeper(int deep) {
            int pnt,i0,i1;
            for (i0 = 0; i0 < list.root.vars; i0++) {
                pnt = list.root.var_to_val(i0);
                i1 = list.root.deep-1;
                while (i1 > list.root.deep - deep -1) {
                    if (!exps[pnt + i1].iszero()) list.root.sys.error(" too deep ");
                    i1--;
                }
                while (i1 > -1) {
                    exps[pnt + i1 + deep].set(exps[pnt + i1]);
                    list.set(pnt + i1 + deep, !exps[pnt + i1].iszero());
                    i1--;
                }
                while (i1 < deep-1) {
                    i1++;
                    exps[pnt + i1].set0();
                    list.del(pnt + i1);
                }
            }
        }
    }
    class many: power<many>, ipower
    {
        public ids root;
        public SortedDictionary<one,num> data;
        public many()
        {
            root = null;
            data = new SortedDictionary<one,num>();
        }
        public many(ids h)
        {
            root = h;
            data = new SortedDictionary<one,num>();
        }
        public many(ids h, int n)
        {
            root = h;
            data = new SortedDictionary<one,num>();
            data.Add(new one(root), new num(n));
        }
        public many(many m)
        {
            root = m.root;
            data = new SortedDictionary<one,num>();
            foreach (KeyValuePair<one,num> o in m.data) data.Add(new one(o.Key),new num(o.Value));
        }
        public override void set(many s)
        {
            root = s.root;
            data.Clear();
            foreach (KeyValuePair<one,num> o in s.data) data.Add(new one(o.Key),new num(o.Value));
        }
        public override void copy(ref many s)
        {
            s.set(this);
        }
        public override void set0()
        {
            data.Clear();
            data.Add(new one(root), new num(0));
        }
        public override void set1()
        {
            data.Clear();
            data.Add(new one(root), new num(1));
        }
        public override void div()
        {
            throw new NotImplementedException();
        }
        public bool equ(many m){
            if (data.Count != m.data.Count) return false;
            foreach (KeyValuePair<one,num> o in m.data) if (!data.ContainsKey(o.Key)) return false;
            foreach (KeyValuePair<one,num> o in data) 
            {
                if (!m.data.ContainsKey(o.Key)) return false;
                if (!o.Value.equ(m.data[o.Key])) return false;
            }
            return true;
        }

        public void add(KeyValuePair<one,num> o, int s) {
            if (data.ContainsKey(o.Key)) {
                data[o.Key].add(o.Value,s);
                data[o.Key].simple();
            } else data.Add(new one(o.Key),new num(o.Value,s));
        }
        public void add(many m, int s)
        {
            foreach (KeyValuePair<one,num> o in m.data) add(o,s);
        }
        public void mul(KeyValuePair<one,num> m)
        {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            one o; num n;
            while (data.Count > 0) {
                o = data.ElementAt(0).Key;
                n = data[o];
                data.Remove(o);
                o.mul(m.Key);
                n.mul(m.Value);
                if (r.ContainsKey(o)) r[o].add(n); else r.Add(o,n);
            }
            data = r;
            foreach (KeyValuePair<one,num> d in data) d.Value.simple();
        }
        public override void mul(many m)
        {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            one o = new one(root);
            foreach (KeyValuePair<one,num> m0 in m.data) {
                foreach (KeyValuePair<one,num> m1 in data) {
                    o.set(m0.Key);
                    o.mul(m1.Key);
                    if (r.ContainsKey(o)) r[o].addmul(m0.Value,m1.Value);
                    else r.Add(new one(o), new num(m0.Value,m1.Value));
                }
            }
            data = r;
            foreach (KeyValuePair<one,num> d in data) d.Value.simple();
        }
        public void expand(many m, KeyValuePair<one,num> on, int val)
        {
            one to = new one(on.Key); to.exps[val].set0();
            KeyValuePair<one,num> ton = new KeyValuePair<one,num>(new one(root), new num(0));
            foreach (KeyValuePair<one,num> o in m.data) {
                ton.Key.set(o.Key); ton.Key.mul(to);
                ton.Value.set(o.Value); ton.Value.mul(on.Value);
                add(ton,1);
            }
        }

        public new void exp(int e)
        {
            if (data.Count > 1) base.exp(e); else if (data.Count == 1) {
                data.ElementAt(0).Key.exp(e);
                data.ElementAt(0).Value.exp(e);
            }
        }
        public KeyValuePair<one,num> extract()
        {
            if (data.Count == 0) return new KeyValuePair<one,num>(new one(root), new num(1)); else 
            {
                one ro = null; num rn = null;
                foreach (KeyValuePair<one,num> m in data) {
                    if (ro == null) {
                        ro = new one(m.Key); rn = new num(m.Value);
                    } else {
                        ro.extract(m.Key);
                        rn.extract(m.Value);
                    }
                }
                return new KeyValuePair<one,num>(ro, rn);
            }
        }

/*
        public void diff(int val) {
            int _sz = root.size;
            num neg = new num(-1), tmp = new num(0), vexp;
            int i = 0, ii = data.Count; while (i < ii) 
            {
                if (data[i].exps[val].iszero()) { data.RemoveAt(i); ii--; }
                else
                {
                    tmp.set(data[i].exps[val].non);
                    data[i].exps[val].non.add(neg);
                    if (data[i].exps[val].vars != null)
                    {
                        one oadd;
                        for (int i0 = 0; i0 < _sz; i0++)
                        {
                            vexp = data[i].exps[val].vars[i0];
                            if (vexp.nonzero())
                            {
                                oadd = new one(data[i]);
                                oadd.exps[i0].non.add(vexp);
                                data.Add(oadd);
                            }
                        }
                    }
                    data[i].mult.mul(tmp);
                    i++;
                }
            }
        }
 */
        public void neg() {
            foreach (KeyValuePair<one,num> o in data) o.Value.neg();
        }

        public void go_deeper(int deep) {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            one o; num n;
            foreach (KeyValuePair<one,num> m in data)
            {
                o = new one(m.Key); n = new num(m.Value);
                o.go_deeper(deep);
                r.Add(o,n);
            }
            data = r;
        }

        public void add_toexp(int val, exp _e) 
        {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            one o; num n;
            foreach (KeyValuePair<one,num> m in data)
            {
                o = new one(m.Key); n = new num(m.Value);
                o.exps[val].add(_e,1);
                r.Add(o,n);
            }
            data = r;
        }

        public exp find_minexp(int val) //_val^(-x) -> /_val^(x)
        {
            exp _min = null;
            foreach (KeyValuePair<one,num> m in data)
                if (_min == null) _min = new exp(m.Key.exps[val]); else _min.min(m.Key.exps[val]);
            return _min;
        }

    }
    class exps 
    {
        public bool ready = false;
        public num max, min;
        ids root;
        int val,deep;
        public SortedDictionary<num,func> data;
        public exps(ids h, int v){
            root = h; min = new num(0); max = new num(0);
            data = new SortedDictionary<num,func>();
            data.Add(new num(0), new func(root,-1,0));
            val = v;
            deep = root.val_to_deep(val);
            func fs = root.values[root.val_to_var(val)]; if (fs == null) return;
            num e1 = new num(1);
            data.Add(e1, new func(fs));
            data[e1].go_deeper(deep);
            if ((data[e1].data[1].data.Count < 1) || (data[e1].data[-1].data.Count < 1)) return;
            ready = true;
        }
        void add(num t) {
            if (! data.ContainsKey(t)) data.Add(new num(t),new func(root,-1));
        }

        public void prep(many m){
            add(m);
            calc();
        }
        void add(many m){
            num t = new num(0); min.set0(); max.set0(); exp te;
            bool f_one = data[new num(1)].f_one;
            foreach (KeyValuePair<one,num> on in m.data) {
                te = on.Key.exps[val];
                if (te.isnum() && (f_one || te.non.isint() )) {
                    min.min(te.non);
                    max.max(te.non);
                }
            }
            add(min); add(max);
//[+1]^(up - min[0])*[-1]^(max[2] - up)
            foreach (KeyValuePair<one,num> on in m.data) {
                te = on.Key.exps[val];
                if (te.isnum() && (f_one || te.non.isint())) {
                    t.set(te.non); t.add(min,-1);
                    add(t);
                    t.set(max); t.add(te.non,-1);
                    add(t);
                }
            }
        }
        void calc(){
            num dl = new num(0), e1 = new num(1);
            foreach (KeyValuePair<num,func> d in data) {
                if (d.Value.data.Count == 0) {
                        foreach (KeyValuePair<num,func> d0 in data) {
                            if (d.Key.CompareTo(d0.Key) >= 0) break;
                            dl.set(d.Key); dl.add(d0.Key,-1);
                            if (data.ContainsKey(dl)) {
                                d.Value.set(d0.Value);
                                d.Value.mul(data[dl]);
                                break;
                            }
                        }
                        if (d.Value.data.Count == 0) {
                            d.Value.set(data[e1]);
                            d.Value.exp(d.Key);
                        }
                }
            }
        }
    }

    class func: power<func>, ipower
    {
        public ids root;
        public SortedDictionary<int,many> data;
        public int id;
        public int tfunc; //0: 0//1; 1:
        public num pfunc;
        public bool f_one;
        public func()
        {
            tfunc = -1; pfunc = new num(-1);
            root = null; id = -1;
            data = new SortedDictionary<int,many>();
        }
        public func(ids h, int var)
        {
            tfunc = -1; pfunc = new num(-1);
            root = h; id = var;
            data = new SortedDictionary<int,many>();
        }
        public func(ids h, int var, int t)
        {
            pfunc = new num(-1);
            root = h; id = var;
            data = new SortedDictionary<int,many>();
            set(t);
        }
        public func(func f)
        {
            data = new SortedDictionary<int,many>();
            set(f);
        }
        public void check(){
            bool r = true;
            foreach (KeyValuePair<int,many> m in data) if (m.Value.data.Count > 1) r = false;
            f_one = r;
        }
        public void set(int t) {
            tfunc = t;
            switch(t) {
                case 0:
                    data.Add(1,new many(root,0));
                    data.Add(-1,new many(root,1));
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
            check();
        }
        public override void set(func f)
        {
            tfunc = f.tfunc; pfunc = new num(f.pfunc);
            root = f.root; id = f.id;
            data.Clear();
            foreach (KeyValuePair<int,many> m in f.data) data.Add(m.Key,new many(m.Value));
            check();
        }
        public override void copy(ref func f)
        {
            f.set(this);
        }
        public override void set0()
        {
            foreach (KeyValuePair<int,many> m in data) if (m.Key < 0) m.Value.set1(); else m.Value.set0();
            check();
        }
        public override void set1()
        {
            foreach (KeyValuePair<int,many> m in data) m.Value.set1();
            check();
        }
        public override void div()
        {
            if (tfunc == 0) {many t = data[-1]; data[-1] = data[1]; data[1] = t;}
        }
        public override void mul(func f)
        {
            if ((tfunc != f.tfunc) || (root != f.root)) root.sys.error("!");
            switch (tfunc) {
                case 0:
                    foreach (KeyValuePair<int,many> m in data) m.Value.mul(f.data[m.Key]);
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
            check();
        }
        public void exp(exp e) {
            if (e.vars == null) exp(e.non); else data.Clear(); 
        }
        public void exp(num e) {
            check();
            if (f_one) {
                foreach (KeyValuePair<int,many> m in data) {
                    m.Value.data.ElementAt(0).Key.exp(e);
                    m.Value.data.ElementAt(0).Value.exp(e);
                }
            } else {
                if (e.get_down() > 1) data.Clear(); else exp((int)e.get_sup());
            }            
            check();
        }
        public void add(func f)
        {
            if ((tfunc != f.tfunc) || (root != f.root)) root.sys.error("!");
            switch (tfunc) {
                case 0:
                    if (data[-1].equ(f.data[-1])) data[1].add(f.data[1],1); else {
                        many t = new many(f.data[1]); t.mul(data[-1]);
                        data[1].mul(f.data[-1]); data[1].add(t,1);
                        data[-1].mul(f.data[-1]);
                    }
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
            check();
        }
        public void simple()
        {
            if (tfunc != 0) return;
            many _up = data[0], _dw = data[1];
            KeyValuePair<one,num> f_up, f_down, f_both;
            f_up = data[1].extract();
            f_down = data[-1].extract();
            f_both = new KeyValuePair<one,num>(new one(f_up.Key), new num(f_up.Value));
            f_both.Key.extract(f_down.Key);
            f_both.Value.extract(f_down.Value);
            f_both.Key.div(); f_both.Value.div();
            data[1].mul(f_both);
            data[-1].mul(f_both);

            f_up.Value.set1();
            foreach(int iu in f_up.Key.list) if (f_up.Key.exps[iu].ispos()) f_up.Key.exp_zero(iu);
            f_down.Value.set1();
            foreach(int id in f_down.Key.list) if (f_down.Key.exps[id].ispos()) f_down.Key.exp_zero(id);

            f_up.Key.div();
            data[1].mul(f_up);
            data[-1].mul(f_up);
            f_down.Key.div();
            data[1].mul(f_down);
            data[-1].mul(f_down);
            check();
        }

        public void go_deeper(int deep) {
            foreach (KeyValuePair<int,many> m in data) m.Value.go_deeper(deep);
            check();
        }

        public void expand(int val)
        {
            exps ex = new exps(root,val);
            if (! ex.ready) return;
            expand(+1,val,ex);
            num minup = new num(ex.min), maxup = new num(ex.max);
            expand(-1,val,ex);
            data[+1].mul(ex.data[ex.min].data[+1]);
            data[+1].mul(ex.data[ex.max].data[-1]);
            data[-1].mul(ex.data[minup].data[+1]);
            data[-1].mul(ex.data[maxup].data[-1]);
            simple();
        }

        public void expand(int up, int val, exps ex)
        {
            SortedDictionary<num,many> ml = new SortedDictionary<num,many>();
            many res = new many(root);
            ex.prep(data[up]);
            num e = new num(0), eu = new num(0), ed = new num(0);
            foreach (KeyValuePair<one,num> o in data[up].data) 
            {
//[+1]/[-1]
//[-up] *= [+1]^min*[-1]^max 
//[+1]^(up - min[0])*[-1]^(max[2] - up)

                if (o.Key.exps[val].isnum()) {
                    e = o.Key.exps[val].non;
                    eu.set(e); eu.add(ex.min,-1);
                    ed.set(ex.max); ed.add(e,-1);
                    if (ex.data.ContainsKey(eu) && ex.data.ContainsKey(ed)) {
                        if (! ml.ContainsKey(e)) {
                            ml.Add(new num(e),new many(ex.data[eu].data[+1]));
                            ml[e].mul(ex.data[ed].data[-1]);
                        }
                        res.expand(ml[e],o,val);
                    } else res.add(o,1);
                } else res.add(o,1);
            }
            data[up] = res;
        }

        public bool revert(int val, int up) //_val^(-x) -> /_val^(x)
        {
            exp min = data[up].find_minexp(val);
            if (min.iszero()) return false;
            min.neg();
            data[up].add_toexp(val,min);
            data[-up].add_toexp(val,min);
            check();
            return true;
        }
        public void revert(int val)
        {
            revert(val,-1);
            revert(val,+1);
        }
/*        public bool revert()
        {
            if (data[1].Count > 1) return false;
            one tmp = data[1][0]; tmp.div();
            mul(0, tmp); simple(0);
            data[1][0] = new one(this, 1);
            return true;
        }*/

        public string print(int _f)
        {
            bool hasdiv;
            string s0 = root.get_name(id) + " =";
            int i;
            switch (tfunc) {
                case 0:
                    s0 += "/" + (pfunc.get_sup() > 0 ? pfunc.get_sup().ToString() : "") + " ";
                    break;
                case 1:
                    s0 += "& ";
                    break;
                case 2:
                    s0 += "^" + root.get_name_onval((int)(pfunc.get_up())) + " ";
                    break;
                default:
                    s0 += " " ;
                    break;
            }
            if (data[0].Count == 0) s0 += "0"; else 
            if ((data[0].Count > 0) && (data[1].Count > 0)) 
            {
                hasdiv = ((data[1].Count > 1) || (!data[1][0].mult.isone()) || (data[1][0].mult.get_sign() < 0));
                for (i = 0; i < root.size; i++) if (!data[1][0].exps[i].iszero()) hasdiv = true;
                if (hasdiv) {
                    s0 = print(_f,0,s0); s0 = print(_f,1,s0 + "//");
                } else {
                    if ((data[0].Count == 1) && data[0][0].mult.iszero()) s0 += "0"; else s0= print(_f,0,s0);
                }
            }
            if (_f > -1) root.sys.wline(_f,s0);
            return s0;
        }
        public string print(int _f, int n, string s0)
        {
            int i0, i1;
            bool f_one, f_many = true;
            string s1;
            for (i0 = 0; i0 < data[n].Count; i0++)
            {
                if (data[n][i0].mult.nonzero())
                {
                    f_one = true;
                    for (i1 = 0; i1 < data[n][i0].head.head.size; i1++)
                    {
                        if (!data[n][i0].exps[i1].iszero())
                        {
                            s1 =  data[n][i0].exps[i1].print(data[n][i0].head.head.get_name_onval(i1));
                            if (f_one) 
                                s0 += data[n][i0].mult.print((f_many ? "" : "+"),"-",(s1[0] == '/') ? "" : "*") + s1;
                             else 
                                s0 += ((s1[0] == '/') ? "" : "*") + s1;
                            
                            f_one = false;
                        }
                    }
                    if (f_one) s0 += data[n][i0].mult.print((f_many ? "" : "+"),"-", "");
                    f_many = false;
                }
                root.sys.progr(i0,data[n].Count);
                if ((s0.Length > 666) && (_f > -1)) {root.sys.wstr(_f,ref s0); s0 = "";}
            }
            return s0;
        }
        public num calc()
        {
            num rt = new num(0);
                if (tfunc < 2)
                {
                    rt.set(calc(0));
                    rt.div(calc(1));
                    rt.simple();
                }
                switch (tfunc)
                {
                    case 0:
                        {
                            int l0, l1,l = (int)(pfunc.get_sup());
                            if (l < 1) {
                                rt.set(rt.get_sup()/rt.get_down(),1);
                            } else {
                            l0 = rt.get_up().ToString().Length;
                            l1 = rt.get_down().ToString().Length;
                            if (l0 > l1) l0 = l1;
                            l0 -= l;
                            if (l0 > 4)
                            {
                                string _s = "1" + new string('0', l0);
                                BigInteger _d, _au, _ad, _bu, _bd, _cu, _cd; BigInteger.TryParse(_s, out _d);
                                _au = rt.get_up() / _d; _bu = rt.get_up() % _d; _cu = _bu / _au;
                                _ad = rt.get_down() / _d; _bd = rt.get_down() % _d; _cd = _bd / _ad;
                                _d += (_cu + _cd) / 2;
                                rt.set(rt.get_sign(), rt.get_up() / _d, rt.get_down() / _d);
                            }
                            }
                        }
                        break;
                    case 1:
                        {
                            if (rt.nonzero()) rt.set_up(1); rt.set_down(1);
                        }
                        break;
                    case 2:
                        {
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
                        }
                        break;
                }
                rt.simple();

            return rt;
        }
        num calc(int ud)
        {
            int i;
            num tr = new num(0), t0 = new num(0);
            foreach (one u in data[ud])
            {
                t0.set(u.mult);
                one tmp = u;
                if (u.islist) for (i = 0; u.list[i] > -1; i++) t0.mul(calc_exp(tmp.exps[u.list[i]],u.list[i]));
                else for (i = 0; i < head.size; i++) if (!u.exps[i].iszero()) t0.mul(calc_exp(tmp.exps[i],i));
                tr.add(t0);
            }
            return tr;
        }
        num calc_exp(exp ex, int i)
        {
            num t1 = new num(head.get_val(i));
            num en = new num(ex.non);
            if (ex.vars != null) for (int ii = 0; ii < head.size; ii++)
            {
                if (ex.vars[ii].nonzero()) 
                {
                    num aa = new num(head.get_val(ii));
                    aa.mul(ex.vars[ii]);
                    en.add(aa);
                }
                en.simple();
            }
            if ((en.get_down() > 42) && (en.get_up() > 1000)) head.sys.error(head.get_name(id) + " in wrong exp = " + en.print("","-", ""));
            t1.exp((int)(en.get_up()));
            if (en.get_down() > 1)
            {
                int _sq = (int)en.get_down();
                if (!t1.root(_sq))
                {
                    if ((_sq % 2 == 0) && (t1.get_sign() < 0)) head.sys.error("even square from neg");
                    if (((t1.get_up() > 1) && (t1.get_up() < head.prec.get_up())) || ((t1.get_down() > 1) && (t1.get_down() < head.prec.get_down())))
                    {
                        t1.mul(head.prec);
                    }
                    t1.set(t1.get_sign(),t1._sq(t1.get_up(), _sq),t1._sq(t1.get_down(), _sq));
                }
            }
            if (en.get_sign() < 0) t1.div();
            return t1;
        }




    }

    class mao_dict {
        static short bmexp = 11;
        static int mexp = 1 << bmexp;
        public ids root;
        public int nvals;
        public num[] exps;
        public int[] vals, to_val;
        ushort[] eneg,eadd; bool[] eflg_a;
        ushort lexp, lval;
        public mao_dict(int v, ids r)
        {
            nvals = v; root = r;
            exps = new num[mexp];
            vals = new int[nvals];
            to_val = new int[root.size];
            for (int i = 0; i < root.size; i++) to_val[i] = -1;
            eneg = new ushort[mexp * mexp]; eadd = new ushort[mexp * mexp]; eflg_a = new bool[mexp * mexp];
            for (uint i = 0; i < mexp * mexp; i++) { eneg[i] = 0xFFFF; eadd[i] = 0xFFFF; eflg_a[i] = true; }
            lexp = 2; exps[0] = new num(0); exps[1] = new num(1);
            lval = 0;
        }
        public ushort exp(num e)
        {
            ushort i;
            for (i = 0; i < lexp; i++) if (e.cmp(exps[i])) return i;
            if (lexp > mexp-2) root.sys.error("too many exp");
            exps[lexp++] = new num(e);
            return i;
        }
        public int val(int v)
        {
            if (to_val[v] < 0)
            {
                if (lval >= nvals) root.sys.error(" ! too many var");
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
                eflg_a[ee] = ((sum.get_up() == 0) || (exps[e1].get_up() == 0) || (sum.get_sign() != exps[e1].get_sign()));
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
        public KeyValuePair<mao_key,num> fr_one(one o)
        {
            int i0,v0;
            KeyValuePair<mao_key, num> ret = new KeyValuePair<mao_key,num>(new mao_key(dict), new num(o.mult));
            for (i0 = 0; i0 < dict.nvals; i0++) ret.Key.key[i0] = 0;
            for (i0 = 0; i0 < dict.root.size; i0++)
            {
                if (o.exps[i0].vars != null) dict.root.sys.error("cant fast on complex exp");
                if (o.exps[i0].non.nonzero())
                {
                    v0 = dict.val(i0);
                    ret.Key.key[v0] = dict.exp(o.exps[i0].non);
                }
            }
            return ret;
        }
        public one to_one(KeyValuePair<mao_key, num> fr, many m)
        {
            int i;
            one ret = new one(m, fr.Value);
            for (i = 0; i < dict.nvals; i++)
            {
                if (fr.Key.key[i] != 0) 
                    ret.exps[dict.vals[i]].non.set(dict.exps[fr.Key.key[i]]);
            }
            ret.simple();
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
            one tmp;
            dict = d;
            _data_i();
            foreach (one o in f.data[0]) add(0, fr_one(o));
            foreach (one o in f.data[1]) add(1, fr_one(o));
        }
        public func to_func(ids h, int var)
        {
            int i=0, cn = data[0].Count + data[1].Count;
            func ret = new func(h,var); ret.data[1].RemoveAt(0);
            foreach (KeyValuePair<mao_key, num> d in data[0]) {ret.data[0].Add(to_one(d, ret)); dict.root.sys.progr(i++,cn);}
            foreach (KeyValuePair<mao_key, num> d in data[1]) {ret.data[1].Add(to_one(d, ret)); dict.root.sys.progr(i++,cn);}
            return ret;
        }

        public many_as_one(many_as_one _m, int _e)
        {
            dict = _m.dict;
            many_as_one tmp = new many_as_one(dict);
            many_as_one _tmp = new many_as_one(dict);
            many_as_one fr = new many_as_one(dict);
            num exp = dict.exps[_e], nexp = new num(0);
            int i0,_eu = (int)(exp.get_up());
            fr.set(_m);
            if (exp.get_down() > 1) {
                if ((fr.data[0].Count > 1) || (fr.data[1].Count > 1)) return;
                if (! fr.data[0].ToArray()[0].Value.root((int)exp.get_down())) return;
                if (! fr.data[1].ToArray()[0].Value.root((int)exp.get_down())) return;
                for (i0 = 0; i0 < dict.nvals; i0++) 
                {
                    nexp.set(dict.exps[fr.data[0].ToArray()[0].Key.key[i0]]);
                    nexp.mul(new num(1,new BigInteger(1),exp.get_down()));
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

            if (exp.get_sign() < 0) {tmp.data[0] = data[0]; data[0] = data[1]; data[1] = tmp.data[0];}
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
            if ((e.data[0].Count < 1) || (e.data[1].Count < 1)) dict.root.sys.error("wrong");
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
                dict.root.sys.progr(pnow++,data[n].Count);
            }
            max_d.neg();
            for (tex = 0; tex < 254; tex++) 
            {
                if (ae[tex] != null) 
                {
                    if (tex > 0) ret = true;
                    now_u.set(max_u); now_d.set(max_d);
                    if (dict.exps[tex].get_down() == 1) 
                    {
                        if (dict.exps[tex].get_sign() > 0) now_u.add_up(0 - dict.exps[tex].get_up());
                        else now_d.add_up(0 - dict.exps[tex].get_up());
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
                dict.root.sys.progr(tex,254);
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
                dict.root.sys.progr(tex,254);
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


    class fileio: IDisposable
    {
        StreamReader fin, f611;
        StreamWriter[] fout;
        parse head;
        int nline,ncline, lines, clines;
        string buf,nout,xout;
        public Boolean has, quit;
        public fileio(string nin, string _nout, parse h)
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
            head = h; buf = fin.ReadLine() + "\n" + fin.ReadLine();
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
            fout[0].WriteLine(head.val);
            fout[0].WriteLine("Line {0:G} Pos {0:G}: " + e, nline+1, head.pos);
            fout[0].Flush();
            Environment.Exit(-1);
        }
        public void wline(int n, string s)
        {
            if (fout[n] == null) fout[n] = new StreamWriter(nout + parse.m_num_to_str(n) + xout);
            fout[n].WriteLine(s);
            fout[n].Flush();
        }
        public void wstr(int n, ref string s)
        {
            if (fout[n] == null) fout[n] = new StreamWriter(nout + parse.m_num_to_str(n) + xout);
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
    class parse: IDisposable
    {
        public string val,delim,name;
        public int pos;
        List<string> m_name, macro;
        List<int> m_nparm;
        public fileio sys;
        public parse(string nin, string nout, string d)
        {
            sys = new fileio(nin,nout,this); 
            delim = d; val = ""; pos = 0;
            m_name = new List<string>(); macro = new List<string>(); m_nparm = new List<int>();
        }
        public bool isnum(char c)
        {
            return ((c >= '0') && (c <= '9'));
        }
        public bool isdelim(char c)
        {
            return (delim.IndexOf(c) > -1);
        }
        public bool isnum(string s, int i)
        {
            if (s.Length <= i) return false;
            return isnum(s[i]);
        }
        public int find_deep(int from, int deep, string _delim) {
            int d = deep, i = from;
            while (i < val.Length) {
                switch (val[i]) {
                    case '(':
                        d++;
                        break;
                    case ')':
                        d--;
                        break;
                }
                if (d < 0) return i;
                if ((d == 0) && (_delim.IndexOf(val[i]) > -1)) return i;
                i++;
            }
            return -1;
        }
        public bool isnum(string s)
        {
            if (s.Length < 1) return false;
            for (int i = 0; i < s.Length; i++) if (!isnum(s, i)) return false;
            return true;
        }
        public string m_parm(){
            string s1 = "";
            int i1;
            if (now() == '(') {
                s1 = calc0().get_sup().ToString(); 
            }
            else
            {
                i1 = find_deep(pos, 0,",<>");
                if (i1 < pos) sys.error("wrong parm");
                s1 = val.Substring(pos,i1-pos);
                s1 = s1.Replace("&0", "#");
                s1 = s1.Replace("&1", "&0");
                s1 = s1.Replace("&2", "&1");
                s1 = s1.Replace("&3", "&2");
                s1 = s1.Replace("&4", "&3");
                pos = i1;
            }
            return s1;
        }
        static char[] m_n_to_c = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        static int[] m_c_to_n = {
        -1,-1,-1,-1, -1,-1,-1,-1,  -1,-1,-1,-1, -1,-1,-1,-1,
        -1,-1,-1,-1, -1,-1,-1,-1,  -1,-1,-1,-1, -1,-1,-1,-1,

        -1,-1,-1,-1, -1,-1,-1,-1,  -1,-1,-1,-1, -1,-1,-1,-1,
         0, 1, 2, 3,  4, 5, 6, 7,   8, 9,-1,-1, -1,-1,-1,-1,

        -1,10,11,12, 13,14,15,16,  17,18,19,20, 21,22,23,24,
        25,26,27,28, 29,30,31,32,  33,34,35,-1, -1,-1,-1,-1,

        -1,10,11,12, 13,14,15,16,  17,18,19,20, 21,22,23,24,
        25,26,27,28, 29,30,31,32,  33,34,35,-1, -1,-1,-1,-1};
        static public string m_num_to_str(int _n) {
            return (m_n_to_c[_n]).ToString();
        }
        static public int m_char_to_num(ref string _v, int _p) {
            if (_v.Length <= _p) return -1;
            if (_v[_p] < m_c_to_n.Length) return m_c_to_n[_v[_p]]; else return -1;
        }
        public bool next()
        {
            string s0,s1,st,sf;
            int _np,_nm,i0,i1,i2;//,i3,i4,i5,i6, deep;
            bool l_add = true;
            val = sys.rline(); val = val.Replace(" ",""); pos = 0;
            if ((val.Length == 0) || (val[0] == '`')) return false;
            if (val.Substring(0,2) == "##")
            {
                pos = 2; name = snext(false);
                if ((_np = m_char_to_num(ref val,pos+1)) < 0) sys.error("macro: wrong num");
                _nm = -1;
                string _m = val.Substring(pos + 2);
                for (i0 = 0; i0 < m_name.Count; i0++) if (m_name[i0].IndexOf("#" + name + "(") > -1) _nm = i0;
                for (i0 = 0; i0 < m_n_to_c.Length; i0++)
                {
                    if (_m.IndexOf("#" + m_num_to_str(i0)) > -1)
                    {
                        if (i0 >= _np) sys.error("macro: used nonparm");
                    }
                }
                if (_nm < 0) {
                    m_name.Add("#" + name + "("); m_nparm.Add(_np); macro.Add(_m);
                } else {
                    if (_np != m_nparm[_nm]) sys.error("macro: wrong num");
                    macro[_nm] += "\n" + _m;
                }
                return false;
            }
            while ((i1 = val.LastIndexOf("#")) > -1) {
                for (i0 = 0; i0 < macro.Count; i0++) if (i1 == val.IndexOf(m_name[i0], i1)) break;
                if (i0 == macro.Count) sys.error("macro: not found");
                s0 = macro[i0].Replace("`\n","");
                int ploop = -1,floop = 0,tloop = 0;
                for (i2 = 0, pos = i1 + m_name[i0].Length; i2 < m_nparm[i0]; i2++, snext(false))
                {
                    s1 = m_parm();
                    if ("<>".IndexOf(now()) > -1) { 
                        l_add = (snext(false) == "<");
                        if (ploop > -1) sys.error("macro: wrong loop");
                        if (! int.TryParse(s1, out floop)) sys.error("macro: wrong loop");
                        if (!int.TryParse(m_parm(), out tloop)) sys.error("macro: wrong loop");
                        ploop = i2;
                    } else {
                        s0 = s0.Replace("#" + m_num_to_str(i2), s1);
                    }
                    if (((i2 == m_nparm[i0] - 1) && (now() != ')')) || ((i2 < m_nparm[i0] - 1) && (now() != ','))) 
                        sys.error("macro: call nparm");
                }
                if (m_nparm[i0] == 0) snext(false);
                sf = val.Substring(0, i1); st = (pos < val.Length ? val.Substring(pos, val.Length - pos) : "");
                if (ploop < 0) val = sf + s0 + st;
                else
                {
                    if (l_add) for (s1 = "", i2 = floop; i2 <= tloop; i2++) s1 += s0.Replace("#" + m_num_to_str(ploop), i2.ToString().Trim());
                    else for (s1 = "", i2 = floop; i2 >= tloop; i2--) s1 += s0.Replace("#" + m_num_to_str(ploop), i2.ToString().Trim());
                    val = sf + s1 + st;
                }
            }
            i1 = val.IndexOf("\n");
            if (i1 > -1) {
                sys.addline(val.Substring(i1+1)); val = val.Substring(0,i1);
            }
            val = val.Replace("&%","#");
            val = val.Replace("&^"," ");
            pos = 0; name = snext(false); return true;
        }
        public bool more() { return pos < val.Length; }
        public char now() { return (more() ? val[pos] : '\0'); }
        public string snext(bool skp)
        {
            int i0;
            if (!more()) return "";
            if (skp && (delim.IndexOf(now()) > -1)) pos++;
            if (!more()) return "";
            i0 = pos;
            if (now() == '"') {
                pos++; while (more() && (now() != '"')) pos++; pos++;
                return val.Substring(i0, pos - i0 - 1);
            } else {
                if (delim.IndexOf(now()) > -1) pos++;
                else while (more() && (delim.IndexOf(now()) == -1)) pos++;
                return val.Substring(i0, pos - i0);
            }
        }
        public num nnext(bool skp)
        {
            string s = snext(skp);
            if (s.Length < 1) sys.error("nonum in calc");
            if (s[0] == '(') return calc();
            if (!isnum(s)) sys.error("nonum in calc");
            return new num(s);
        }
        public num calc()
        {
            num ret = new num(0);
            string st;
            while (true)
            {
                st = snext(false);
                if (st.Length < 1) sys.error("unfinished calc");
                switch (st[0])
                {
                    case ')': ret.simple();  return ret;
                    case '+':
                        ret.add(nnext(false));
                        break;
                    case '-':
                        ret.sub(nnext(false));
                        break;
                    case '*':
                        ret.mul(nnext(false));
                        break;
                    case '/':
                        ret.div(nnext(false));
                        break;
                    case '^':
                        ret.exp(nnext(false));
                        break;
                    case '(':
                        ret.set(calc());
                        break;
                    default:
                        if (!isnum(st)) sys.error("nonum in calc");
                        ret.set(st);
                        break;
                }
            }
        }
        public num calc0() {pos++; return calc();}
        public void Dispose()
        {
            sys.Dispose();
        }
        public BigInteger get_parm(ids root)
        {
            num tmp;
            if (isnum(now()) || (now() == '(')) tmp = nnext(false); else  tmp =  root.get_val(root.find_val(snext(false)));
            return tmp.toint();
        }

    }

    static class Program
    {
        public static shard0 m0;
        public static System.Drawing.Bitmap bm1;
        static parse par;
        static void parseone(parse par, ids root, int i, one data)
        {
          string s;
          int val = -1;
          bool div = false;
          if (par.now() == '+') par.pos++; else if (par.now() == '-') {par.pos++; data.mult.neg();}
          while (true)
          {
              s = par.snext(false); if (s.Length < 1) return;
              if (par.isdelim(s[0]))
              {
                  switch (s[0])
                  {
                      case '/':
                          if (par.now() == '/') 
                              return;
                          div = true;
                          break;
                      case '*':
                          div = false;
                          break;
                      case '+':
                      case '-':
                          par.pos--; return;
                      case '(':
                          if (div) data.mult.div(par.calc()); else data.mult.mul(par.calc());
                          break;
                      case '^':
                          if (val < 0) par.sys.error("wrong exp");
                          {
                              exp tn = new exp(data.head,0);
                              if (par.now() != '(') {
                                  if (par.isnum(par.now())) tn.non.set(par.nnext(false)); else tn.addvar(par.snext(false),1);
                              } else {
                                  int deep = 1, i0 = par.pos + 1;
                                  bool isname = false;
                                  while ((i0 < par.val.Length) && (deep > 0)) {
                                      if (par.isdelim(par.val[i0])) {
                                          switch (par.val[i0]) {
                                              case '(':
                                                  deep++;
                                                  break;
                                              case ')':
                                                  deep--;
                                                  break;
                                          }
                                      } else {
                                          if (!par.isnum(par.val[i0])) isname = true;
                                      }
                                      i0++;
                                  }
                                  if ((deep > 0)) par.sys.error("wrong exp");
                                  if (isname) {
                                      par.snext(false);
                                      num _now = new num(), _add = new num(0);
                                      bool flg = true;
                                      int _sign = 0, _val = -1; _now.unset();
                                      while (flg) {
                                          if (par.isdelim(par.now())) {
                                              switch(par.now()){
                                                  case ')':
                                                      flg = false;
                                                      par.snext(false);
                                                      break;
                                                  case '+':
                                                      if (_now.exist()) {
                                                          if (_val < 0) _add.add(_now); else tn.addvar(_val,_now);
                                                          _now.unset();
                                                      }
                                                      _sign = 1; _val = -1; par.snext(false);
                                                      break;
                                                  case '-':
                                                      if (_now.exist()) {
                                                          if (_val < 0) _add.add(_now); else tn.addvar(_val,_now);
                                                          _now.unset();
                                                      }
                                                      _sign = -1; _val = -1; par.snext(false);
                                                      break;
                                                  case '*':
                                                      par.snext(false);
                                                      if (par.isnum(par.now())) {
                                                            if (!_now.exist()) par.sys.error("wrong exp");
                                                            _now.mul(par.nnext(false));
                                                      } else if (par.now() == '(') {
                                                            if (!_now.exist()) par.sys.error("wrong exp");
                                                            _now.mul(par.calc0());
                                                      } else {
                                                            if (!_now.exist()) {
                                                                if (_sign < 0) _now.set(-1); else _now.set(1);
                                                                _sign = 0;
                                                            }
                                                            if (_val > -1) par.sys.error("wrong exp");
                                                            _val = root.find_val(par.snext(false));
                                                      }
                                                      break;
                                                  case '(':
                                                      _now.set(par.calc0());
                                                      if (_sign < 0) _now.neg();
                                                      _sign = 0;
                                                      break;
                                                  default:
                                                      par.sys.error("wrong exp");
                                                      break;
                                              }
                                          } else {
                                              if (par.isnum(par.now())) {
                                                  _now.set(par.nnext(false));
                                                  if (_sign < 0) _now.neg();
                                                  _sign = 0;
                                              } else {
                                                  if (!_now.exist()) {
                                                      if (_sign < 0) _now.set(-1); else _now.set(1); 
                                                      _sign = 0;
                                                  }
                                                  if (_val > -1) par.sys.error("wrong exp");
                                                  _val = root.find_val(par.snext(false));
                                              }
                                          }
                                      }
                                      if (_now.exist()) {
                                          if (_val < 0) _add.add(_now); else tn.addvar(_val,_now);
                                      }
                                      tn.non.add(_add);
                                  } else {
                                      tn.non.add(par.calc0());
                                  }
                              }
                              data.exps[val].add(tn,(div ? -1: 1));
                          }
                          break;
                      default:
                          par.sys.error("wrong");
                          break;
                  }
              }
              else
              {
                  if (par.isnum(s))
                  {
                      if (div) data.mult.div(new num(s)); else data.mult.mul(new num(s));
                      val = -1;
                  }
                  else
                  {
                      val = root.find_val(s);
                      if (par.now() != '^') data.exps[val].add(new exp(data.head,1),div ? -1 : 1);
                  }
              }
          }
        }


        [STAThread]
        static int Main(string[] args)
        {
            int sx=0, sy=0;
            if (args.Length < 1) return 0;
            string ss = (args.Length < 2 ? "" : args[1]);
            par = new parse(args[0], ss, "#&!@$+-=*/^(),~:<>\"[];");
            par.next();
            sx = (int)par.nnext(true).get_up();
            sy = (int)par.nnext(true).get_up();
            if ((sx < 100) || (sy < 100)) return -1;
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            m0 = new shard0(sx, sy);
            bm1 = new System.Drawing.Bitmap(sx, sy);
            for (int i0 = 0; i0 < sx; i0++) for (int i1 = 0; i1 < sy; i1++) bm1.SetPixel(i0, i1, Color.FromArgb(0, 0, 0));
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

        static void doit() {
            int var0,var1,var2,val0;
            string val;
            int x0,x1,f0,f1,c0,c1;
            int[] xid = new int[99];
            int _r0,_r1,_r2; double _d0,_d1,x2;
            par.next(); ids root = new ids(par.nnext(true).get_up(),par.nnext(true).get_up(),par.nnext(true).get_up(), par.sys);
            while (par.sys.has)
            {
                if (par.next()) 
                {
                    switch (par.now()) 
                    {
                     case '=':
                        par.snext(false);
                        if ((var0 = root.find_var(par.name)) < 0) var0 = root.set_empty(par.name);
                        int nowdiv = 0;
                        if (par.more()) {
                            root.values[var0] = new many(root,var0);
                            switch (par.now())
                            {
                            case '/': //precusion
                                par.snext(false); root.values[var0].tfunc = 0;
                                if (!par.isdelim(par.now())) root.values[var0].pfunc = par.nnext(false);
                                break;
                            case '^': //row
                                root.values[var0].tfunc = 1;
                                
                                
                                
                                
                                root.values[var0].pfunc.set(root.find_val(par.snext(true)));
                                if (root.values[var0].pfunc.get_sup() < 0) par.sys.error("row: no var");





                                break;
                            case '&': // -1 | +1
                                root.values[var0].tfunc = 2; root.values[var0].pfunc.set(0); par.snext(false);
                                break;
                            case '!': //factor
                                root.values[var0].tfunc = 3; root.values[var0].pfunc.set(0); par.snext(false);
                                break;
                            }
                            while (par.more())
                            {
                                root.values[var0].data[nowdiv].Add(new one(root.values[var0], new num(1)));
                                parseone(par, root,var0,root.values[var0].data[nowdiv][root.values[var0].data[nowdiv].Count-1]);
                                if (par.now() == '/')
                                {
                                    if ((par.more()) && (nowdiv == 0))
                                    {
                                        nowdiv = 1; root.values[var0].data[1].RemoveAt(0);  par.pos++; 
                                    } else par.sys.error("wrong");
                                }
                            }
                            if (root.values[var0].tfunc == 2) root.values[var0].revert_mult(0); 
                            else root.values[var0].simple();
                            root.values[var0].print(0); 
                        }
                     break;
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
                                root.set_var(xid[0], new num(1, _fr, _one));
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
            par.sys.wline(0,"finished, vars free = " + (root.vars - root.last-1).ToString());
            par.sys.close();
        }
    }
}
