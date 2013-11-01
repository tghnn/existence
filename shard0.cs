//for Them
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
    public partial class Form1 : Form
    {
        public System.Drawing.Bitmap bm;
        public int sx, sy;
        delegate void SetCallback(int i);
        System.Drawing.Graphics Gr;
        public bool rp;
        public Form1(int x, int y)
        {
            int i0,i1;
            rp = true;
            sx = x; sy = y;
            bm = new System.Drawing.Bitmap(x, y);
            for (i0 = 0; i0 < sx; i0++) for (i1 = 0; i1 < sy; i1++) bm.SetPixel(i0, i1, Color.FromArgb(6, 6, 6));
            this.Width = sx + 0;
            this.Height = sy + 0;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Gr = this.CreateGraphics();
            Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
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
                Gr.DrawImageUnscaled(bm, 0, 0);
            }
        }
        private void From1_Shown(object sender, EventArgs e)
        {
            if (rp) Gr.DrawImageUnscaled(bm, 0, 0);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (rp) Gr.DrawImageUnscaled(bm, 0, 0);
        }
    }


    class ids
    {
        public int size, last;
        public string[] names;
        public many[] values;
        public num[] calc;
        public fileio sys;
        public ids(int s, fileio f)
        {
            size = s; sys = f;
            last = 0;
            names = new string[s];
            values = new many[s];
            calc = new num[s];
        }
        public int set_empty(string nam)
        {
            if (last >= size) return -1;
            names[last] = nam;
            values[last] = null;
            calc[last] = new num();
            last++; return last - 1;
        }
        public void uncalc()
        {
            for (int i = 0; i < last; i++) calc[i].unset();
        }
        public int find(string n)
        {
            int i;
            for (i = 0; i < size; i++) if (n == names[i]) return i;
            return -1;
        }
    }
    class num
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
            sign = n.get_sign();
            up = BigInteger.Abs(n.get_up());
            down = BigInteger.Abs(n.get_down());
            exs = true;
        }
        public void set(num n,int s)
        {
            sign = n.get_sign() * s;
            up = BigInteger.Abs(n.get_up());
            down = BigInteger.Abs(n.get_down());
            exs = true;
        }
        public void set(string s)
        {
            sign = 1;
            down = 1;
            exs = BigInteger.TryParse(s, out up);
        }
        public num(int _s, BigInteger _u, BigInteger _d)
        {
            set(_s,_u,_d);
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
        public bool great(num a)
        {
            if (sign == a.get_sign())
            {
                return (a.get_up()*down > up*a.get_down());
            } else return (a.get_sign() > 0);
        }
        public bool cmp(num a)
        {
            return ((sign == a.get_sign()) && (up == a.get_up()) && (down == a.get_down()));
        }

        public bool isone()
        {
            return (down == 1) && (up == 1);
        }
        public void simple()
        {
            BigInteger a;
                if (up == 0) { sign = 1; down = 1; } else do
                {
                    a = BigInteger.GreatestCommonDivisor(up, down);
                    up = BigInteger.Divide(up, a);
                    down = BigInteger.Divide(down, a);
                } while (a != 1);
        }
        public void revert()
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
            if (ex < 0) revert();
        }
        public void exp(num ex)
        {
            exp((int)(ex.get_up()) * ex.get_sign());
        }
        public void mul(num a)
        {
            sign *= a.get_sign();
            up *= a.get_up();
            down *= a.get_down();
        }
        public void div(num a)
        {
            sign *= a.get_sign();
            up *= a.get_down();
            down *= a.get_up();
        }
        public void add_up(BigInteger a)
        {
            up += a;
        }
        public void add(num a, int s)
        {
            if (sign == 0) set(a,s); else {
            if (sign * (a.get_sign() * s) < 0)
            {
                up = up * a.get_down() - a.get_up() * down;
            }
            else
            {
                up = up * a.get_down() + a.get_up() * down;
            }
            down *= a.get_down();
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
            return sign * up / down;
        }
        public double todouble()
        {
            return (double)(sign * up) / (double)(down);
        }
        public string print(string pos, string neg, string b0, string b1)
        {
            string s0;
            s0 = "";
            if (down > 1) s0 += b0;
            s0 += (sign < 0 ? neg : pos);
            s0 += up.ToString().Trim();
            if (down > 1) s0 += "/" + down.ToString().Trim() + b1;
            return s0;
        }
    }
    class one
    {
        public many head;
        public num mult;
        public num[] exps;
        public one(many h, int m)
        {
            init_z(h);
            mult = new num(m);
        }
        public one(many h, num m)
        {
            init_z(h);
            mult = new num(m);
        }
        public one(one o)
        {
            init(o, 1);
        }
        public one(one o, int s)
        {
            init(o, s);
        }
        void init(one o, int s)
        {
            int i;
            init(o.head);
            mult = new num(o.mult); mult.set_sign(mult.get_sign() * s);
            for (i = 0; i < head.head.size; i++) exps[i] = new num(o.exps[i]);
        }
        void init(many h)
        {
            head = h;
            exps = new num[h.head.size];
        }
        void init_z(many h)
        {
            int i;
            init(h);
            for (i = 0; i < head.head.size; i++) exps[i] = new num(0);
        }

        public bool cmp(one a)
        {
            int i;
            if (head.head != a.head.head) return false;
            for (i = 0; i < head.head.size; i++)
            {
                if (!exps[i].cmp(a.exps[i])) break;
            }
            return (i == head.head.size);
        }
        public bool mul(one a)
        {
            int i;
            bool rt = true;
            if (head.head != a.head.head) return false;
            mult.mul(a.mult);
            for (i = 0; i < head.head.size; i++) { 
                exps[i].add(a.exps[i]);
                if ((a.exps[i].get_sign() != 0) && (a.exps[i].get_sign() == exps[i].get_sign())) rt = false;
            }
            return rt;
        }
        public bool div()
        {
            int i0;
            if (!mult.nonzero()) return false; mult.revert();
            for (i0 = 0; i0 < head.head.size; i0++) exps[i0].neg();
            return true;
        }

    }
    class many
    {
        public ids head;
        public List<one>[] data;
        public int id;
        public int tfunc;
        public num pfunc;
        public many(ids h, int i)
        {
            tfunc = -1; pfunc = new num(-1);
            head = h; id = i;
            data = new List<one>[2];
            data[0] = new List<one>();
            data[1] = new List<one>();
            data[1].Add(new one(this,1));
        }
        public many(many m)
        {
            tfunc = -1; pfunc = new num(0);
            head = m.head; id = m.id;
            data = new List<one>[2];
            data[0] = new List<one>();
            data[1] = new List<one>();
            foreach (one m0 in m.data[0]) data[0].Add(new one(m0));
            foreach (one m1 in m.data[1]) data[1].Add(new one(m1));
        }
        static public int simple(List<one> data)
        {
            int i, j,sm=0;
            for (i = 0; i < data.Count; i++)
            {
                data[i].mult.simple();
                for (j = i + 1; j < data.Count; j++)
                {
                    if (data[i].cmp(data[j]))
                    {
                        data[i].mult.add(data[j].mult);
                        data.RemoveAt(j); sm++;
                        j--;
                    }
                }
            }
            return sm;
        }
        static public void mul(List<one> multo, one mul0)
        {
            int i;
            for (i = 0; i < multo.Count; i++) multo[i].mul(mul0);
        }
        static public void muladd(List<one> addto, List<one> mul0, one mul1, int cn)
        {
            int i;
            for (i = 0; i < cn; i++)
            {
                addto.Add(new one(mul0[i]));
                addto[addto.Count - 1].mul(mul1);
            }
        }
        static public void muladd(List<one> addto, List<one> mul0, one mul1)
        {
            muladd(addto,mul0,mul1,mul0.Count);
        }
        static public void add(List<one> addto, List<one> add0)
        {
            int i;
            for (i = 0; i < add0.Count; i++) addto.Add(new one(add0[i]));
            simple(addto);
        }
        static public void sub(List<one> addto, List<one> add0)
        {
            int i;
            for (i = 0; i < add0.Count; i++) addto.Add(new one(add0[i],-1));
            simple(addto);
        }
        static public void mul(List<one> multo, List<one> mul0)
        {
            int i,cn;
            if (mul0.Count == 0) {
                multo.RemoveRange(0,multo.Count); return;
            }
            for (cn = multo.Count, i = 0; i < mul0.Count; i++) muladd(multo,multo,mul0[i],cn);
            multo.RemoveRange(0,cn);
            simple(multo);
        }
        static public void muladd(List<one> muladdto, List<one> mul0)
        {
            int i,cn;
            if (mul0.Count < 1) return;
            cn = muladdto.Count;
            for (i = 0; i < mul0.Count; i++) muladd(muladdto, muladdto, mul0[i], cn);
        }
        static public void extract(one to, one from)
        {
            int i0;
            to.mult.set((((to.mult.get_sign() < 0) && (from.mult.get_sign() < 0)) ? -1 : 1),BigInteger.GreatestCommonDivisor(to.mult.get_up(), from.mult.get_up()),BigInteger.GreatestCommonDivisor(to.mult.get_down(), from.mult.get_down()));
            for (i0 = 0; i0 < to.head.head.size; i0++)
            {
                if (from.exps[i0].get_sign() == to.exps[i0].get_sign())
                {
                    if (from.exps[i0].great(to.exps[i0])) to.exps[i0] = new num(from.exps[i0]);
                }
                else to.exps[i0].zero();
            }
        }
        public one extract(List<one> from)
        {
            one res; int i0; 
            if (from.Count == 0) res = new one(this, 1); else 
            {
                res = new one(from[0]);
                for (i0 = 1; i0 < from.Count; i0++) extract(res, from[i0]);
            }
            return res;
        }
        public int simple()
        {
            one f_up, f_down, f_both;
            int i0,sm;
            sm=simple(data[0]);
            sm+=simple(data[1]);
            f_up = extract(data[0]);
            f_down = extract(data[1]);
            f_both = new one(f_up);
            extract(f_both, f_down);
            f_both.div();
            mul(data[0], f_both);
            mul(data[1], f_both);
            f_up.mult.one();
            f_down.mult.one();
            for (i0 = 0; i0 < head.size; i0++)
            {
                if (f_up.exps[i0].get_sign() > 0) f_up.exps[i0].zero();
                if (f_down.exps[i0].get_sign() > 0) f_down.exps[i0].zero();
            }
            f_up.div();
            mul(data[0], f_up);
            mul(data[1], f_up);
            f_down.div();
            mul(data[0], f_down);
            mul(data[1], f_down);
            sm+=simple(data[0]);
            sm+=simple(data[1]);
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

        public bool revert(int ud, int _id) //_id^(-x) -> /_id^(x)
        {
            bool ret = false;
            num _exp, _max = new num(0);
            foreach (one u in data[ud]) 
            {
                _exp = new num(u.exps[_id]); 
                if ((_exp.nonzero()) && (_exp.get_sign() < 0)) 
                    {
                        if (_exp.great(_max)) _max.set(_exp);
                        ret = true;
                    }
            }
            if (!ret || (!_max.nonzero())) return ret;
            _max.neg();
            foreach (one u in data[ud]) u.exps[_id].add(_max);
            foreach (one d in data[1-ud]) d.exps[_id].add(_max);
            return ret;
        }

        public bool expand(int ud, int _id)
        {
            int i,j;
            bool ret = false;
            num _exp;
            num _dex = new num();
            many id = head.values[_id];
            for (i = 0; i < data[ud].Count; i++)
            {
                _exp = new num(data[ud][i].exps[_id]); if (_exp.get_sign() > 0) {
                    if (_exp.get_down() == 1)
                    {
                        List<one> u, d;
                        u = new List<one>();
                        add(u, id.data[_exp.get_sign() > 0 ? 0 : 1]);
                        for (j = (int)_exp.get_up(); j > 1; j--) mul(u, id.data[_exp.get_sign() > 0 ? 0 : 1]);
                        data[ud][i].exps[_id].zero();
                        mul(u, data[ud][i]);
                        data[ud].RemoveAt(i);

                        d = new List<one>();
                        add(d, id.data[_exp.get_sign() > 0 ? 1 : 0]);
                        for (j = (int)_exp.get_up(); j > 1; j--) mul(d, id.data[_exp.get_sign() > 0 ? 1 : 0]);
                        mul(data[ud], d);

                        add(data[ud], u);
                        mul(data[1-ud], d);
                        i--;
                    }
                    else
                    {
                        if ((id.data[0].Count == 1) && (id.data[1].Count == 1))
                        {
                            one u, d, ru, rd;
                            _dex.set(1,new BigInteger(1), _exp.get_down());
                            u = new one(id.data[_exp.get_sign() > 0 ? 0 : 1][0]);
                            d = new one(id.data[_exp.get_sign() > 0 ? 1 : 0][0]);
                            if (u.mult.root((int)_exp.get_down()) && d.mult.root((int)_exp.get_down()))
                            {
                                for (j = 0; j < head.size; j++)
                                {
                                    if (u.exps[j].nonzero()) u.exps[j].mul(_dex);
                                    if (d.exps[j].nonzero()) d.exps[j].mul(_dex);
                                }
                                ru = new one(u); rd = new one(d);
                                for (j = (int)_exp.get_up(); j > 1; j--) ru.mul(u);
                                for (j = (int)_exp.get_up(); j > 1; j--) rd.mul(d);
                                data[ud][i].exps[_id].zero();
                                d.div();
                                data[ud][i].mul(u);
                                data[ud][i].mul(d);
                            }
                        }
                    }
                    ret = true;
                }
            }
            return ret;
        }

        public void revert(int _id)
        {
            if (head.values[_id] == null) return;
            revert(0,_id);
            revert(1,_id);
        }
        public void expand(int _id)
        {
            bool f,f0,f1;
            if (head.values[_id] == null) return;
            f = true; while ( f )
            {
                f0 = expand(0,_id);
                f1 = expand(1,_id);
                f = f0 | f1;
                if (f) simple();
                f = false;
            }
        }

        public string print()
        {
            bool hasdiv;
            string s0 = "";
            int i;
            if ((data[0].Count < 1) || (data[1].Count < 1)) return "AAAA";
            hasdiv = ((data[1].Count > 1) || (!data[1][0].mult.isone()) || (data[1][0].mult.get_sign() < 0));
            for (i = 0; i < head.size; i++) if (data[1][0].exps[i].nonzero()) 
                hasdiv = true;
            s0 = (hasdiv ? print(data[0]) + "//" + print(data[1]) : print(data[0]));
            return s0;
        }
        static public string print(List<one> data)
        {
            int i0, i1;
            bool oneout;
            string s0;
            for (s0 = "", i0 = 0; i0 < data.Count; i0++)
            {
                if (data[i0].mult.nonzero())
                {
                    oneout = false;
                    for (i1 = 0; i1 < data[i0].head.head.size; i1++)
                    {
                        if (data[i0].exps[i1].nonzero())
                        {
                            if (!oneout) 
                            {
                                if (data[i0].mult.isone() && (data[i0].exps[i1].get_sign() > 0))  {
                                    s0 += (i0 > 0 ? (data[i0].mult.get_sign() < 0 ? "-" : "+") : (data[i0].mult.get_sign() < 0 ? "-": ""));
                                } else {
                                    s0 += data[i0].mult.print((i0 > 0 ? "+" : ""), "-", "", "");
                                }
                            }
                            s0 += (data[i0].exps[i1].get_sign() < 0 ? "/" : (oneout || (!data[i0].mult.isone()) ? "*" : "")) + data[i0].head.head.names[i1] + (data[i0].exps[i1].isone() ? "" : "^" + data[i0].exps[i1].print("","","(",")"));
                            oneout = true;
                        }
                    }
                    if (!oneout) s0 += data[i0].mult.print((i0 > 0 ? "+" : ""), "-", "", "");
                }
            }
            return s0;
        }
        public num calc(BigInteger prec)
        {
            if (!head.calc[id].exist())
            {
                if (tfunc < 1)
                {
                    num t0 = calc(0, prec);
                    num t1 = calc(1, prec);
                    head.calc[id].set(t0);
                    head.calc[id].div(t1);
                    head.calc[id].simple();
                }
                switch (tfunc)
                {
                    case 0:
                        {
                            int l0, l1;
                            l0 = head.calc[id].get_up().ToString().Length;
                            l1 = head.calc[id].get_down().ToString().Length;
                            if (l0 > l1) l0 = l1;
                            l0 -= (int)(pfunc.get_up());
                            if (l0 > 4)
                            {
                                string _s = "1" + new string('0', l0);
                                BigInteger _d, _au, _ad, _bu, _bd, _cu, _cd; BigInteger.TryParse(_s, out _d);
                                _au = head.calc[id].get_up() / _d; _bu = head.calc[id].get_up() % _d; _cu = _bu / _au;
                                _ad = head.calc[id].get_down() / _d; _bd = head.calc[id].get_down() % _d; _cd = _bd / _ad;
                                _d += (_cu + _cd) / 2;
                                head.calc[id].set(head.calc[id].get_sign(), head.calc[id].get_up() / _d, head.calc[id].get_down() / _d);
                            }
                        }
                        break;
                    case 1:
                        {
                            BigInteger[] ua, da;
                            int var = (int)(pfunc.get_up()),fe = 0, se,ne;
                            ne = data[0].Count;
                            if ((ne < 4) || (ne > 1000)) head.sys.error("row: wrong");
                            ua = new BigInteger[ne];
                            da = new BigInteger[ne];
                            fe = (int)(data[0][0].exps[var].get_sup());
                            se = (int)(data[0][1].exps[var].get_up()) - fe;
                            if ((fe < 0) || (se < 0) || (se > 11)) head.sys.error("row: wrong exp");
                            if (!head.calc[var].exist()) head.values[var].calc(prec);
                            BigInteger _u,_d,_um, _dm;
                            num tn = new num(head.calc[var]); tn.exp(fe);
                            _u = tn.get_sup(); _d = 1;
                            tn.set(head.calc[var]); tn.exp(se);
                            _um = tn.get_sup(); _dm = tn.get_down();
                            for (int i = 0; i < ne; i++)
                            {
                                ua[i] = _u; da[i] = _d; _u *= _um; _d *= _dm;
                            }
                            _u = 0;
                            for (int i = 0; i < ne; i++)
                            {
                                if (data[0][i].exps[var].get_sup() != fe + se*i) head.sys.error("row: wrong exp");
                                if (data[0][i].mult.get_down() > 1) head.sys.error("row: wrong mul");
                                _u += ua[i] * da[ne - i - 1] * data[0][i].mult.get_sup();
                            }
                            tn.set(head.calc[var]); tn.exp(fe);
                            _d = da[ne-1] * tn.get_down() * head.values[id].data[1][0].mult.get_sup();
                            head.calc[id].set(_u, _d);
                        }
                        break;
                }
            }
            return head.calc[id];
        }
        public num calc(int ud,BigInteger prec)
        {
            num tr = new num(0), t0 = new num(0), t1 = new num(0);
            num _prec = new num(1,prec,prec);
            foreach (one u in data[ud])
            {
                t0.set(u.mult);
                for (int i = 0; i < head.size; i++)
                {
                    num ex = u.exps[i];
                    if (ex.nonzero())
                    {
                        if (!head.calc[i].exist())
                        {
                            if (head.values[i] == null) head.sys.error("non is non");
                            head.values[i].calc(prec);
                        }
                        t1.set(head.calc[i]);
                        t1.exp((int)(ex.get_up()));
                        if (ex.get_down() > 1)
                        {
                            if (!t1.root((int)(ex.get_down())))
                            {
                                if (((t1.get_up() > 1) && (t1.get_up() < prec)) || ((t1.get_down() > 1) && (t1.get_down() < prec)))
                                {
                                    t1.mul(_prec);
                                }
                                t1.set(t1.get_sign(),t1._sq(t1.get_up(), (int)(ex.get_down())),t1._sq(t1.get_down(), (int)(ex.get_down())));
                            }
                        }
                        if (ex.get_sign() < 0) t1.revert();
                        t0.mul(t1);
                    }
                }
                tr.add(t0);
            }
            return tr;
        }

    }
    class mao_dict {
        public fileio fio;
        public int nvars;
        public num[] exps;
        public int[] vars, to_var;
        byte[] eadd;
        int lexp, lvar;
        public mao_dict(int v, int mvar, fileio f)
        {
            nvars = v; fio = f;
            exps = new num[256];
            vars = new int[v];
            to_var = new int[mvar];
            for (int i = 0; i < mvar; i++) to_var[i] = -1;
            eadd = new byte[256*256];
            for (int i = 0; i < 256*256; i++) eadd[i] = 255;
            for (int i = 0; i < 256; i++) {
                eadd[i] = (byte)i;
                eadd[i << 8] = (byte)i;
            }
            lexp = 2; exps[0] = new num(0); exps[1] = new num(1);
            lvar = 0;
        }
        public byte exp(num e)
        {
            int i;
            for (i = 0; i < lexp; i++) if (e.cmp(exps[i])) return (byte)(i);
            if (lexp >= 254) {
                fio.error("too many exp");
            }
            exps[lexp++] = new num(e);
            return (byte)(lexp - 1);
        }
        public int var(int v)
        {
            if (to_var[v] < 0)
            {
                if (lvar >= nvars) return -1;
                to_var[v] = lvar;
                vars[lvar++] = v;
            }
            return to_var[v];
        }
        public byte add(byte e0, byte e1)
        {
            int ee = (int)e0 + (((int)e1) << 8);
            if (eadd[ee] == 255)
            {
                num sum = new num(exps[e0]);
                sum.add(exps[e1]);
                sum.simple();
                eadd[ee] = exp(sum);
            }
            return eadd[ee];
        }
    }
    class mao_key :IComparable {
        public mao_dict dict;
        public byte[] key;
        public mao_key(mao_dict d)
        {
            dict = d;
            key = new byte[d.nvars];
            for (int i = 0; i < d.nvars; i++) key[i]=0;
        }
        public mao_key(mao_dict d, byte[] k)
        {
            dict = d;
            key = new byte[dict.nvars];
            for (int i = 0; i < d.nvars; i++) key[i] = k[i];
        }
        public mao_key(mao_key k)
        {
            dict = k.dict;
            key = new byte[dict.nvars];
            set(k);
        }
        public void set(mao_key k)
        {
            for (int i = 0; i < dict.nvars; i++) key[i] = k.key[i];
        }
        public int CompareTo(object obj) {
            if (obj == null) return 1;
            mao_key k = obj as mao_key;
            for (int i = 0; i < dict.nvars; i++) {
                if (key[i] > k.key[i]) return -1;
                if (key[i] < k.key[i]) return 1;
            }
            return 0;
        }
    }
    class many_as_one {
        mao_dict dict;
        SortedDictionary<mao_key,num>[] data;

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
            int i;
            KeyValuePair<mao_key, num> ret = new KeyValuePair<mao_key,num>(new mao_key(dict), new num(o.mult));
            for (i = 0; i < dict.nvars; i++) ret.Key.key[i] = 0;
            for (i = 0; i < o.exps.Length; i++)
            {
                if (o.exps[i].nonzero()) ret.Key.key[dict.var(i)] = dict.exp(o.exps[i]);
            }
            return ret;
        }
        public one to_one(KeyValuePair<mao_key, num> fr, many m)
        {
            int i;
            fr.Value.simple();
            one ret = new one(m, fr.Value);
            for (i = 0; i < dict.nvars; i++)
            {
                if (fr.Key.key[i] != 0) ret.exps[dict.vars[i]].set(dict.exps[fr.Key.key[i]]);
            }
            return ret;
        }

        public void add(int ud, KeyValuePair<mao_key,num> a)
        {
            if (data[ud].ContainsKey(a.Key)) data[ud][a.Key].add(a.Value); else data[ud].Add(new mao_key(a.Key), new num(a.Value));
        }
        public void add(int ud, SortedDictionary<mao_key, num> fr)
        {
            foreach (KeyValuePair<mao_key, num> d in fr) add(ud, d);
        }
        public void muladd(int ud, SortedDictionary<mao_key,num> fr, KeyValuePair<mao_key,num> a)
        {
            int i;
            KeyValuePair<mao_key,num> tmp = new KeyValuePair<mao_key,num>(new mao_key(dict), new num(0));
            foreach (KeyValuePair<mao_key,num> d in fr) {
                for (i = 0; i < dict.nvars; i++) {
                    tmp.Key.key[i] = dict.add(a.Key.key[i],d.Key.key[i]);
                }
                tmp.Value.set(a.Value); tmp.Value.mul(d.Value);
                add(ud,tmp);
            }
        }

        public void mul(int ud, SortedDictionary<mao_key,num> m0)
        {
            SortedDictionary<mao_key, num> tmp = data[ud];
            data[ud] = new SortedDictionary<mao_key, num>();
            foreach (KeyValuePair<mao_key, num> d in m0) muladd(ud, tmp, d);
        }
        public void mul(int ud, SortedDictionary<mao_key, num> m0, SortedDictionary<mao_key, num> m1)
        {
            data[ud].Clear();
            foreach (KeyValuePair<mao_key, num> d in m0) muladd(ud, m1, d);
        }
        void set(many_as_one fr)
        {
            for (int i = 0; i < 2; i++)
            {
                data[i].Clear();
                foreach (KeyValuePair<mao_key, num> d in fr.data[i]) data[i].Add(new mao_key(d.Key), new num(d.Value));
            }
        }
        public many_as_one(many m, mao_dict d)
        {
            dict = d;
            _data_i();
            foreach (one o in m.data[0]) add(0, fr_one(o));
            foreach (one o in m.data[1]) add(1, fr_one(o));
        }
        public many to_many(ids h, int m)
        {
            many ret = new many(h,m); ret.data[1].RemoveAt(0);
            foreach (KeyValuePair<mao_key, num> d in data[0]) ret.data[0].Add(to_one(d,ret));
            foreach (KeyValuePair<mao_key, num> d in data[1]) ret.data[1].Add(to_one(d, ret));
            return ret;
        }

        public many_as_one(many_as_one _m, int _e)
        {
            dict = _m.dict;
            many_as_one tmp = new many_as_one(dict);
            many_as_one fr = new many_as_one(dict);
            num exp = dict.exps[_e], nexp = new num(0);
            int i0,_eu = (int)(exp.get_up());
            fr.set(_m);
            if (exp.get_down() > 1) {
                if ((fr.data[0].Count > 1) || (fr.data[1].Count > 1)) return;
                if (! fr.data[0].ToArray()[0].Value.root((int)exp.get_down())) return;
                if (! fr.data[1].ToArray()[0].Value.root((int)exp.get_down())) return;
                for (i0 = 0; i0 < dict.nvars; i0++) 
                {
                    nexp.set(dict.exps[fr.data[0].ToArray()[0].Key.key[i0]]);
                    nexp.mul(new num(1,new BigInteger(1),exp.get_down()));
                    fr.data[0].ToArray()[0].Key.key[i0] = dict.exp(nexp);
                }
            }
            _data_i();
            data[0].Add(new mao_key(dict), new num(1));
            data[1].Add(new mao_key(dict), new num(1));
            switch (_eu)
            {
                case 0: return; 
                case 1:
                    set(fr);
                    break;
                case 2:
                    mul(0,fr.data[0],fr.data[0]); mul(1,fr.data[1],fr.data[1]);
                    break;
                default:
                    if ((_eu & 1) == 0)
                    {
                        mul(0, fr.data[0], fr.data[0]); mul(1, fr.data[1], fr.data[1]);
                    }
                    else set(fr);
                    for (i0 = (_eu >> 1); i0 > 0; i0--) {
                        for (int i = 0; i < 2; i++)
                        {
                            tmp.mul(i, data[i], fr.data[i]);
                            mul(i, tmp.data[i], fr.data[i]);
                        }
                    }
                    break;
            }
            if (exp.get_sign() < 0) {tmp.data[0] = data[0]; data[0] = data[1]; data[1] = tmp.data[0];}
        }
        public bool expand(int n, many_as_one e, int id)
        {
            bool ret = false;
            int ex = dict.var(id),ee; byte tex;
            num max_u = new num(0), max_d = new num(0), now_u = new num(0), now_d = new num(0);
            many_as_one[] me = new many_as_one[254], ae = new many_as_one[254];
            mao_key z = new mao_key(dict);
            KeyValuePair<mao_key, num> tu = new KeyValuePair<mao_key,num>(new mao_key(dict), new num(0));
            me[0] = new many_as_one(e,0);
            me[1] = new many_as_one(e,1);
            if ((e.data[0].Count < 1) || (e.data[1].Count < 1)) dict.fio.error("wrong");
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
                ae[tex].add(0,tu);
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
                        for (int tex0 = 0; tex0 < 254; tex0++) if ((ae[tex0] != null) && (tex0 != tex)) ae[tex0].mul(0,me[tex].data[1]);
                        mul(1-n,me[tex].data[1]);
                    }
                    ee = dict.exp(now_u);
                    if (me[ee] == null) me[ee] = new many_as_one(e,ee);
                    ae[tex].mul(0,me[ee].data[1]);
                    ee = dict.exp(now_d);
                    if (me[ee] == null) me[ee] = new many_as_one(e,ee);
                    ae[tex].mul(0,me[ee].data[0]);
                }

            }
            ee = dict.exp(now_u);
            if (me[ee] == null) me[ee] = new many_as_one(e,ee);
            mul(1-n,me[ee].data[1]);
            ee = dict.exp(now_d);
            if (me[ee] == null) me[ee] = new many_as_one(e,ee);
            mul(1-n,me[ee].data[0]);
            data[n].Clear();
            for (tex = 0; tex < 254; tex++) 
            {
                if (ae[tex] != null) {
                  me[tex].mul(0,ae[tex].data[0]);
                  add(n,me[tex].data[0]);
                }
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




    class fileio
    {
        StreamReader fin;
        StreamWriter fout;
        parse head;
        int nline;
        public Boolean has, err;
        public fileio(string nin, string nout, parse h)
        {
            fin = new StreamReader(nin);
            fout = new StreamWriter(nout);
            nline = 0; has = true; err = false;
            head = h;
        }
        public string rline()
        {
            string r;
            nline++;
            r = fin.ReadLine();
            if (r == null) { has = false; return ""; }
            else { has = true; return r; }
        }
        public void close()
        {
            fin.Close();
            fout.Close();
        }
        public void error(string e)
        {
            fout.WriteLine(head.val);
            fout.WriteLine("Line {0:G} Pos {0:G}: " + e, nline, head.pos);
            err = true; fout.Flush();
            Environment.Exit(-1);
        }
        public void wline(string s)
        {
            fout.WriteLine(s);
            fout.Flush();
        }
    }
    class parse
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
        public bool isnum(string s, int i)
        {
            if (s.Length <= i) return false;
            return ((s[i] >= '0') && (s[i] <= '9'));
        }
        public bool isnum(string s)
        {
            if (s.Length < 1) return false;
            for (int i = 0; i < s.Length; i++) if (!isnum(s, i)) return false;
            return true;
        }
        public bool next()
        {
            string s0,s1,st,sf;
            int i0,i1,i2,i3,i4,i5,i6, deep;
            val = sys.rline(); val = val.Replace(" ",""); pos = 0;
            if ((val.Length == 0) || (val[0] == '`')) return false;
            name = snext(false);
            if (now() == '#')
            {
                for (i0 = 0; i0 < m_name.Count; i0++) if (m_name[i0].IndexOf("#" + name + "(") > -1) sys.error("macro: name intersect");
                if (!isnum(val, pos+1)) sys.error("macro: wrong num");
                int _np = (int)(val[pos+1] - '0'); string _m = val.Substring(pos + 2);
                m_name.Add("#" + name + "("); m_nparm.Add(_np); macro.Add(_m);
                for (i0 = 0; i0 < 10; i0++)
                {
                    if (_m.IndexOf("#" + ((char)(i0 + '0')).ToString()) > -1)
                    {
                        if (i0 >= _np) sys.error("macro: used nonparm");
                    }
                    else
                    {
                        if (i0 < _np) sys.error("macro: nonused parm");
                    }
                }
                return false;
            }
            while ((i1 = val.LastIndexOf("#")) > -1) {
                for (i0 = 0; i0 < macro.Count; i0++) if (i1 == val.IndexOf(m_name[i0], i1)) break;
                if (i0 == macro.Count) sys.error("macro: not found");
                s0 = macro[i0];
                int ploop = -1,floop = 0,tloop = 0;
                for (i2 = 0,i3 = i1 + m_name[i0].Length; i2 < m_nparm[i0]; i2++)
                {
                    i4 = val.IndexOf(":",i3); i5 = val.IndexOf(",",i3); i6 = val.IndexOf(")",i3);
                    if ((i5 < 0) || ((i5 > i6) && (i6 > -1))) i5 = i6;
                    if ((i4 > -1) && (i4 < i5)) {
                        if (ploop > -1) sys.error("macro: wrong loop");
                        if (val[i3] == '(') { 
                            pos = i3 + 1;  floop = (int)(calc().get_sup()); 
                        }
                        else
                        {
                            if (!isnum(val.Substring(i3, i4 - i3))) sys.error("macro: wrong loop");
                            int.TryParse(val.Substring(i3, i4 - i3), out floop);
                        }
                        if (val[i4 + 1] == '(') { 
                            pos = i4 + 2;  tloop = (int)(calc().get_sup()); i5++;
                        }
                        else
                        {
                            if (!isnum(val.Substring(i4 + 1, i5 - i4 - 1))) sys.error("macro: wrong loop");
                            int.TryParse(val.Substring(i4 + 1, i5 - i4 - 1), out tloop);
                        }
                        ploop = i2;
                        i4 = i5;
                    } else {
                        i4 = i3; deep = 0; while (true)
                        {
                            if (i4 >= val.Length) sys.error("macro: call wrong");
                            if (val[i4] == '(') deep++;
                            if (val[i4] == ')') { deep--; if (deep < 0) break; }
                            if ((val[i4] == ',') && (deep == 0)) break;
                            i4++;
                         }
                         s1 = val.Substring(i3,i4-i3);
                         s0 = s0.Replace("#" + ((char)(i2 + '0')).ToString(), s1);
                    }
                    if (((i2 == m_nparm[i0] - 1) && (val[i4] != ')')) || ((i2 < m_nparm[i0] - 1) && (val[i4] != ','))) sys.error("macro: call nparm");
                    i3 = i4 + 1;
                }
                sf = val.Substring(0, i1); st = (i3 < val.Length ? val.Substring(i3, val.Length - i3) : "");
                if (ploop < 0) val = sf + s0 + st;
                else
                {
                    for (s1 = "", i3 = floop; i3 <= tloop; i3++) s1 += s0.Replace("#" + ((char)(ploop + '0')).ToString(), i3.ToString().Trim());
                    val = sf + s1 + st;
                }
            }
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
            if (delim.IndexOf(now()) > -1) pos++;
            else while (more() && (delim.IndexOf(now()) == -1)) pos++;
            return val.Substring(i0, pos - i0);
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

    }

    static class Program
    {
        static Form1 m0;
        static parse par;
        static void parseone(parse par, ids root, int i, one data)
        {
          string s;
          int var = -1;
          bool div = false;
          num tn;
          if (par.now() == '+') par.pos++; else if (par.now() == '-') {par.pos++; data.mult.neg();}
          while (true)
          {
              s = par.snext(false); if (s.Length < 1) return;
              if (par.delim.IndexOf(s[0]) > -1)
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
                          if (var < 0) par.sys.error("wrong exp");
                          tn = ((par.now() == '(') ? par.calc0() : par.nnext(false));
                          if (div) data.exps[var].sub(tn); else data.exps[var].add(tn);
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
                      var = -1;
                  }
                  else
                  {
                      if ((var = root.find(s)) < 0) par.sys.error("var not found");
                      if (par.now() != '^') data.exps[var].add(new num(div ? -1 : 1));
                  }
              }
          }
        }


        [STAThread]
        static int Main(string[] args)
        {
            int sx=0, sy=0;
            if (args.Length < 2) return 0;
            par = new parse(args[0], args[1], "#&!@$+-=*/^(),~:");
            par.next();
            sx = (int)par.nnext(true).get_up();
            sy = (int)par.nnext(true).get_up();
            if ((sx < 100) || (sy < 100)) return -1;
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            m0 = new Form1(sx, sy);
            Thread calc = new Thread(doit);
            calc.Start();
            Application.Run(m0);

            return 0;
        }
        static void doit() {
            int i,i0,i1;
            string val;
            int x0,x1,f0,f1,c0,c1;
            int[] xid = new int[6];
            int _r0,_r1,_r2; double _d0,_d1,x2;
            BigInteger prec;
            par.next(); ids root = new ids((int)par.nnext(true).get_up(), par.sys);
            while (par.sys.has)
            {
                if (par.next()) 
                {
                    switch (par.now()) 
                    {
                     case '=':
                        par.snext(false);
                        if (root.find(par.name) > -1) par.sys.error("double name");
                        i = root.set_empty(par.name);
                        if (i < 0) par.sys.error("too many");
                        int nowdiv = 0;
                        if (par.more()) root.values[i] = new many(root,i);
                        switch (par.now())
                        {
                            case '/':
                                par.snext(false); root.values[i].tfunc = 0;
                                if (par.delim.IndexOf(par.now()) == -1) root.values[i].pfunc = par.nnext(false);
                                break;
                            case '^':
                                root.values[i].tfunc = 1; root.values[i].pfunc.set(root.find(par.snext(true)));
                                if (root.values[i].pfunc.get_sup() < 0) par.sys.error("row: no var");
                                break;
                        }
                        while (par.more())
                        {
                            root.values[i].data[nowdiv].Add(new one(root.values[i], new num(1)));
                            parseone(par, root,i,root.values[i].data[nowdiv][root.values[i].data[nowdiv].Count-1]);
                            if (par.now() == '/')
                            {
                                if ((par.more()) && (nowdiv == 0))
                                {
                                    nowdiv = 1; root.values[i].data[1].RemoveAt(0);  par.pos++; 
                                } else par.sys.error("wrong");
                            }
                        }
                        if (root.values[i] != null) 
                        {
                            if (root.values[i].tfunc == 1) 
                                root.values[i].revert_mult(0); 
                            else 
                                root.values[i].simple();
                            par.sys.wline(root.names[i] + " = " + root.values[i].print()); 
                        }
                     break;
                     case '$':
                        if ((i=root.find(par.name)) < 0) par.sys.error("no name");
                        if (root.values[i] == null) par.sys.error("empty name");
                        {
                        many v = root.values[i];
                        List<int> _id = new List<int>();
                        bool _div = false;
                        while (par.more())
                        {
                            val = par.snext(true); if (val.Length < 1) break;
                            if ((i0 = root.find(val)) < 0) par.sys.error("no name");
                            if (i0 == i) par.sys.error("recursion - look recursion");
                            v.revert(i0);
                            _id.Add(i0);
                            if (par.now() == '$') _div=true;
                        }
                        for (int _i=0; _i < _id.Count; _i++) v.expand(_id[_i]);
                        if (_div && (v.data[1].Count == 1))
                        {
                            v.data[1][0].div();
                            many.mul(v.data[0], v.data[1][0]);
                            v.data[1][0] = new one(v, 1);
                            v.simple();
                        }
                        par.sys.wline(root.names[i] + " = " + v.print());
                        }
                        break;
                     case '!':
                        if ((i=root.find(par.name)) < 0) par.sys.error("no name");
                        if (root.values[i] == null) par.sys.error("empty name");
                        root.values[i].revert(i);
                        {
                        mao_dict mdict = new mao_dict((int)par.nnext(true).get_up(),root.size,par.sys);
                        many_as_one[] mao_fr = new many_as_one[root.size];
                        List<int> _id = new List<int>();
                        bool _div = false;
                        while (par.more())
                        {
                            val = par.snext(true); if (val.Length == 0) break; 
                            if ((i0 = root.find(val)) < 0) par.sys.error("no name");
                            if (i0 == i) par.sys.error("recursion - look recursion");
                            if (root.values[i0] != null) {
                                root.values[i].revert(i0);
                                _id.Add(i0);
                                mao_fr[i0] = new many_as_one(root.values[i0],mdict);
                            }
                            if (par.now() == '$') _div = true;
                        }
                        many_as_one mao_to = new many_as_one(root.values[i],mdict);
                        bool r = true;
                        while (r)
                        {
                            r = false;
                            for (int _i = 0; _i < _id.Count; _i++)
                            {
                                r = mao_to.expand(mao_fr[_id[_i]], _id[_i]) || r;
                            }
                            
                        }
                        root.values[i] = mao_to.to_many(root,i);
                        if (_div && (root.values[i].data[1].Count == 1))
                        {
                            root.values[i].data[1][0].div();
                            many.mul(root.values[i].data[0], root.values[i].data[1][0]);
                            root.values[i].data[1][0] = new one(root.values[i], 1);
                            root.values[i].simple();
                        }

                        par.sys.wline(root.names[i] + " = " + root.values[i].print());
                        }                      
                        break;
                     case '@':
                        i0 = 0; val = "";
                        if ((i = root.find(par.name)) < 0) par.sys.error("no name");
                        if ((!par.more()) || ((i0 = root.find(val = par.snext(true))) < 0)) par.sys.error("must be defined many");
                        if ((par.now() != '=') || (!par.more()) || ((val = par.snext(true)).Length < 1)) par.sys.error("must be equate");
                        if (i0 == i) par.sys.error("recursion - look recursion");
                        {
                            string nn;
                            int ip = root.last;
                            one _dv,_ml;
                            if (root.values[i] == null)
                            {
                                _ml = new one(root.values[i0], 1);
                                _ml.exps[i].one();
                            }
                            else
                            {
                                if ((root.values[i].data[0].Count != 1) || (root.values[i].data[1].Count != 1)) par.sys.error("one only");
                                _ml = new one(root.values[i].data[0][0]);
                            }
                            _dv = new one(_ml);
                            _dv.div();
                            List<one> dv = new List<one>();

                            one odv = new one(root.values[i0], new num(par.isnum(val) ? val : "1"));
                            if (!par.isnum(val))
                            {
                                if ((i1 = root.find(val)) < 0) par.sys.error("no name");
                                odv.exps[i1].one();
                            }
                            odv.mult.neg();
                            many.add(dv, root.values[i0].data[1]);
                            many.mul(dv, odv);
                            many.add(dv, root.values[i0].data[0]);
                            num e = new num();
                            for (i0 = 0; i0 < dv.Count; i0++)
                            {
                                e.zero();
                                if (dv[i0].mul(_ml)) {
                                    do { e.add(new num(-1));
                                    } while (dv[i0].mul(_ml));
                                    dv[i0].mul(_dv);
                                } else {
                                    dv[i0].mul(_dv);
                                    while (dv[i0].mul(_dv)) e.add(new num(1));
                                    dv[i0].mul(_ml);
                                }
                                nn = root.names[i] + (e.nonzero() ? ((e.get_sign() < 0 ? "_" : "") + e.get_up().ToString().Trim() + (e.get_down() > 1 ? ("_" + e.get_down().ToString().Trim()) : "")) : "0");
                                if ((i1 = root.find(nn)) < 0) { 
                                    i1 = root.set_empty(nn);
                                    if (i1 < 0) par.sys.error("too many");
                                    root.values[i1] = new many(root,i1);
                                }
                                e.zero(); root.values[i1].data[0].Add(new one(dv[i0])); many.simple(root.values[i1].data[0]);
                            }
                            for (i0 = ip; i0 < root.last; i0++) par.sys.wline(root.names[i0] + " = " + root.values[i0].print());
                        }
                     break;
                     case '~':
                        prec = par.nnext(true).get_up();
                        f0 = (int)(par.nnext(true).get_up());
                        f1 = (int)(par.nnext(true).get_up());
                        c0 = (int)(par.nnext(true).get_up());
                        c1 = (int)(par.nnext(true).get_up());
                        if ((f0 + c0 > m0.sx) || (f1 + c1 > m0.sy) || (c0 < 2) || (c1 < 2)) par.sys.error("wrong size");
                        i = 0; i0 = -1;
                        while (par.more() && (i < 6))
                        {
                            val = par.snext(true); if (val.Length < 1) break;
                            if ((xid[i] = root.find(val)) < 0) par.sys.error("no name");
                            if (root.values[xid[i]] == null) i0 = i;
                            i++;
                        }
                        m0.rp = false;
                        if ((i0 == 0) && (i == 3)) for (x0 = 0; x0 < c0 * 6; x0++)
                            {
                                root.uncalc();
                                root.calc[xid[0]].set(x0);
                                root.values[xid[1]].calc(prec);
                                _r0 = (int)root.calc[xid[1]].toint();
                                root.values[xid[2]].calc(prec);
                                _r1 = (int)root.calc[xid[2]].toint();
                                if (_r0 < 0) _r0 = 0; if (_r1 < 0) _r1 = 0;
                                m0.bm.SetPixel((int)(_r0 % c0) + f0, (int)(_r1 % c1) + f1, Color.FromArgb(255, 255, 255));
                            }
                        else if (i0 == 1) {
                            if (i == 4)  for (x0 = 22; x0 < c0-22; x0 += 11)
                            {
                                for (x1 = 22; x1 < c1-22; x1 += 11)
                                {
                                    root.uncalc();
                                    root.calc[xid[0]].set(x0);
                                    root.calc[xid[1]].set(x1);
                                    root.values[xid[2]].calc(prec);
                                    _d0 = root.calc[xid[2]].todouble();
                                    root.values[xid[3]].calc(prec);
                                    _d1 = root.calc[xid[3]].todouble();
                                    for (x2 = 0; x2 < 10; x2++) m0.bm.SetPixel(f0 + x0 + (int)(_d0 * x2), f1 + x1 + (int)(_d1 * x2), Color.FromArgb(255,2,2));
                                    m0.bm.SetPixel(f0 + x0, f1 + x1, Color.FromArgb(255,255,255));
                                }
                                m0.Set(0);
                            }
                            else for (x0 = 0; x0 < c0; x0++)
                        {
                            for (x1 = 0; x1 < c1; x1++)
                            {
                                root.uncalc();
                                root.calc[xid[0]].set(x0);
                                root.calc[xid[1]].set(x1);
                                root.values[xid[2]].calc(prec);
                                _r0 = (int)root.calc[xid[2]].toint(); _r1 = _r0; _r2 = _r0;
                                if (i > 3)
                                {
                                    root.values[xid[3]].calc(prec);
                                    _r1 = (int)root.calc[xid[3]].toint(); _r2 = 0;
                                    if (i == 5)
                                    {
                                        root.values[xid[4]].calc(prec);
                                        _r2 = (int)root.calc[xid[4]].toint();
                                    }
                                }
                                if (_r0 < 0) _r0 = 0; if (_r1 < 0) _r1 = 0; if (_r2 < 0) _r2 = 0;
                                if (_r0 > 255) _r0 = 255; if (_r1 > 255) _r1 = 255; if (_r2 > 255) _r2 = 255;
                                m0.bm.SetPixel(f0 + x0, f1 + x1, Color.FromArgb(_r0,_r2,_r1));
                            }
                            m0.Set(0);
                        }
                        }
                        m0.Set(0);
                        m0.rp = true;
                     break;
                     case '&':
                        prec = par.nnext(true).get_up();
                        BigInteger _fr,_to,_one = 1,_res;
                        _fr = par.nnext(true).get_up();
                        _to = par.nnext(true).get_up();
                        i = 0; while (par.more() && (i < 2))
                        {
                            val = par.snext(true); if (val.Length < 1) break;
                            if ((xid[i] = root.find(val)) < 0) par.sys.error("no name");
                            i++;
                        }
                        if ((root.values[xid[0]] != null) || (root.values[xid[1]] == null)) par.sys.error("wrong");
                        while (_fr <= _to)
                        {
                            root.uncalc();
                            root.calc[xid[0]].set(new num(1,_fr,_one));
                            root.values[xid[1]].calc(prec);
                            _res = root.calc[xid[1]].toint();
                            par.sys.wline(_fr.ToString() + " & " + _res.ToString() + " : " + root.calc[xid[1]].print("+","-","",""));
                            _fr++;
                        }
                        break;
                    }
                }
            }
            par.sys.close();
        }
    }
}
