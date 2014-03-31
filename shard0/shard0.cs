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

    class vars
    {
        public string name;
        public int val_f,val_n;
        public func var;
        public vars(string n, int vf, int vn)
        {
            name = n; val_f = vf; val_n = vn; var = null;
        }
    }
    class ids
    {
        public int vals_last,vars_last, stat_uncalc, stat_calc, exp_prec;
        public num prec;
        public BigInteger exp_max;
        public SortedDictionary<string,int> names;
        public vars[] vars;
        num[] vals_calc;
        int[] vals_var;
        public int[] calc_stat;
        public fileio sys;
        public func fzero;
        public one ozero;
        public num nzero;
        public string[] funcs_name = {"","fact"};
        public SortedDictionary<string,int> fnames;
        public ids(int _vars, int _vals, BigInteger p, BigInteger e, fileio f)
        {
            if ((_vars < 11) || (_vals < _vars) || (_vars + _vals > 6666) || (p < 11)) sys.error("wrong init");
            prec = new num(p,p); sys = f; vals_last = 0; vars_last = 0;
            stat_uncalc = 1; stat_calc = 2;
            names = new SortedDictionary<string,int>();
            fnames = new SortedDictionary<string,int>();
            vars = new vars[_vars];
            vals_calc = new num[_vals];
            vals_var  = new int[_vals];
            calc_stat = new int[_vars];
            exp_max = e;
            exp_prec = (int)(BigInteger.Log(e,10)/2);
            fzero = new func(this,-1,0);
            ozero = new one(this);
            nzero = new num(0);
            for (int i = 0; i < funcs_name.Count(); i++) fnames.Add(funcs_name[i],i);
        }
        int deep(string n)
        {
            int i = 0; while ((i < n.Length) && (n[i]=='\'')) i++;
            return i;
        }
        public int size() { return vals_var.Count();}
        public int find_val(string n)
        {
            int i,f; i = deep(n); string n0 = n.Substring(i);
            if (!names.ContainsKey(n0)) sys.error(n0 + " var not found"); 
            f = names[n0];
            if (i > vars[f].val_n) sys.error(n0 + " too deep"); 
            return vars[f].val_f+i;
        }
        public int find_var(string n)
        {
            int i = deep(n); string n0 = n.Substring(i);
            if (names.ContainsKey(n0)) {
                if (vars[names[n0]].val_n != i) sys.error(n0 + " not deep");
                return names[n0];
            }
            names.Add(n0,vars_last);
            return add_var(n0,i);
        }
        public int add_var(string n, int s) {
            int i = s;
            if ((vars_last >= vars.Count()) || (vals_last >= vals_var.Count())) sys.error("too many vars");
            vars[vars_last] = new vars(n,vals_last,i);
            calc_stat[vars_last] = 0;
            while (i > -1) {
                vals_calc[vals_last] = new num();
                vals_var[vals_last] = vars_last;
                i--; vals_last++;
            }
            vars_last++; return vars_last - 1;
        }
        public void uncalc() { stat_uncalc+=2; stat_calc+=2;}
        private void uncalc_var(int var)
        {
            for (int ii = vars[var].val_n - 1; ii > 0; ii--) vals_calc[vars[var].val_f + ii].set(vals_calc[vars[var].val_f + ii-1]);
            vals_calc[vars[var].val_f].unset(); calc_stat[var] = stat_uncalc;
        }
//          val           var
//old    uncalc,calc  uncalc,calc
//uncalc  get           recurs
//calc    get             get
        public num get_val(int val)
        {
           int var = vals_var[val]; 
           int nowdeep = val - vars[var].val_f; 
           bool isvar = (nowdeep == 0);
           if (var >= vars_last) sys.error("nonexisted var");
           if (calc_stat[var] == stat_calc) { 
           } else if (calc_stat[var] == stat_uncalc) {
               if (isvar) sys.error(vars[var].name + " recursion");
           } else {
               if (vars[var].var == null) sys.error(vars[var].name + " var: non is non");
               uncalc_var(var); vals_calc[vars[var].val_f].set(vars[var].var.calc()); calc_stat[var] = stat_calc;
           }
           if (!vals_calc[val].exist()) sys.error(vars[var].name + " val: non is non");
           return (vals_calc[val]);
        }
        public void set_var(int var, num n)
        {
           if (calc_stat[var] != stat_calc) {
               if (calc_stat[var] != stat_uncalc) uncalc_var(var);
               calc_stat[var] = stat_calc;
           }
           vals_calc[vars[var].val_f].set(n);
        }
        public void set_val(int val, num n)
        {
           vals_calc[val].set(n);
        }
        public void set_var_onval(int val, num n)
        {
            set_var(vals_var[val],n);
        }
        public int val_to_var(int v) {return vals_var[v];}
        public int val_to_deep(int v) {return v - vars[vals_var[v]].val_f;}
        public string get_name_var(int var)
        {
            return vars[var].name;
        }
        public string get_name_val(int val)
        {
            return new String('\'', val - vars[vals_var[val]].val_f) + vars[vals_var[val]].name;
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
        private BigInteger up, down;
        static BigInteger prec_base = 10;
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
        static public num add(num a0, num a1)
        {
            num r = new num(a1); r.add(a0,1);
            return r;
        }
        static public num sub(num s0, num s1)
        {
            num r = new num(s1); r.add(s0,1);
            return r;
        }
        static public num mul(num m0, num m1)
        {
            return new num(m0.sign * m1.sign,m0.up * m1.up,m0.down * m1.down);
        }
        static public num div(num d0, num d1)
        {
            return new num(d0.sign * d1.sign,d0.up * d1.down,d0.down * d1.up);
        }
        static public num max(num m0, num m1)
        {
            if (m0.great(m1)) return new num(m1); else return new num(m0);
        }
        static public num min(num m0, num m1)
        {
            if (m0.great(m1)) return new num(m0); else return new num(m1);
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
            up = n.up;
            down = n.down;
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
        private num(int _s, BigInteger _u, BigInteger _d)
        {
            sign = _s; up = _u; down = _d; exs = true;
        }
        public num(BigInteger _u, BigInteger _d)
        {
            set(_u,_d);
        }

        public void set_up(BigInteger _u)
        {
            up = (_u > 0 ? _u: 0-_u); exs = true;
        }
        public void set_down(BigInteger _d)
        {
            down = (_d > 0 ? _d: 0-_d);
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

        public bool isint()
        {
            return (down == 1);
        }
        public bool isint(int n)
        {
            return (down == 1) && (sign * n >= 0) && ((int)up == BigInteger.Abs(n));
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
        public void exp(ids r, num ex)
        {
            int u,d;
            if (ex.up == 0) { set1(); return; }
            if ((ex.up > r.exp_max) || (ex.down > r.exp_max)) {
                num e1 = new num(ex);
                e1.prec(r.exp_prec);
                if (e1.down > r.exp_max) { set1(); return; }
                if (e1.up > r.exp_max) r.sys.error("exp too large");
                u = (int)e1.get_sup(); d = (int)e1.get_down();
            } else {u = (int)ex.get_sup(); d = (int)ex.get_down();}
            exp(u);
            exs = root(d);
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
            return (get_sign() < 0 ? "_" : "") + get_up().ToString().Trim() + (get_down() > 1 ? ("_" + get_down().ToString().Trim()) : "");
        }
        public int CompareTo(object obj) {
            if (obj == null) return 1;
            num k = obj as num;
            if (sign * k.sign < 0) return sign;
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
            if (i.isint(0)) return k.print(plus,minus,non_one);
            return "[" + k.print("","-","") + "," + i.print("","-","") + "]" + non_one;
        }
    }

    class one: IComparable
    {
        ids root;
        public SortedDictionary<int,func> exps;
        public one(ids h)
        {
            root = h;
            exps = new SortedDictionary<int,func>();
        }
        public one(one o)
        {
            root = o.root;
            exps = new SortedDictionary<int,func>();
            set(o);
        }
        public void set(one o)
        {
            exps.Clear();
            foreach(KeyValuePair<int,func> m in o.exps)
                exps.Add(m.Key,new func(m.Value));
        }

        public int CompareTo(object obj) {
            if (obj == null) return 1;
            one o = obj as one;
            SortedDictionary<int,func>.Enumerator e0 = exps.GetEnumerator();
            SortedDictionary<int,func>.Enumerator  e1 = o.exps.GetEnumerator();
            bool n0,n1;
            int r;
            while (true) {
                n0 = e0.MoveNext(); n1 = e1.MoveNext();
                if (n0 ^ n1) return (n0 ? e0.Current.Value.CompareTo(root.fzero) : root.fzero.CompareTo(e1.Current.Value));
                else {
                    if (! n0) return 0;
                    if (e0.Current.Key == e1.Current.Key) {
                        if ((r = e0.Current.Value.CompareTo(e1.Current.Value)) != 0) return r;
                    } else {
                        if (e0.Current.Key < e1.Current.Key) return e0.Current.Value.CompareTo(root.fzero);
                        else return root.fzero.CompareTo(e1.Current.Value); 
                    }
                }
            }
        }
/*
        public bool test_mul(one a)
        {
            if (zero.root != a.zero.root) return false;


            list.link(a.list); foreach (int i in list) { 
                if (!exps[i].test_add(a.exps[i],1)) {list.link(); return false;}
            }
            list.link(); return true;
        }
        public bool mul_t(one a)
        {
            bool rt = true;
            if (zero.root != a.zero.root) return false;
            list.link(a.list);
            foreach (int i in list) {
                if (!exps[i].add(a.exps[i],1)) rt = false;
                list.set(i,! exps[i].iszero());
            }
            list.link(); return rt;
        }
 */ 
        public void mul(one o)
        {
            foreach(KeyValuePair<int,func> m in o.exps) {
                if (exps.ContainsKey(m.Key)) {
                    exps[m.Key].add(m.Value,1);
                    if (exps[m.Key].data[1].isconst(0)) exps.Remove(m.Key);
                }
                else exps.Add(m.Key, new func(m.Value));
            }
        }

        public void div()
        {
            foreach(KeyValuePair<int,func> m in exps) m.Value.data[1].neg();
        }
        public void exp(int e)
        {
            exp(new num(e));
        }
        public void exp(num e)
        {
            foreach(KeyValuePair<int,func> f in exps) {
                foreach(KeyValuePair<one,num> m in f.Value.data[1].data) m.Value.mul(e);
            }
        }
        public void exp(func e)
        {
            foreach(KeyValuePair<int,func> m in exps) m.Value.mul(e);
        }

        public void extract(one from)
        {
            foreach(KeyValuePair<int,func> m in exps) 
                if (from.exps.ContainsKey(m.Key)) m.Value.extract(from.exps[m.Key]);
                else m.Value.set0();
        }
        public void simple()
        {
            SortedDictionary<int,func> r = new SortedDictionary<int,func>();
            foreach(KeyValuePair<int,func> m in exps) {
                m.Value.simple();
                if (! m.Value.data[1].isconst(0)) r.Add(m.Key, m.Value);
            }
            exps = r;
        }
        public void go_deeper(int deep) {
            SortedDictionary<int,func> r = new SortedDictionary<int,func>();
            foreach(KeyValuePair<int,func> m in exps) {
                if (root.val_to_var(m.Key) != root.val_to_var(m.Key + deep)) root.sys.error(" too deep ");
                r.Add(m.Key + deep,m.Value);
            }
            exps = r;
        }
        public num calc() {
            num rt = new num(1), t = new num(0);
            foreach(KeyValuePair<int,func> f in exps) {
                t.set(root.get_val(f.Key));
                t.exp(root,f.Value.calc());
                rt.mul(t);
            }
            return rt;
        }
    }
    class many: power<many>, ipower, IComparable
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
        public many(ids h, num n)
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
        public int CompareTo(object obj) {
            if (obj == null) return (data.Count > 0 ? data.ElementAt(0).Value.get_sign(): 0);
            many m = obj as many;
            SortedDictionary<one,num>.Enumerator o0 = data.GetEnumerator();
            SortedDictionary<one,num>.Enumerator o1 = m.data.GetEnumerator();
            bool n0,n1;
            int r;
            while (true) {
                n0 = o0.MoveNext(); n1 = o1.MoveNext();
                if (n0 ^ n1) return (n0 ? o0.Current.Value.CompareTo(root.ozero) : root.ozero.CompareTo(o1.Current.Value));
                else {
                    if (! n0) return 0;
                    if ((r = o0.Current.Key.CompareTo(o1.Current.Key)) == 0) {
                        if ((r = o0.Current.Value.CompareTo(o1.Current.Value)) != 0) return r;
                    } else {
                        if (r > 0) return o0.Current.Value.CompareTo(root.nzero);
                        else return root.nzero.CompareTo(o1.Current.Value);
                    }
                }
            }
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
        public void mul(num m)
        {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            one o; num n;
            while (data.Count > 0) {
                o = data.ElementAt(0).Key;
                n = data[o];
                data.Remove(o);
                n.mul(m);
                r.Add(o,n);
            }
            data = r;
        }
        public override void mul(many m)
        {
            many r = many.mul(this,m);
            data = r.data;
        }
        public static many mul(many _m0, many _m1)
        {
            if (_m0.root != _m1.root) return null;
            many r = new many(_m0.root,0);
            one o = new one(_m0.root);
            foreach (KeyValuePair<one,num> m0 in _m0.data) {
                foreach (KeyValuePair<one,num> m1 in _m1.data) {
                    o.set(m0.Key);
                    o.mul(m1.Key);
                    if (r.data.ContainsKey(o)) r.data[o].addmul(m0.Value,m1.Value);
                    else r.data.Add(new one(o), num.mul(m0.Value,m1.Value));
                }
            }
            foreach (KeyValuePair<one,num> d in r.data) d.Value.simple();
            return r;
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
        public int type_exp() 
        {
            if (isone()) {
                if (data.ElementAt(0).Value.isint(1)) return 0; else return 1;
            } else return 2;
        }
        public int type_pow() 
        {
            if (isone() && (data.ElementAt(0).Key.exps.Count == 0)) {
                if (data.ElementAt(0).Value.isint()) return 0; else return 1;
            } else return 2;
        }
        public void exp(func e)
        {
            int te = type_exp(),tp = e.type_pow();
            if (te + tp > 2) root.sys.error("exp: cant many to many");
            if (te > 1) exp((int)(e.data[1].data.ElementAt(0).Value.get_up()));
            else {
                if (tp < 2) {
                    data.ElementAt(0).Key.exp(e.data[1].data.ElementAt(0).Value);
                    data.ElementAt(0).Value.exp(root,e.data[1].data.ElementAt(0).Value);
                } else {
                    data.ElementAt(0).Key.exp(e);
                }
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
        public int sign() {
            bool p = false, m = false;
            foreach (KeyValuePair<one,num> o in data) {
                if (o.Value.get_sign() > 0) {
                    p = true; if (m) return 0;
                }
                if (o.Value.get_sign() < 0) {
                    m = true; if (p) return 0;
                }
            }
            if (p) return 1;
            if (m) return -1;
            return 0;
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

        public void add_toexp(int val, func _e) 
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

        public func find_minexp(int val) //_val^(-x) -> /_val^(x)
        {
            func _min = null, t; int s;
            foreach (KeyValuePair<one,num> o in data)
                if (o.Key.exps.ContainsKey(val)) {
                    if (_min == null) _min = new func(o.Key.exps[val]); else {
                        t = new func(o.Key.exps[val]);
                        t.add(_min,-1); s = 0;
                        foreach (KeyValuePair<one,num> o1 in t.data[1].data) {
                            if (o1.Value.get_sign() > 0) {
                                if (s < 0) return null; else s = +1;
                            }
                            if (o1.Value.get_sign() < 0) {
                                if (s > 0) return null; else s = -1;
                            }
                        }
                        if (s < 0) _min.set(o.Key.exps[val]);
                    }
                }
            return _min;
        }

        public bool isone() 
        {
            return data.Count == 1;
        }
        public bool isconst()
        {
             return isone() && (data.ElementAt(0).Key.exps.Count == 0);
        }
        public bool isconst(int n)
        {
             return isconst() && data.ElementAt(0).Value.isint(n);
        }
        public void extract(many m)
        {
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();

            foreach (KeyValuePair<one,num> d in data)
            {
                if (m.data.ContainsKey(d.Key)) {
                    if (d.Value.get_sign() == m.data[d.Key].get_sign())
                    {
                        r.Add(new one(d.Key),num.mul(d.Value,m.data[d.Key]));
                    }
                }
            }
            data = r;
        }

        public void simple()
        {
            if (data.Count < 2) return;
            SortedDictionary<one,num> r = new SortedDictionary<one,num>();
            foreach (KeyValuePair<one,num> d in data)
            {
                d.Value.simple();
                if (! d.Value.isint(0)) r.Add(d.Key,d.Value);
            }
            data = r;
        }
    }
    class exps 
    {
        public num max, min;
        ids root;
        int deep;
        public int type; 
        //0 - 1*one, 1 - n*one, 2 - many
        //0,1 - sign, 2 - no sign
        public SortedDictionary<func,many> data;
        func e1;
        public exps(many m, int _deep){
            root = m.root; min = new num(0); max = new num(0);
            data = new SortedDictionary<func,many>();
            data.Add(new func(root,-1,0), new many(root,1));
            deep = _deep;
            e1 = new func(root,-1,1);
            data.Add(e1, new many(m));
            data[e1].go_deeper(deep);
            type = data[e1].type_exp();
        }
        void add(func e) {
            if (! data.ContainsKey(e)) data.Add(e,new many(root,0));
        }

        public void add(many m, int val, int updown){
            num t; 
            min.set0(); max.set0(); 
            func te;
            if (type > 1) {
                foreach (KeyValuePair<one,num> on in m.data) {
                    te = on.Key.exps[val];
                    if (te.type_pow() + type < 3) {
                        t = te.data[1].data.ElementAt(0).Value;
                        min.min(t); max.max(t);
                    }
                }
                min.neg();
                add(new func(root,-1,0,min)); add(new func(root,-1,0,max));
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
                te = on.Key.exps[val]; 
                if (te.type_pow() + type < 3) {
                    if (type < 2) add(new func(te)); else {
                        if (updown == +1) { //pow + (-min)
                            t.set(te.data[1].data.ElementAt(0).Value); t.add(min,+1);
                            add(new func(root,-1,0,t));
                        }
                        if (updown == -1) {//max - pow
                            t.set(max); t.add(te.data[1].data.ElementAt(0).Value,-1);
                            add(new func(root,-1,0,t));
                        }
                    }
                }
            }
        }
        public void calc(){
            func dl = new func(root,-1,0);
            foreach (KeyValuePair<func,many> d in data) {
                if (d.Value.isconst(0)) {
                    if (type > 0) {
                        foreach (KeyValuePair<func,many> d0 in data) {
                            if (d.Key.CompareTo(d0.Key) >= 0) break;
                            dl.set(d.Key); dl.add(d0.Key,-1);
                            if (data.ContainsKey(dl)) {
                                d.Value.set(d0.Value);
                                d.Value.mul(data[dl]);
                                break;
                            }
                        }
                    }
                    if (d.Value.isconst(0)) {
                        d.Value.set(data[e1]);
                        d.Value.exp(d.Key);
                    }
                }
            }
        }
    }

    class func: power<func>, ipower, IComparable
    {
        public ids root;
        public SortedDictionary<int,many> data;
        public int id;
        public int tfunc; //0: 0//1; 1:
        public num pfunc;
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
            set(t, new num(0));
        }
        public func(ids h, int var, int t, num n)
        {
            pfunc = new num(-1);
            root = h; id = var;
            data = new SortedDictionary<int,many>();
            set(t, n);
        }
        public func(func f)
        {
            data = new SortedDictionary<int,many>();
            set(f);
        }
        public int type_pow() {
            return (data[-1].isconst(1) ? data[1].type_pow() : 2);
        }
        public void set(int t, num n) {
            tfunc = t;
            switch(t) {
                case 0:
                    data.Add(1,new many(root,n));
                    data.Add(-1,new many(root,1));
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
        public override void set(func f)
        {
            tfunc = f.tfunc; pfunc = new num(f.pfunc);
            root = f.root; id = f.id;
            data.Clear();
            foreach (KeyValuePair<int,many> m in f.data) data.Add(m.Key,new many(m.Value));
        }
        public override void copy(ref func f)
        {
            f.set(this);
        }
        public override void set0()
        {
            foreach (KeyValuePair<int,many> m in data) if (m.Key < 0) m.Value.set1(); else m.Value.set0();
        }
        public override void set1()
        {
            foreach (KeyValuePair<int,many> m in data) m.Value.set1();
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
        }
        public void extract(func f)
        {
            if ((tfunc != f.tfunc) || (root != f.root)) root.sys.error("!");
            switch (tfunc) {
                case 0:
                    if (data[-1].CompareTo(f.data[-1]) == 0) {
                        data[1].extract(f.data[1]);
                    } else {
                        many t = many.mul(f.data[1],data[-1]);
                        data[1].mul(f.data[-1]);
                        data[-1].mul(f.data[-1]);
                        data[1].extract(t);
                    }
                    simple();
                    break;
            }
        }
        public num get_num() {
            if (type_pow() > 1) return null;
            return num.div(data[1].data.ElementAt(0).Value,data[-1].data.ElementAt(0).Value);
        }
        public void add(func f, int s)
        {
            if ((tfunc != f.tfunc) || (root != f.root)) root.sys.error("!");
            switch (tfunc) {
                case 0:
                    if (data[-1].CompareTo(f.data[-1]) == 0) data[1].add(f.data[1],s); else {
                        many t = new many(f.data[1]); t.mul(data[-1]);
                        data[1].mul(f.data[-1]); data[1].add(t,s);
                        data[-1].mul(f.data[-1]);
                    }
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }
        public int CompareTo(object obj) {
            if (obj == null) return data[1].CompareTo(null);
            func f = obj as func;
            int r = data[1].CompareTo(f.data[1]);
            if (r != 0) return r;
            if (data.ContainsKey(-1) && f.data.ContainsKey(-1)) return -f.data[-1].CompareTo(data[-1]);
            return 0;
        }

        public void simple()
        {
            if (tfunc != 0) return;
            many _up = data[1], _dw = data[-1];
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
            foreach(KeyValuePair<int,func> u in f_up.Key.exps) if (u.Value.data[1].sign() * u.Value.data[-1].sign() >= 0) u.Value.set0();
            f_up.Key.simple();

            f_down.Value.set1();
            foreach(KeyValuePair<int,func> d in f_down.Key.exps) if (d.Value.data[1].sign() * d.Value.data[-1].sign() >= 0) d.Value.set0();
            f_down.Key.simple();

            f_up.Key.div();
            data[1].mul(f_up);
            data[-1].mul(f_up);
            f_down.Key.div();
            data[1].mul(f_down);
            data[-1].mul(f_down);
            data[1].simple();
            data[-1].simple();
            if (data[-1].isconst() && (! data[-1].isconst(1))) {
                data[-1].data.ElementAt(0).Value.div();
                data[1].mul(data[-1].data.ElementAt(0).Value);
                data[-1].data.ElementAt(0).Value.set1();
            }
        }

        public void go_deeper(int deep) {
            foreach (KeyValuePair<int,many> m in data) m.Value.go_deeper(deep);
        }

        public void expand(int val)
        {
            int deep = root.val_to_deep(val);
            func f = root.vars[root.val_to_var(val)].var;
            if (f == null) return;
            exps exu = new exps(f.data[1],deep);
            exps exd = new exps(f.data[-1],deep);
            expand(+1,val,exu,exd);
            num minup = new num(exu.min), maxdw = new num(exd.max);
            expand(-1,val,exu,exd);
            data[+1].mul(exu.data[new func(root,-1,0,exu.min)]);
            data[+1].mul(exd.data[new func(root,-1,0,exd.max)]);
            data[-1].mul(exu.data[new func(root,-1,0,minup)]);
            data[-1].mul(exd.data[new func(root,-1,0,maxdw)]);
            simple();
        }

        public void expand(int up, int val, exps exu, exps exd)
        {
            SortedDictionary<func,many> ml = new SortedDictionary<func,many>();
            many res = new many(root);
            exu.add(data[up],val,+1); exu.calc();
            exd.add(data[up],val,-1); exd.calc();
            int type = (exu.type > exd.type ? exu.type : exd.type);
            func eu, ed, e;
            foreach (KeyValuePair<one,num> o in data[up].data) 
            {
//[+1]/[-1]
//[-up] *= [+1]^min*[-1]^max 
//[+1]^(up + (-min))*[-1]^(max - up)
                if (o.Key.exps.ContainsKey(val) && (o.Key.exps[val].type_pow() + type < 3)) {
                    e = o.Key.exps[val];
                    eu = (exu.type > 1 ? new func(root,-1,0,num.add(exu.min,e.get_num())) : e);
                    ed = (exu.type > 1 ? new func(root,-1,0,num.sub(exd.max,e.get_num())) : e);
                    if (exu.data.ContainsKey(eu) && exd.data.ContainsKey(ed)) {
                        if (! ml.ContainsKey(e)) {
                            ml.Add(new func(e),new many(exu.data[eu]));
                            ml[e].mul(exd.data[ed]);
                        }
                        res.expand(ml[e],o,val);
                    } else res.add(o,1);
                } else res.add(o,1);
            }
            data[up] = res;
        }

        public bool revert(int val, int up) //_val^(-x) -> /_val^(x)
        {
            func min = data[up].find_minexp(val);
            if ((min == null) || (min.data[1].isconst(0))) return false;
            min.data[1].neg();
            data[up].add_toexp(val,min);
            data[-up].add_toexp(val,min);
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
/*
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
 */
        public num calc()
        {
            num rt = new num(0);
                if (tfunc < 2)
                {
                    rt.set(_calc());
                }
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
                rt.simple();

            return rt;
        }
        num _calc()
        {
            num tr = new num(0), t0 = new num(0);
            foreach (KeyValuePair<int,many> m in data)
            {
                foreach (KeyValuePair<one,num> o in m.Value.data) 
                {
                    t0.add(num.mul(o.Key.calc(),o.Value));
                }
                t0.exp(m.Key);
                tr.add(t0);
                t0.set0();
            }
            return tr;
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
            to_val = new int[root.size()];
            for (int i = 0; i < root.size(); i++) to_val[i] = -1;
            eneg = new ushort[mexp * mexp]; eadd = new ushort[mexp * mexp]; eflg_a = new bool[mexp * mexp];
            for (uint i = 0; i < mexp * mexp; i++) { eneg[i] = 0xFFFF; eadd[i] = 0xFFFF; eflg_a[i] = true; }
            lexp = 2; exps[0] = new num(0); exps[1] = new num(1);
            lval = 0;
        }
        public ushort exp(num e)
        {
            ushort i;
            for (i = 0; i < lexp; i++) if (e.equ(exps[i])) return i;
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
        public KeyValuePair<mao_key,num> fr_one(KeyValuePair<one,num> o)
        {
            int i0,v0;
            KeyValuePair<mao_key, num> ret = new KeyValuePair<mao_key,num>(new mao_key(dict), new num(o.Value));
            for (i0 = 0; i0 < dict.nvals; i0++) ret.Key.key[i0] = 0;
            foreach (KeyValuePair<int,func> f in o.Key.exps)
            {
                if (f.Value.type_pow() > 1) dict.root.sys.error("cant fast on complex exp");
                v0 = dict.val(f.Key);
                ret.Key.key[v0] = dict.exp(f.Value.get_num());
            }
            return ret;
        }
        public one to_one(mao_key fr, func f)
        {
            int i;
            one ret = new one(f.root);
            for (i = 0; i < dict.nvals; i++)
                if (fr.key[i] != 0) ret.exps.Add(dict.vals[i],new func(f.root,-1,0,dict.exps[fr.key[i]]));
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
            foreach (KeyValuePair<one,num> o in f.data[1].data) add(0, fr_one(o));
            foreach (KeyValuePair<one,num> o in f.data[-1].data) add(1, fr_one(o));
        }
        public func to_func(ids h, int var)
        {
            int i=0, cn = data[0].Count + data[1].Count;
            func ret = new func(h,var,0); ret.data[1].data.Clear(); ret.data[-1].data.Clear();
            foreach (KeyValuePair<mao_key, num> d in data[0]) {ret.data[1].data.Add(to_one(d.Key, ret),new num(d.Value)); dict.root.sys.progr(i++,cn);}
            foreach (KeyValuePair<mao_key, num> d in data[1]) {ret.data[-1].data.Add(to_one(d.Key, ret), new num(d.Value)); dict.root.sys.progr(i++,cn);}
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
                    nexp.mul(new num(new BigInteger(1),exp.get_down()));
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
//        parse head;
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
        public ids root;
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
            int fval = -1;
            num nval = null, n1 = new num(1);
            func ex = new func(root,-1,0, n1);
            KeyValuePair<one,num> on = new KeyValuePair<one,num>(new one(root),new num(1));
            KeyValuePair<one,num> ret = new KeyValuePair<one,num>(new one(root),new num(now == '-' ? -1: +1));
            if ((now == '-') || (now == '+')) next();
            bool l = true;
            Action eset = () => {
                if (nval == null) {
                    if (fval > -1) {
                        if (ret.Key.exps.ContainsKey(fval)) ret.Key.exps[fval].add(ex,1); else ret.Key.exps.Add(fval,ex);
                        fval = -1;
                        ex = new func(root,-1,0, n1);
                    }
                } else {
                    if ((fval >= 0) || (ex.type_pow() > 1)) sys.error("parse");
                    nval.exp(root,ex.get_num());
                    ret.Value.mul(nval); nval = null;
                    ex = new func(root,-1,0, n1);
                }
            };
            char[] pc = {isabc,isnum,'{','('};
            Action[] pf = {
                      () => { 
                          on.Key.exps.Clear(); 
                          on.Key.exps.Add(root.find_val(get(isname,"")),new func(root,-1,0,n1));
                          ex.data[1].mul(on);
                      },
                      () => ex.data[1].data.ElementAt(0).Value.mul(new num(get(isnum,""))),
                      () => ex.data[1].data.ElementAt(0).Value.mul(calc()),
                      () => ex.mul(fpars(-1,true)),
                      () => sys.error("nonum in calc")
                          };
            Action[] oof = {
                () => {l = false; },
                () => {l = false; },
                () => {eset(); next();},
                () => {eset(); next(); ex.data[1].neg();},
                () => {
                    next();
                    branchnow(pc,pf); 
                    if (!((nval == null) ^ (fval < 0))) sys.error("parse");
                    eset();
                    repeat = true;
                },
                () => sys.error("nonum in calc")
                };
            char[] nc = {isabc,isnum,'{','('};
            Action[] nf = {
                () => {string _n = get(isname,"");
                    if (isequnow('(')) {
                        fval = root.add_var("",0); root.vars[fval].var = fpars(fval,true,_n);
                    } else fval = root.find_val(_n);
                      },
                () => {nval = new num(get(isnum,""));},
                () => {nval = calc();},
                () => {fval = root.add_var("",0); root.vars[fval].var = fpars(fval,true);},
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
            if ((nval != null) || (fval >= 0)) eset();
            ret.Key.simple(); ret.Value.simple();
            return ret;
        }
        public many mpars()
        {
            many m = new many(root);
            KeyValuePair<one,num> on;
            int d = deep.Count;
            if (isequnow(isopen)) next();
            while ((! isequnow(isclose)) && (!isequnow(isend))) {
                on = opars();
                if (m.data.ContainsKey(on.Key)) m.data[on.Key].add(on.Value); else m.data.Add(on.Key,on.Value);
            }
            if (d < deep.Count) next();
            return m;
        }
        public func fpars(int v, bool flag)
        {
            string _fn = "";
            if (flag) {
                if (isequnow(isabc)) {
                    _fn = get(isname,"");
                }
            }
            return fpars(v,flag,_fn);
        }
        public func fpars(int v, bool flag, string _fn)
        {
            int t = 0;
            int d = deep.Count;
            if (flag) {
                if (! root.fnames.ContainsKey(_fn)) sys.error("parse: func");
                t = root.fnames[_fn];
                if (! isequnow('(')) sys.error("parse: func");
                next();
            }
            func f = new func(root,v,t);
            bool isdown = false;
            if ((t == 0) && isequnow('(')) {
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
            switch (t) {
                case 0:
                    f.data[1] = mpars();
                    if (isdown) {
                        next(); f.data[-1] = mpars();
                    }
                    break;
                case 1:
                    f.data[1] = mpars();
                    break;
            }
            if (d < deep.Count) next();
            return f;
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
                      () => ln[lp-1].exp(root,ln[lp]),
                      () => sys.error("nonum in calc")
                     };
            char[] nc = {isopen,isnum};
            Action[] nf = {
                              () => ln[lp+1].set(calc()),
                              () => ln[lp+1].set(get(isnum,"")),
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
                lo[++lp] = now;
                while((lp > 0) && (m_c_prior[lo[lp-1]] >= m_c_prior[lo[lp]])) {
                    branch(lo[lp-1],"+-*/^",fn); lp--;
                }
                branchnow(oc,of);
            }
            ln[0].simple();
            return ln[0];
        }
        public void Dispose()
        {
            sys.Dispose();
        }

        public string sign(num v)
        {
            if (v.get_sign() < 0) return "-";
            if (v.get_sign() > 0) return "+";
            return "";
        }
        public string print(num v, bool s)
        {
            string ret = "";
            if (s) ret += sign(v);
            ret += v.get_up().ToString().Trim();
            if (! v.isint()) ret += "/" + v.get_down().ToString().Trim();
            return ret;
        }
        public string print(one o, num n)
        {
            string ret = "", name;
            bool first = true, f1 = false;;
            foreach(KeyValuePair<int,func> m in o.exps) {
                if (first) {
                    if (n.isint() && (n.get_up() == 1)) {
                        if  ((m.Value.type_pow() > 1) || (m.Value.get_num().get_sign() > 0)) {
                            ret += sign(n);
                            f1 = true;
                        } else ret += print(n,true);
                    } else ret += print(n,true);
                }
                name = root.get_name_val(m.Key); if (name.Length == 0) name = print(root.vars[root.val_to_var(m.Key)].var, true);
                switch (m.Value.type_pow()) {
                    case 0: //int
                        ret += (m.Value.get_num().get_sign() < 0 ? "/" : (f1 ? "" : "*")) + name + (m.Value.get_num().get_up() > 1 ? "^" + m.Value.get_num().get_up().ToString().Trim() : "");
                        break;
                    case 1: //num
                        ret += (m.Value.get_num().get_sign() < 0 ? "/" : (f1 ? "" : "*")) + name + "^{" + print(m.Value.get_num(),false) + "}";
                        break;
                    case 2: 
                        ret += (f1 ? "" : "*") + name + "^" + print(m.Value,true);
                        break;
                }
                first = false; f1 = false;
            }
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
        public string print(func f, bool pair)
        {
            string ret = "";
            if (f == null) return ret;
            switch (f.tfunc) {
                case 0:
                    if (f.data[-1].isconst(1)) {
                        ret += print(f.data[1]);
                    } else {
                        ret += "(" + print(f.data[1]) + ")/(" + print(f.data[-1]) + ")";
                    }
                    if (pair) ret = "(" + ret + ")";
                    break;
                case 1:
                    ret += "(" + print(f.data[1]) + ")";
                    break;
            }
            return root.funcs_name[f.tfunc] + ret;
        }

        public BigInteger get_parm()
        {
            num tmp;
            if (isequnow(isnum) || isequnow(isopen)) tmp = calc(); else  tmp =  root.get_val(root.find_val(get(isname,"")));
            return tmp.toint();
        }
    }



    static class Program
    {
        public static shard0 m0;
        public static System.Drawing.Bitmap bm1;
        static parse par;


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
            par.lnext(); par.root = new ids((int)par.get_parm(),(int)par.get_parm(),par.get_parm(),par.get_parm(), par.sys);
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
        static void doit() {
            int var0,var1,var2,val0;
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
                                var0 = par.root.find_var(name);
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
                                par.root.vars[var0].var = (par.isequnow(parse.isend) ? null : par.fpars(var0,flag));
                                par.sys.wline(0,par.print(par.root.vars[var0].var,false));
                            break;
                        }
                    }
                }
            }
            par.sys.wline(0,"finished, vars free = " + (par.root.vars.Count() - par.root.vars_last-1).ToString());
            par.sys.close();
        }
    }
}
/*
                    switch (par.now) 
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
