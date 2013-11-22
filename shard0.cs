//for Them, who my death and my life
using System;
using System.Threading;
using System.Drawing;
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
        public int vars, deep,size, last;
        public num prec;
        string[] names;
        public many[] values;
        num[] calc;
        public bool[] calc_flg;
        public fileio sys;
        public ids(BigInteger _v, BigInteger _d, BigInteger p, fileio f)
        {
            if ((_v < 11) || (_v > 6666) || (_d < 1) || (_d > 6) || (p < 11)) sys.error("wrong init");
            vars = (int)_v; deep = (int)_d; size = vars*deep;
            prec = new num(1,p,p); sys = f; last = 0;
            names = new string[vars];
            values = new many[vars];
            calc = new num[size];
            calc_flg = new bool[vars];
        }
        public int set_empty(string nam)
        {
            if (last >= vars) sys.error("too many vars");
            names[last] = nam.Replace("'","");
            values[last] = null;
            for (int i = 0; i < deep; i++) calc[last*deep + i] = new num();
            last++; return last - 1;
        }
        public void uncalc()
        {
            for (int i = 0; i < last; i++) {
                for (int ii = deep - 1; ii > 0; ii--) calc[i*deep + ii].set(calc[i*deep + ii-1]);
                calc[i*deep].unset();
                calc_flg[i] = false;
            }
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

        public num get_val(int val)
        {
           int val0, id; 
           id = val / deep; val0 = id * deep;
           if (id >= last) sys.error("nonexisted var");
           if (!calc[val0].exist())
           {
               if (values[id] == null) sys.error(names[id] + " non is non");
               calc[val0] = values[id].calc();
           }
           if (!calc[val].exist()) sys.error(names[id] + " non is non");
           return (calc[val]);
        }
        public num get_var(int var)
        {
            return get_val(var*deep);
        }
        public void set_var(int var, num n)
        {
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
        public int var_to_val(int n) {return n*deep;}
    }
    class num :IComparable
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
        public void set(num n)
        {
            sign = n.sign;
            up = BigInteger.Abs(n.up);
            down = BigInteger.Abs(n.down);
            exs = true;
        }
        public void set(num n,int s)
        {
            sign = n.sign * s;
            up = BigInteger.Abs(n.up);
            down = BigInteger.Abs(n.down);
            exs = true;
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
        public void zero()
        {
            init(0,1);
        }
        public void one()
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
        public bool cmp(num a)
        {
            return ((sign == a.sign) && (up == a.up) && (down == a.down));
        }

        public bool isone()
        {
            return (down == 1) && (up == 1);
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
        public void div()
        {
            BigInteger t = up;
            up = down; down = t;
        }
        public void exp(int ex)
        {
            int _e;
            _e = ((ex < 0) ? -ex : ex);
            if ((_e & 1) == 0) sign = 1;
            BigInteger u0 = up, d0 = down;
            up = 1; down = 1; 
            for (int i = _e; i > 0; i >>= 1) { 
                if ((i&1) != 0) {up *= u0; down *= d0;} 
                u0 *= u0;  d0 *= d0;
            }
            if (ex < 0) div();
        }
        public void exp(num ex)
        {
            int e = (int)ex.up;
            if (e < 10000) exp(e*ex.sign);
        }
        public void mul(int a)
        {
            if (a < 0)
            {
                sign = -sign;
                up *= -a;
            }
            else if (a > 0) up *= a;
            else zero();
        }
        public void pow2()
        {
            sign *= sign;
            up *= up;
            down *= down;
        }
        public void mul(num a)
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
            if ((sign < 0) && (s % 2 == 0)) return false;
            BigInteger u = _sq(up,s), d = _sq(down,s);
            if ((BigInteger.Pow(u, s) == up) && (BigInteger.Pow(d, s) == down))
            {
                up = u; down = d; return true;
            }
            return false;
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
        public int CompareTo(object obj) {
            if (obj == null) return 1;
            num k = obj as num;
            if ((sign == k.sign) && (up == k.up) && (down == k.down)) return 0;
            if (great(k)) return -1; else return 1;
        }
    }
    class complex
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
        public complex(complex _k)
        {
            k = new num(_k.k);
            i = new num(_k.i);
        }
        public void simple()
        {
            k.simple(); i.simple();
        }
        public void set(complex a)
        {
            k.set(a.k); i.set(a.i);
        }
        public bool cmp(complex a)
        {
            return k.cmp(a.k) && i.cmp(a.i);
        }
        public void neg()
        {
            k.neg(); i.neg();
        }
        public void add(complex a)
        {
            k.add(a.k); i.add(a.i);
        }
        public void div()
        {
            num x0, x1;
            x0 = new num(k); x0.pow2();
            x1 = new num(i); x1.pow2();
            x0.add(x1); x0.div();
            k.mul(x0); i.mul(x0);
        }
        public void mul(complex a)
        {
            num x,_k;
            _k = new num(k); _k.mul(a.k);
            x = new num(i); x.mul(a.i);
            x.neg(); _k.add(x);
            i.mul(a.k); k.mul(a.i);
            i.add(k); k.set(_k);
        }
        public void pow2()
        {
            //k^2-i^2 : 2*k*i
            num k2,i2;
            k2 = new num(k); k2.pow2();
            i2 = new num(i); i2.pow2();
            i2.neg(); k2.add(i2);
            i.mul(k); i.mul(2);
            k.set(k2);
        }
        public void exp(int ex)
        {
            int _e;
            _e = ((ex < 0) ? -ex : ex);
            complex t = new complex(this);
            k.set(1); i.set(0);
            for (int i0 = _e; i0 > 0; i0 >>= 1)
            {
                if ((i0 & 1) != 0) mul(t);
                t.pow2();
            }
            if (ex < 0) div();
        }
        public string print(string plus, string minus, string non_one)
        {
            if (i.iszero()) return k.print(plus,minus,non_one);
            return "[" + k.print("","-","") + "," + i.print("","-","") + "]" + non_one;
        }
    }
    class exp
    {
        public many head;
        public num non;
        public num[] vars;
        public exp(ref many h)
        {
            head = h;
            non = new num(0);
        }
        public exp(ref many h, int n)
        {
            head = h;
            non = new num(n);
        }
        public exp(many h, num n)
        {
            head = h;
            non = new num(n);
        }
        public exp(ref exp e)
        {
            head = e.head;
            non = new num(e.non);
            if (e.vars != null)
            {
                vars = new num[head.head.size];
                for (int i = 0; i < head.head.size; i++) vars[i] = new num(e.vars[i]);
            }
        }
        public void set(ref exp e)
        {
            non.set(e.non);
            if (e.vars != null)
            {
                vars = new num[head.head.size];
                for (int i = 0; i < head.head.size; i++) vars[i] = new num(e.vars[i]);
            } else vars = null;
        }
        void set_vars()
        {
            if (vars == null)
            {
                vars = new num[head.head.size];
                for (int i = 0; i < head.head.size; i++) vars[i] = new num(0);
            }
        }
        public void addvar(int val, num n) {
            set_vars();
            vars[val].add(n);
        }
        public void addvar(string s, num n) {
            addvar(head.head.find_val(s),n);
        }
        public void addvar(string s, int n) {
            addvar(s,new num(n));
        }
        public bool test_add(ref exp e, int sign)
        {
            num tmp = new num(non);
            tmp.add(e.non, sign);
            if ((e.non.get_sign() != 0) && (e.non.get_sign() == tmp.get_sign())) return false;
            if (e.vars != null)
            {
                set_vars();
                BigInteger a = 0; for (int i = 0; i < head.head.size; i++) 
                {
                    tmp.set(vars[i]);
                    tmp.add(e.vars[i],sign);
                    tmp.simple();
                    a += tmp.get_up();
                    if ((e.vars[i].get_sign() != 0) && (e.vars[i].get_sign() == tmp.get_sign())) return false;
                }
            }
            return true;
        }
        public bool add(ref exp e, int sign)
        {
            bool r = true; //non greater, false - become greater
            non.add(e.non, sign); non.simple();
            if ((e.non.get_sign() != 0) && (e.non.get_sign() == non.get_sign())) r = false;
            if (e.vars != null)
            {
                set_vars();
                BigInteger a = 0; for (int i = 0; i < head.head.size; i++) 
                {
                    vars[i].add(e.vars[i],sign);
                    vars[i].simple();
                    a += vars[i].get_up();
                    if ((e.vars[i].get_sign() != 0) && (e.vars[i].get_sign() == vars[i].get_sign())) r = false;
                }
                if (a == 0) vars = null;
            }
            return r;
        }
        public void mul(num n)
        {
            non.mul(n);
            if (vars != null)
            {
                for (int i = 0; i < head.head.size; i++) vars[i].mul(n);
            }
        }
        public void min(ref exp e)
        {
            if (!non.great(e.non)) non.set(e.non);
            if (e.vars == null) {
                if (vars != null) for (int i = 0; i < head.head.size; i++) if (vars[i].get_sign() > 0) vars[i].zero();
            } else {
                set_vars();
                for (int i = 0; i < head.head.size; i++) if (!vars[i].great(e.vars[i])) vars[i].set(e.vars[i]);
            }
        }
        public void neg()
        {
            non.neg();
            if (vars != null)
            {
                for (int i = 0; i < head.head.size; i++) vars[i].neg();
            }
        }
        public void zero()
        {
            non.zero();
            vars = null;
        }
        public bool iszero()
        {
            return (non.iszero() && (vars == null));
        }
        public bool cmp(ref exp e)
        {
            if (!non.cmp(e.non)) return false;
            if (vars == null) { if (e.vars == null) return true; else return false; }
            if (e.vars == null) return false;
            for (int i = 0; i < head.head.size; i++) if (!vars[i].cmp(e.vars[i])) return false;
            return true;
        }
        public bool ispos()
        {
            if (non.get_sign() > 0) return true;
            if (vars == null) {
                if (non.get_sign() < 0) return false; else return true;
            } else {
                for (int i = 0; i < head.head.size; i++) if (vars[i].get_sign() > 0) return true;
                return false;
            }
        }

        public void extract(ref exp e)
        {
           if (non.get_sign() == e.non.get_sign())
           {
                if (e.non.great(non)) non.set(e.non);
           }
           else non.zero();
            if (vars != null) {
                if (e.vars == null) vars = null; else {
                    for (int i = 0; i < head.head.size; i++) {
                        if (e.vars[i].get_sign() == vars[i].get_sign())
                        {
                            if (e.vars[i].great(vars[i])) vars[i].set(e.vars[i]);
                        }
                        else vars[i].zero();
                    }
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
                for (int i = 0; i < head.head.size; i++) if (vars[i].nonzero()) {
                    r += vars[i].print((first ? "" : "+"),"-","*") + head.head.get_name_onval(i);
                    first = false;
                }
                return r + (non.iszero() ? "" : non.print("+","-","")) + ")";
            }
        }

    }
    class one
    {
        static int max_list=6;
        public many head;
        public num mult;
        public exp[] exps;
        public int[] list;
        public bool islist = false;
        public one(many h, int m)
        {
            init_z(ref h);
            mult = new num(m);
        }
        public one(ref many h, int m)
        {
            init_z(ref h);
            mult = new num(m);
        }
        public one(ref many h, num m)
        {
            init_z(ref h);
            mult = new num(m);
        }
        public one(ref one o)
        {
            init(ref o, 1);
        }
        public one(ref one o, int s)
        {
            init(ref o, s);
        }
        void init(ref one o, int s)
        {
            init(ref o.head);
            mult = new num(o.mult); mult.set_sign(mult.get_sign() * s);
            for (int i0 = 0; i0 < head.head.size; i0++) exps[i0] = new exp(ref o.exps[i0]);
            if (o.islist)
            {
                int i;
                islist = true;
                for (i = 0; o.list[i] > -1; i++) list[i] = o.list[i];
                list[i] = -1;
            }
        }
        void init(ref many h)
        {
            head = h;
            exps = new exp[h.head.size];
            list = new int[max_list];
        }
        void init_z(ref many h)
        {
            init(ref h);
            for (int i0 = 0; i0 < head.head.size; i0++) exps[i0] = new exp(ref h);
        }

        public bool cmp(ref one a)
        {
            int i;
            if (head.head != a.head.head) return false;
            if (islist && a.islist) {
            for (i = 0; list[i] > -1; i++) if (!exps[list[i]].cmp(ref a.exps[list[i]])) return false;
            for (i = 0; a.list[i] > -1; i++) if (!exps[a.list[i]].cmp(ref a.exps[a.list[i]])) return false;
            } else for (i = 0; i < head.head.size; i++) if (!exps[i].cmp(ref a.exps[i])) return false;
            return true;
        }
        public bool test_mul(ref one a)
        {
            int i;
            bool rt = true;
            if (head.head != a.head.head) return false;
            for (i = 0; i < head.head.size; i++) { 
                if (!exps[i].test_add(ref a.exps[i],1)) return false;
            }
            return rt;
        }
        public bool mul_t(ref one a)
        {
            int i;
            bool rt = true;
            if (head.head != a.head.head) return false;
            mult.mul(a.mult);
            for (i = 0; i < head.head.size; i++) { 
                rt = rt && exps[i].add(ref a.exps[i],1);
            }
            islist = false;
            return rt;
        }
        public void mul(ref one a)
        {
            int i;
            if (head.head != a.head.head) return;
            mult.mul(a.mult);
            if (a.islist) 
            for (i = 0; a.list[i] > -1; i++) { 
                exps[a.list[i]].add(ref a.exps[a.list[i]],1);
            } else
            for (i = 0; i < head.head.size; i++) { 
                exps[i].add(ref a.exps[i],1);
            }
            islist = false;
        }
        public bool div()
        {
            int i0;
            if (mult.iszero()) return false; mult.div();
            if (islist) for (i0 = 0; list[i0] > -1; i0++) exps[list[i0]].neg();
            else for (i0 = 0; i0 < head.head.size; i0++) exps[i0].neg();
            return true;
        }
        public void extract(ref one from) //nonlist
        {
            int i0;
            mult.set((((mult.get_sign() < 0) && (from.mult.get_sign() < 0)) ? -1 : 1),BigInteger.GreatestCommonDivisor(mult.get_up(), from.mult.get_up()),BigInteger.GreatestCommonDivisor(mult.get_down(), from.mult.get_down()));
            for (i0 = 0; i0 < head.head.size; i0++) exps[i0].extract(ref from.exps[i0]);
            islist = false;
        }
        public void simple()
        {
            int i,n;
            mult.simple();
            if (!islist) {
                for (i = 0,n=0; (i < head.head.size) && (n < max_list-1); i++) if (!exps[i].iszero()) list[n++]=i;
                if (i == head.head.size) {list[n] = -1; islist = true;}
            }
        }
        public void exp_zero(int val) {
            islist = false;
            exps[val].zero();
        }
    }
    class many
    {
        public ids head;
        public List<one>[] data;
        public int id;
        public int tfunc;
        public num pfunc;
        public many(ref ids h, int var)
        {
            tfunc = -1; pfunc = new num(-1);
            head = h; id = var;
            data = new List<one>[2];
            data[0] = new List<one>();
            data[1] = new List<one>();
            data[1].Add(new one(this,1));
        }
        public many(ref many m)
        {
            tfunc = -1; pfunc = new num(0);
            head = m.head; id = m.id;
            data = new List<one>[2];
            data[0] = new List<one>();
            data[1] = new List<one>();
            foreach (one m0 in m.data[0]) {one tmp = m0; data[0].Add(new one(ref tmp));}
            foreach (one m1 in m.data[1]) {one tmp = m1; data[1].Add(new one(ref tmp));}
        }
        public many(ref many m, int ud)
        {
            tfunc = -1; pfunc = new num(0);
            head = m.head; id = m.id;
            data = new List<one>[2];
            data[0] = new List<one>();
            data[1] = new List<one>();
            foreach (one m0 in m.data[ud]) { one tmp = m0; data[0].Add(new one(ref tmp)); }
            data[1].Add(new one(this, 1));
        }
        public int simple(int num)
        {
            int i, j,sm=0;
            for (i = 0; i < data[num].Count; i++)
            {
                if (data[num][i].mult.iszero()) {data[num].RemoveAt(i); i--; }
                else {
                    for (j = i + 1; j < data[num].Count; j++)
                    {
                        one tmp = data[num][j];
                        if (data[num][i].cmp(ref tmp))
                        {
                            data[num][i].mult.add(data[num][j].mult);
                            data[num].RemoveAt(j); sm++;
                            j--;
                        }
                    }
                    data[num][i].simple();
                }
            }
            return sm;
        }
        public void mul(int multo, ref one mul0)
        {
            int i;
            for (i = 0; i < data[multo].Count; i++) data[multo][i].mul(ref mul0);
        }
        public void muladd(int addto, ref List<one> mul0, ref one mul1, int cn)
        {
            int i;
            for (i = 0; i < cn; i++)
            {
                one tmp = mul0[i];
                data[addto].Add(new one(ref tmp));
                data[addto][data[addto].Count - 1].mul(ref mul1);
            }
        }
        public void muladd(int addto, ref List<one> mul0, ref one mul1)
        {
            muladd(addto,ref mul0,ref mul1,mul0.Count);
        }
        public void add(int addto, ref List<one> add0)
        {
            int i;
            for (i = 0; i < add0.Count; i++) {one tmp = add0[i]; data[addto].Add(new one(ref tmp));}
            simple(addto);
        }
        public void sub(int addto, ref List<one> add0)
        {
            int i;
            for (i = 0; i < add0.Count; i++) {one tmp = add0[i]; data[addto].Add(new one(ref tmp,-1));}
            simple(addto);
        }
        public void mul(int multo, ref List<one> mul0)
        {
            int i,cn;
            if (mul0.Count == 0) {
                data[multo].RemoveRange(0,data[multo].Count); return;
            }
            for (cn = data[multo].Count, i = 0; i < mul0.Count; i++) {one tmp = mul0[i]; muladd(multo, ref data[multo], ref tmp,cn);}
            data[multo].RemoveRange(0,cn);
            simple(multo);
        }
        public void muladd(int muladdto, ref List<one> mul0)
        {
            int i,cn;
            if (mul0.Count < 1) return;
            cn = data[muladdto].Count;
            for (i = 0; i < mul0.Count; i++) {one tmp = mul0[i]; muladd(muladdto, ref data[muladdto], ref tmp, cn);}
        }
        public one extract(ref List<one> from)
        {
            one res; int i0; 
            if (from.Count == 0) res = new one(this, 1); else 
            {
                one tmp = from[0]; res = new one(ref tmp);
                for (i0 = 1; i0 < from.Count; i0++) res.extract(ref tmp);
            }
            return res;
        }
        public void diff(int ud, int val) {
            num neg = new num(-1), tmp = new num(0), vexp;
            int i = 0, ii = data[ud].Count; while (i < ii) 
            {
                if (data[ud][i].exps[val].iszero()) { data[ud].RemoveAt(i); ii--; }
                else
                {
                    tmp.set(data[ud][i].exps[val].non);
                    data[ud][i].exps[val].non.add(neg);
                    if (data[ud][i].exps[val].vars != null)
                    {
                        one otmp, oadd;
                        for (int i0 = 0; i0 < head.size; i0++)
                        {
                            vexp = data[ud][i].exps[val].vars[i0];
                            if (vexp.nonzero())
                            {
                                otmp = data[ud][i]; oadd = new one(ref otmp);
                                oadd.exps[i0].non.add(vexp);
                                data[ud].Add(oadd);
                            }
                        }
                    }
                    data[ud][i].mult.mul(tmp);
                    i++;
                }
            }
        }
        public void neg(int ud) {
            foreach (one u in data[ud]) u.mult.neg();
        }

        public int simple()
        {
            List<one> _up = data[0], _dw = data[1];
            one f_up, f_down, f_both;
            int i0,sm;
            sm=simple(0);
            sm+=simple(1);
            f_up = extract(ref _up);
            f_down = extract(ref _dw);
            f_both = new one(ref f_up);
            f_both.extract(ref f_down);
            f_both.div();
            mul(0, ref f_both);
            mul(1, ref f_both);
            f_up.mult.one();
            f_down.mult.one();
            for (i0 = 0; i0 < head.size; i0++)
            {
                if (f_up.exps[i0].ispos()) f_up.exp_zero(i0);
                if (f_down.exps[i0].ispos()) f_down.exp_zero(i0);
            }
            f_up.div();
            mul(0, ref f_up);
            mul(1, ref f_up);
            f_down.div();
            mul(0, ref f_down);
            mul(1, ref f_down);
            sm+=simple(0);
            sm+=simple(1);
            return sm;
        }
        public void revert_mult(int ud) 
        {
            BigInteger _div = 1,_t;
            foreach (one u in data[ud]) 
            {
                _t = u.mult.get_down();
                if ((_div % _t) > 0) _div *= (_t / BigInteger.GreatestCommonDivisor(_div,_t));
            }
            num div = new num(_div);
            foreach (one u in data[ud])  
            {
                u.mult.mul(div); u.mult.simple();
            }
            foreach (one d in data[1-ud]) 
            {
                d.mult.mul(div); d.mult.simple();
            }
        }

        public bool revert(int ud, int val) //_val^(-x) -> /_val^(x)
        {
            exp _min = null;
            foreach (one u in data[ud]) {exp tmp = u.exps[val]; if (_min == null) _min = new exp(ref tmp); else _min.min(ref tmp);}
            if (_min.iszero()) return false;
            foreach (one u in data[ud]) {u.exps[val].add(ref _min,-1); u.islist = false; }
            foreach (one d in data[1-ud]) {d.exps[val].add(ref _min,-1); d.islist = false; }
            return true;
        }

        public bool expand(int ud, int val)
        {
            int i,j;
            bool ret = false;
            exp _exp;
            num _dex = new num();
            many id = head.values[head.val_to_var(val)];
            for (i = 0; i < data[ud].Count; i++)
            {
                _exp = new exp(ref data[ud][i].exps[val]);
                if ((_exp.vars == null) && _exp.non.nonzero()) {
                    if (_exp.non.get_down() == 1)
                    {
                        many mtmp = new many(ref head,0);
                        mtmp.add(0, ref id.data[_exp.non.get_sign() > 0 ? 0 : 1]);
                        for (j = (int)_exp.non.get_up(); j > 1; j--) mtmp.mul(0, ref id.data[_exp.non.get_sign() > 0 ? 0 : 1]);
                        data[ud][i].exp_zero(val);
                        one otmp = data[ud][i];
                        mtmp.mul(0, ref otmp);
                        data[ud].RemoveAt(i);

                        mtmp.data[1].RemoveAt(0); mtmp.add(1, ref id.data[_exp.non.get_sign() > 0 ? 1 : 0]);
                        for (j = (int)_exp.non.get_up(); j > 1; j--) mtmp.mul(1, ref id.data[_exp.non.get_sign() > 0 ? 1 : 0]);

                        mul(ud, ref mtmp.data[1]);

                        add(ud, ref mtmp.data[0]);
                        mul(1-ud, ref mtmp.data[1]);
                        i--;
                    } else {
                        if ((id.data[0].Count == 1) && (id.data[1].Count == 1))
                        {
                            one u, d, ru, rd, tmp;
                            _dex.set(1,new BigInteger(1), _exp.non.get_down());
                            tmp = id.data[_exp.non.get_sign() > 0 ? 0 : 1][0]; u = new one(ref tmp);
                            tmp = id.data[_exp.non.get_sign() > 0 ? 1 : 0][0]; d = new one(ref tmp);
                            if (u.mult.root((int)_exp.non.get_down()) && d.mult.root((int)_exp.non.get_down()))
                            {
                                for (j = 0; j < head.size; j++)
                                {
                                    if (!u.exps[j].iszero()) u.exps[j].mul(_dex);
                                    if (!d.exps[j].iszero()) d.exps[j].mul(_dex);
                                }
                                ru = new one(ref u); rd = new one(ref d);
                                for (j = (int)_exp.non.get_up(); j > 1; j--) ru.mul(ref u);
                                for (j = (int)_exp.non.get_up(); j > 1; j--) rd.mul(ref d);
                                data[ud][i].exp_zero(val);
                                d.div();
                                data[ud][i].mul(ref u);
                                data[ud][i].mul(ref d);
                            }
                        }
                    }
                    ret = true;
                }
            }
            return ret;
        }

        public void revert(int val)
        {
            revert(0,val);
            revert(1,val);
        }
        public bool revert()
        {
            if (data[1].Count > 1) return false;
            one tmp = data[1][0]; tmp.div();
            mul(0, ref tmp); simple(0);
            data[1][0] = new one(this, 1);
            return true;
        }
        public void expand(int val)
        {
            bool f,f0,f1;
            if (head.values[head.val_to_var(val)] == null) return;
            f = true; while ( f )
            {
                f0 = expand(0,val);
                f1 = expand(1,val);
                f = f0 | f1;
                if (f) simple();
                f = false;
            }
        }

        public void print(int f)
        {
            bool hasdiv;
            string s0 = head.get_name(id) + " =";
            int i;
            switch (tfunc) {
                case 0:
                    s0 += "/" + (pfunc.get_sup() > 0 ? pfunc.get_sup().ToString() : "") + " ";
                    break;
                case 1:
                    s0 += "& " + s0;
                    break;
                case 2:
                    s0 += "^" + head.get_name_onval((int)(pfunc.get_up())) + " ";
                    break;
                default:
                    s0 += " " ;
                    break;
            }
            head.sys.wstr(f,ref s0); s0 = "";
            if ((data[0].Count > 0) && (data[1].Count > 0)) 
            {
                hasdiv = ((data[1].Count > 1) || (!data[1][0].mult.isone()) || (data[1][0].mult.get_sign() < 0));
                for (i = 0; i < head.size; i++) if (!data[1][0].exps[i].iszero()) hasdiv = true;
                if (hasdiv) {
                    print(f,0); head.sys.wstr(f,"//");  print(f,1);
                } else {
                    if ((data[0].Count == 1) && data[0][0].mult.iszero()) head.sys.wstr(f,"0"); else print(f,0);
                }
            }
            head.sys.wline(f,"");
        }
        public void print(int f, int n)
        {
            int i0, i1;
            bool f_one, f_many = true;
            string s0, s1;
            for (s0 = "", i0 = 0; i0 < data[n].Count; i0++)
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
                head.sys.progr(i0,data[n].Count);
                if (s0.Length > 666) {head.sys.wstr(f,ref s0); s0 = "";}
            }
            head.sys.wstr(f,ref s0);
        }
        public num calc()
        {
            num rt = new num(0);
                if (head.calc_flg[id]) head.sys.error(head.get_name(id) + " recursion look recursion");
                head.calc_flg[id] = true;
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
                if (u.islist) for (i = 0; u.list[i] > -1; i++) t0.mul(calc_exp(ref tmp.exps[u.list[i]],u.list[i]));
                else for (i = 0; i < head.size; i++) if (!u.exps[i].iszero()) t0.mul(calc_exp(ref tmp.exps[i],i));
                tr.add(t0);
            }
            return tr;
        }
        num calc_exp(ref exp ex, int i)
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
        public mao_dict(int v, ref ids r)
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
                if (lval >= nvals) return -1;
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
        public mao_key(ref mao_dict d)
        {
            dict = d;
            key = new ushort[d.nvals];
            for (int i = 0; i < d.nvals; i++) key[i]=0;
        }
        public mao_key(ref mao_dict d, ushort[] k)
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

        public many_as_one(ref mao_dict d)
        {
            dict = d;
            _data_i();
        }
        void _data_i(){
            data = new SortedDictionary<mao_key,num>[2];
            data[0] = new SortedDictionary<mao_key, num>();
            data[1] = new SortedDictionary<mao_key, num>();
        }
        public KeyValuePair<mao_key,num> fr_one(ref one o)
        {
            int i0,v0;
            KeyValuePair<mao_key, num> ret = new KeyValuePair<mao_key,num>(new mao_key(ref dict), new num(o.mult));
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
        public one to_one(KeyValuePair<mao_key, num> fr, ref many m)
        {
            int i;
            one ret = new one(ref m, fr.Value);
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
            KeyValuePair<mao_key,num> tmp = new KeyValuePair<mao_key,num>(new mao_key(ref dict), new num(0));
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
        void set(ref many_as_one fr)
        {
            for (int i = 0; i < 2; i++)
            {
                data[i].Clear();
                foreach (KeyValuePair<mao_key, num> d in fr.data[i]) data[i].Add(new mao_key(d.Key), new num(d.Value));
            }
        }
        public many_as_one(ref many m, ref mao_dict d)
        {
            one tmp;
            dict = d;
            _data_i();
            foreach (one o in m.data[0]) {tmp = o; add(0, fr_one(ref tmp));}
            foreach (one o in m.data[1]) {tmp = o; add(1, fr_one(ref tmp));}
        }
        public many to_many(ref ids h, int var)
        {
            int i=0, cn = data[0].Count + data[1].Count;
            many ret = new many(ref h,var); ret.data[1].RemoveAt(0);
            foreach (KeyValuePair<mao_key, num> d in data[0]) {ret.data[0].Add(to_one(d, ref ret)); dict.root.sys.progr(i++,cn);}
            foreach (KeyValuePair<mao_key, num> d in data[1]) {ret.data[1].Add(to_one(d, ref ret)); dict.root.sys.progr(i++,cn);}
            return ret;
        }

        public many_as_one(ref many_as_one _m, int _e)
        {
            dict = _m.dict;
            many_as_one tmp = new many_as_one(ref dict);
            many_as_one _tmp = new many_as_one(ref dict);
            many_as_one fr = new many_as_one(ref dict);
            num exp = dict.exps[_e], nexp = new num(0);
            int i0,_eu = (int)(exp.get_up());
            fr.set(ref _m);
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
            data[0].Add(new mao_key(ref dict), new num(1));
            data[1].Add(new mao_key(ref dict), new num(1));

            tmp.set(ref fr);
            for (int i = _eu; i > 0; i >>= 1) { 
                if ((i&1) != 0) {
                     mul(0,ref tmp.data[0]);
                     mul(1,ref tmp.data[1]);
                }
                if (i > 1) {
                    _tmp.set(ref tmp);
                    tmp.mul(0,ref _tmp.data[0], ref _tmp.data[0]);
                    tmp.mul(1,ref _tmp.data[1], ref _tmp.data[1]);
                }
            }

            if (exp.get_sign() < 0) {tmp.data[0] = data[0]; data[0] = data[1]; data[1] = tmp.data[0];}
        }
        public bool expand(int n, ref many_as_one e, int val)
        {
            bool ret = false;
            int ex = dict.val(val),ee; ushort tex;
            num max_u = new num(0), max_d = new num(0), now_u = new num(0), now_d = new num(0);
            many_as_one[] me = new many_as_one[254], ae = new many_as_one[254];
            mao_key z = new mao_key(ref dict);
            KeyValuePair<mao_key, num> tu = new KeyValuePair<mao_key,num>(new mao_key(ref dict), new num(0));
            me[0] = new many_as_one(ref e,0);
            me[1] = new many_as_one(ref e,1);
            if ((e.data[0].Count < 1) || (e.data[1].Count < 1)) dict.root.sys.error("wrong");
            int pnow = 0;
            foreach (KeyValuePair<mao_key, num> u in data[n]) 
            {
                tex=u.Key.key[ex];
                tu.Key.set(u.Key);
                tu.Value.set(u.Value);
                if (me[tex] == null) {
                    me[tex] = new many_as_one(ref e,tex);
                    if (me[tex].data == null) me[tex] = null; else 
                    {
                        if (max_u.great(dict.exps[tex])) max_u.set(dict.exps[tex]);
                        if (!max_d.great(dict.exps[tex])) max_d.set(dict.exps[tex]);
                    }
                }
                if (me[tex] != null) tu.Key.key[ex] = 0; else tex = 0;
                if (ae[tex] == null) ae[tex] = new many_as_one(ref dict);
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
                    if (me[ee] == null) me[ee] = new many_as_one(ref e,ee);
                    ae[tex].mul(0, ref me[ee].data[1]);
                    ee = dict.exp(now_d);
                    if (me[ee] == null) me[ee] = new many_as_one(ref e,ee);
                    ae[tex].mul(0, ref me[ee].data[0]);
                }
                dict.root.sys.progr(tex,254);
            }
            ee = dict.exp(now_u);
            if (me[ee] == null) me[ee] = new many_as_one(ref e,ee);
            mul(1-n, ref me[ee].data[1]);
            ee = dict.exp(now_d);
            if (me[ee] == null) me[ee] = new many_as_one(ref e,ee);
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
        public bool expand(ref many_as_one e, int id)
        {
            bool r0 =  expand(0,ref e,id);
            bool r1 =  expand(1,ref e,id);
            return r0 | r1;
        }
    }


    class fileio: IDisposable
    {
        StreamReader fin;
        StreamWriter[] fout;
        parse head;
        int nline,ncline, lines, clines;
        string buf,nout;
        public Boolean has, quit;
        public fileio(string nin, string _nout, parse h)
        {
            StreamReader fc; string ts;
            if (!File.Exists(nin)) Environment.Exit(-1);
            fc = new StreamReader(nin);
            clines++; lines = 0; quit = false; while ((ts = fc.ReadLine()) != null)
            {
                if (ts == "`end") break;
                if (ts == "`quit") { quit = true; break; }
                lines++;
                if ((ts.Length > 4) && (ts[0] != '`') && ((ts[0] != '#') || (ts[1] != '#'))) clines++;
            }
            fc.Close();
            fin = new StreamReader(nin);
            nout = _nout;
            fout = new StreamWriter[10];
            fout[0] = new StreamWriter(nout + "0");
            nline = 0; ncline = 0; has = true;
            head = h; buf = "";
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
                nline++; r = fin.ReadLine();
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
            fout[0].WriteLine("Line {0:G} Pos {0:G}: " + e, nline, head.pos);
            fout[0].Flush();
            Environment.Exit(-1);
        }
        public void wline(int n, string s)
        {
            if (fout[n] == null) fout[n] = new StreamWriter(nout + n.ToString().Trim());
            fout[n].WriteLine(s);
            fout[n].Flush();
        }
        public void wstr(int n, ref string s)
        {
            if (fout[n] == null) fout[n] = new StreamWriter(nout + n.ToString().Trim());
            fout[n].Write(s);
            fout[n].Flush();
        }
        public void wstr(int n, string s)
        {
            if (fout[n] == null) fout[n] = new StreamWriter(nout + n.ToString().Trim());
            fout[n].Write(s);
            fout[n].Flush();

        }
        public void Dispose()
        {
            fin.Close();
            for (int n = 0; n < 10; n++) if (fout[n] == null) fout[n].Close();
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
        public bool next()
        {
            string s0,s1,st,sf;
            int i0,i1,i2;//,i3,i4,i5,i6, deep;
            bool l_add = true;
            val = sys.rline(); val = val.Replace(" ",""); pos = 0;
            if ((val.Length == 0) || (val[0] == '`')) return false;
            if (val.Substring(0,2) == "##")
            {
                pos = 2; name = snext(false);
                if (!isnum(val, pos+1)) sys.error("macro: wrong num");
                int _np = (int)(val[pos+1] - '0'), _nm = -1;
                string _m = val.Substring(pos + 2);
                for (i0 = 0; i0 < m_name.Count; i0++) if (m_name[i0].IndexOf("#" + name + "(") > -1) _nm = i0;
                for (i0 = 0; i0 < 10; i0++)
                {
                    if (_m.IndexOf("#" + ((char)(i0 + '0')).ToString()) > -1)
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
                        s0 = s0.Replace("#" + ((char)(i2 + '0')).ToString(), s1);
                    }
                    if (((i2 == m_nparm[i0] - 1) && (now() != ')')) || ((i2 < m_nparm[i0] - 1) && (now() != ','))) 
                        sys.error("macro: call nparm");
                }
                if (m_nparm[i0] == 0) snext(false);
                sf = val.Substring(0, i1); st = (pos < val.Length ? val.Substring(pos, val.Length - pos) : "");
                if (ploop < 0) val = sf + s0 + st;
                else
                {
                    if (l_add) for (s1 = "", i2 = floop; i2 <= tloop; i2++) s1 += s0.Replace("#" + ((char)(ploop + '0')).ToString(), i2.ToString().Trim());
                    else for (s1 = "", i2 = floop; i2 >= tloop; i2--) s1 += s0.Replace("#" + ((char)(ploop + '0')).ToString(), i2.ToString().Trim());
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
        public BigInteger get_parm(ref ids root)
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
                              exp tn = new exp(ref data.head,0);
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
                              data.exps[val].add(ref tn,(div ? -1: 1));
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
                      exp tmp = new exp(ref data.head,1);
                      if (par.now() != '^') data.exps[val].add(ref tmp,div ? -1 : 1);
                  }
              }
          }
        }


        [STAThread]
        static int Main(string[] args)
        {
            int sx=0, sy=0;
            if (args.Length < 2) return 0;
            par = new parse(args[0], args[1], "#&!@$+-=*/^(),~:<>\"[]");
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
        static void doit() {
            int var0,var1,var2,val0;
            string val;
            int x0,x1,f0,f1,c0,c1;
            int[] xid = new int[99];
            int[] xout = new int[99];
            string[] xstr = new string[99];
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
                            root.values[var0] = new many(ref root,var0);
                            switch (par.now())
                            {
                            case '/':
                                par.snext(false); root.values[var0].tfunc = 0;
                                if (!par.isdelim(par.now())) root.values[var0].pfunc = par.nnext(false);
                                break;
                            case '&':
                                root.values[var0].tfunc = 1; root.values[var0].pfunc.set(0); par.snext(false);
                                break;
                            case '^':
                                root.values[var0].tfunc = 2; root.values[var0].pfunc.set(root.find_val(par.snext(true)));
                                if (root.values[var0].pfunc.get_sup() < 0) par.sys.error("row: no var");
                                break;
                            }
                            while (par.more())
                            {
                                root.values[var0].data[nowdiv].Add(new one(ref root.values[var0], new num(1)));
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
                        while (par.more())
                        {
                            val = par.snext(false); if (val.Length < 1) break;
                            val0 = root.find_val(val);
                            if (root.val_to_var(val0) == var0) par.sys.error(root.get_name(var0) + " $recursion - look recursion");
                            root.values[var0].revert(val0);
                            _id.Add(val0);
                            if (par.now() == '$') _div=true;
                            par.snext(false);
                        }
                        for (int i=0; i < _id.Count; i++) root.values[var0].expand(_id[i]);
                        if (_div && (root.values[var0].data[1].Count == 1))
                        {
                            root.values[var0].data[1][0].div();
                            one tmp = root.values[var0].data[1][0];
                            root.values[var0].mul(0, ref tmp);
                            root.values[var0].data[1][0] = new one(ref root.values[var0], 1);
                        }
                        }
                        else
                        {
                        mao_dict mdict = new mao_dict((int)par.nnext(true).get_up(),ref root);
                        many_as_one[] mao_fr = new many_as_one[root.vars];
                        List<int> _id = new List<int>();
                        while (par.more())
                        {
                            val = par.snext(true); if (val.Length < 1) break;
                            val0 = root.find_val(val); var1 = root.val_to_var(val0);
                            if (var1 == var0) par.sys.error(root.get_name(var0) + " $recursion - look recursion");
                            if (root.values[var1] != null) {
                                root.values[var0].revert(val0);
                                _id.Add(val0);
                                mao_fr[_id.Count - 1] = new many_as_one(ref root.values[var1],ref mdict);
                            }
                            if (par.now() == '$') _div = true;
                        }
                        many_as_one mao_to = new many_as_one(ref root.values[var0],ref mdict);
                        bool r = true;
                        while (r)
                        {
                            r = false;
                            for (int i = 0; i < _id.Count; i++)
                            {
                                r = mao_to.expand(ref mao_fr[i], _id[i]) || r;
                            }
                            
                        }
                        root.values[var0] = mao_to.to_many(ref root,var0);
                        if (_div && (root.values[var0].data[1].Count == 1))
                        {
                            root.values[var0].data[1][0].div();
                            one tmp = root.values[var0].data[1][0];
                            root.values[var0].mul(0, ref tmp);
                            root.values[var0].data[1][0] = new one(ref root.values[var0], 1);
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
                            many tmp = new many(ref root.values[var0]);
                            root.values[var0].diff(0,val0);
                            root.values[var0].mul(1, ref tmp.data[1]); //d^2
                            root.values[var0].mul(0, ref tmp.data[1]); //u'*d
                            tmp.diff(1, val0);
                            tmp.mul(0, ref tmp.data[1]); //u*d'
                            tmp.neg(0);
                            root.values[var0].add(0,ref tmp.data[0]);//u'*d-u*d'
                            root.values[var0].simple();
                            root.values[var0].print(0);
                         }
                            break;
                     case '@':
                        {
                            char _c;
                            string nn, s_val;
                            one _dv, _ml, odv = null;
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
                                    root.values[var1] = new many(ref root.values[var0], 0);
                                    root.values[var2] = new many(ref root.values[var0], 1);
                                }
                                else
                                {
                                    root.values[var1] = new many(ref root.values[var0]);
                                    root.values[var1].revert();
                                    root.values[var2] = new many(ref root, 1);
                                }
                                root.values[var1].id = var1; root.values[var2].id = var2;
                                root.values[var1].print(0); root.values[var2].print(0);
                                break;
                            }
                            x0 = -1; if (par.now() == '!') x0 = (int)par.nnext(true).get_up();
                            mao_dict mdict = new mao_dict((x0 > 0 ? x0 : 6), ref root);
                            many_as_one mdv = (x0 > 0 ? new many_as_one(ref root.values[var0], ref mdict) : new many_as_one(ref mdict));
                            many dv = (x0 > 0 ? new many(ref root, 0) : new many(ref root.values[var0]));
                            if (par.now() == '=')
                            {
                                val = par.snext(true);
                                odv = new one(ref dv, new num(par.isnum(val) ? val : "1"));
                                if (!par.isnum(val)) odv.exps[root.find_val(val)].non.set(1);
                                odv.mult.neg();
                            }
                            _c = par.now(); s_val = par.snext(true);
                            switch (_c)
                            {
                                case '&':
                                    _ml = new one(ref dv, 1);
                                    _ml.exps[root.find_val(s_val)].non.set(1);
                                    break;
                                case '$':
                                    var1 = root.val_to_var(root.find_val(s_val));
                                    if (root.values[var1] == null) par.sys.error("@ empty $");
                                    if ((root.values[var1].data[0].Count != 1) || (root.values[var1].data[1].Count != 1)) par.sys.error("one only");
                                    one tmp = root.values[var1].data[0][0], tmp1 = root.values[var1].data[1][0];
                                    tmp.mul(ref tmp1);
                                    _ml = new one(ref tmp);
                                    break;
                                default:
                                    par.sys.error("@ wrong shard opt");
                                    _ml = new one(ref dv, 1);
                                    break;
                            }
                            _ml.simple(); _dv = new one(ref _ml); _dv.div();
                            num e = new num(); int ip = root.last;
                            if (x0 < 0)
                            {
                                SortedDictionary<num, many> res = new SortedDictionary<num, many>();
                                if (odv != null)
                                {
                                    if (odv.mult.nonzero())
                                    {
                                        dv.mul(1, ref odv);
                                        dv.add(0, ref dv.data[1]);
                                        dv.simple(0);
                                    }
                                    dv.data[1].RemoveRange(0, dv.data[1].Count); 
                                    dv.data[1].Add(new one(ref dv, 1));
                                }
                                else
                                {
                                    if (!dv.revert()) par.sys.error("@ can't shard this many");
                                }
                                int i1 = dv.data[0].Count;
                                while (dv.data[0].Count > 0)
                                {
                                    e.zero();
                                    odv = dv.data[0][0];
                                    if (odv.test_mul(ref _ml))
                                    {
                                        while (odv.mul_t(ref _ml)) e.add(-1);
                                        odv.mul(ref _dv);
                                    }
                                    else if (odv.test_mul(ref _dv))
                                    {
                                        while (odv.mul_t(ref _dv)) e.add(1);
                                        odv.mul(ref _ml);
                                    }
                                    if (!res.ContainsKey(e)) res.Add(new num(e), new many(ref root,-1));
                                    res[e].data[0].Add(odv);
                                    e.zero(); dv.data[0].RemoveAt(0);
                                    root.sys.progr(i1 - dv.data[0].Count, i1);
                                }
                                e = null;
                                foreach (KeyValuePair<num, many> _d in res)
                                {
                                    nn = s_val + (_d.Key.nonzero() ? ((_d.Key.get_sign() < 0 ? "m" : "p") + _d.Key.get_up().ToString().Trim() + (_d.Key.get_down() > 1 ? ("\\" + _d.Key.get_down().ToString().Trim()) : "")) : "_0") + "_" + par.name;
                                    if ((var1 = root.find_var(nn)) < 0)
                                    {
                                        var1 = root.set_empty(nn);
                                        _d.Value.id = var1;
                                        root.values[var1] = _d.Value;
                                    }
                                    else par.sys.error("@ overwrite " + nn);
                                }
                            }
                            else
                            {
                                SortedDictionary<num, many_as_one> res = new SortedDictionary<num, many_as_one>();
                                KeyValuePair<mao_key, num> tkey, kml,kdv;
                                if (odv != null)
                                {
                                    tkey = mdv.fr_one(ref odv);
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
                                mdv.add(1,mdv.fr_one(ref odv));
                                kml = mdv.fr_one(ref _ml); kdv = mdv.fr_one(ref _dv);
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
                                    if (!res.ContainsKey(e)) res.Add(new num(e), new many_as_one(ref mdict));
                                    res[e].add(0,ref tkey);
                                    e.zero();
                                    root.sys.progr(i1 - mdv.data[0].Count, i1);
                                }
                                e = null;
                                foreach (KeyValuePair<num, many_as_one> _d in res)
                                {
                                    nn = s_val + (_d.Key.nonzero() ? ((_d.Key.get_sign() < 0 ? "m" : "p") + _d.Key.get_up().ToString().Trim() + (_d.Key.get_down() > 1 ? ("\\" + _d.Key.get_down().ToString().Trim()) : "")) : "_0") + "_" + par.name;
                                    if ((var1 = root.find_var(nn)) < 0)
                                    {
                                        var1 = root.set_empty(nn);
                                        root.values[var1] = _d.Value.to_many(ref root,var1);
                                        root.values[var1].data[1].Add(new one(ref root.values[var1],1));
                                    }
                                    else par.sys.error("@ overwrite " + nn);
                                }
                            }
                            for (int i0 = ip; i0 < root.last; i0++) root.values[i0].print(0);
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
                        BigInteger _fr = 0,_fr0,_to = 0,_one = 1,_res1;
                        int _typ = 0, _all, i0;
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
                            _fr = par.get_parm(ref root);
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
                            _to = par.get_parm(ref root);
                            if ((xid[0] = root.find_var(par.snext(true))) < 0) par.sys.error("loop: no name");
                            if (root.values[xid[0]] != null) par.sys.error("loop: must non");
                        }
                        for (_all = 1; par.more() && (_all < xout.Length); _all++)
                        {
                            par.snext(false);
                            if (par.now() != '"') {
                                val = par.snext(false); if (val.Length < 1) break;
                                xid[_all] = root.find_val(val);
                            } else xid[_all] = -1;
                            val = par.snext(false); if (val.Length < 1) break;
                            xstr[_all] = val.Substring(1);
                            xout[_all] = (int)(par.nnext(false).get_up());
                        }
                        string[] _out = new string[11];
                        _fr0 = _fr; while (true)
                        {
                            if (!_singl)
                            {
                                if (l_add) { if (_fr > _to) break; } else { if (_fr < _to) break; }
                                root.uncalc();
                                root.set_var(xid[0], new num(1, _fr, _one));
                            }
                            i0 = 0; while (i0 < 10) _out[i0++]="";
                            i0 = 1; while (i0 < _all) {
                                if (xid[i0] > -1) {
                                    if (_typ == 0) {
                                        _res1 = root.get_val(xid[i0]).toint();
                                        _out[xout[i0]] += _res1.ToString(xstr[i0]);
                                    } else {
                                        _out[xout[i0]] += root.get_val(xid[i0]).print("","-","");
                                    }
                                } else {
                                    _out[xout[i0]] += xstr[i0];
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
