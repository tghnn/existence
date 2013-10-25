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
        public BigInteger get_down() { return down;}
        public num()
        {
            init(0, 0); exs = false;
        }
        public num(num n)
        {
            set(n);
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
        public num(int _s, BigInteger _u, BigInteger _d)
        {
            set(_s,_u,_d);
        }
        public void set(int _s, BigInteger _u, BigInteger _d)
        {
            sign = _s;
            up = _u;
            down = _d;
            exs = true;
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
        public void mul(num a)
        {
            sign *= a.get_sign();
            up *= a.get_up();
            down *= a.get_down();
        }
        public void revert()
        {
            BigInteger t = up;
            up = down; down = t;
        }
        public void exp(int ex)
        {
            if ((ex & 1) == 0) sign = 1;
            if (ex == 0) { up = 1; down = 1; }
            else
            {
                BigInteger u0 = up, d0 = down;
                for (int i = 1; i < ex; i++) { up *= u0; down *= d0; }
            }
        }
        public void div(num a)
        {
            sign *= a.get_sign();
            up *= a.get_down();
            down *= a.get_up();
        }
        public void div()
        {
            BigInteger t;
            t = up; up = down; down = t;
        }
        public void add_up(BigInteger a)
        {
            up += a;
        }
        public void add(num a)
        {
            if (sign == 0) set(a); else {
            if (sign * a.get_sign() < 0)
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
            if (mult.get_up() == 0) return false; mult.div();
            for (i0 = 0; i0 < head.head.size; i0++) exps[i0].neg();
            return true;
        }

    }
    class many
    {
        public ids head;
        public List<one>[] data;
        public int id;
        public many(ids h, int i)
        {
            head = h; id = i;
            data = new List<one>[2];
            data[0] = new List<one>();
            data[1] = new List<one>();
            data[1].Add(new one(this,1));
        }
        public many(many m)
        {
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
        public bool revert(int ud, int _id) //_id^(-x) -> /_id^(x)
        {
            bool ret = false;
            num _exp, _max = new num(0);
            foreach (one u in data[ud]) 
            {
                _exp = new num(u.exps[_id]); 
                if ((_exp.get_up() != 0) && (_exp.get_sign() < 0)) 
                    {
                        if (_exp.great(_max)) _max.set(_exp);
                        ret = true;
                    }
            }
            if (!ret || (_max.get_up() == 0)) return ret;
            _max.neg();
            foreach (one u in data[ud])  u.exps[_id].add(_max);
            foreach (one d in data[1-ud])  d.exps[_id].add(_max);
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
                                    if (u.exps[j].get_up() > 0) u.exps[j].mul(_dex);
                                    if (d.exps[j].get_up() > 0) d.exps[j].mul(_dex);
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
            for (i = 0; i < head.size; i++) if (data[1][0].exps[i].get_up() != 0) hasdiv = true;
            s0 = (hasdiv ? "(" + print(data[0]) + ")/(" + print(data[1]) + ")" : print(data[0]));
            return s0;
        }
        static public string print(List<one> data)
        {
            int i0, i1;
            bool oneout;
            string s0;
            for (s0 = "", i0 = 0; i0 < data.Count; i0++)
            {
                if (data[i0].mult.get_up() != 0)
                {
                    oneout = false;
                    for (i1 = 0; i1 < data[i0].head.head.size; i1++)
                    {
                        if (data[i0].exps[i1].get_up() > 0)
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
                num t0 = calc(0, prec);
                num t1 = calc(1, prec);
                head.calc[id].set(t0);
                head.calc[id].div(t1);
                head.calc[id].simple();
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
                    if (ex.get_up() > 0)
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
                        t1.simple();
                        t0.mul(t1);
                    }
                }
                t0.simple();
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
                if (o.exps[i].get_up() != 0) ret.Key.key[dict.var(i)] = dict.exp(o.exps[i]);
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
            fout.WriteLine("Line {0:G} : " + e, nline);
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
        public char last;
        public bool thisnum, more, div;
        public num rnum;
        List<string> m_name, macro;
        List<int> m_nparm;
        public fileio sys;
        public parse(string nin, string nout, string d)
        {
            sys = new fileio(nin,nout,this); 
            delim = d; val = ""; pos = 0; rnum = new num(1); reset();
            m_name = new List<string>(); macro = new List<string>(); m_nparm = new List<int>();
        }
        public bool isnum(string s, int i)
        {
            if (s.Length <= i) return false;
            return ((s[i] >= '0') && (s[i] <= '9'));
        }
        public bool isnum(string s)
        {
            return isnum(s, 0);
        }
        public void reset()
        {
            div = false; rnum.one();
        }
        public bool next()
        {
            string s0,s1;
            int i0,i1,i2,i3,i4, deep;
            val = sys.rline(); pos = 0; more = true;
            if ((val.Length == 0) || (val[0] == '`')) return false;
            name = get();
            if (thisnum) return false;
            if (last == '#')
            {
                for (i0 = 0; i0 < m_name.Count; i0++) if ((name.IndexOf(m_name[i0]) > -1) || (m_name[i0].IndexOf(name) > -1)) sys.error("macro: name intersect");
                if (!isnum(val, pos)) sys.error("macro: wrong num");
                int _np = (int)(val[pos] - '0'); string _m = val.Substring(pos + 1);
                m_name.Add(name + "("); m_nparm.Add(_np); macro.Add(_m);
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
            for (i0 = 0; i0 < macro.Count; i0++)
            {
                while ((i1 = val.IndexOf(m_name[i0])) > -1)
                {
                    s0 = macro[i0];
                    for (i2 = 0,i3 = i1 + m_name[i0].Length; i2 < m_nparm[i0]; i2++)
                    {
                        i4 = i3; deep = 0; while (true)
                        {
                            if (i4 >= val.Length) sys.error("macro: call wrong");
                            if (val[i4] == '(') deep++;
                            if (val[i4] == ')') { deep--; if (deep < 0) break; }
                            if ((val[i4] == ',') && (deep == 0)) break;
                            i4++;
                        }
                        if (((i2 == m_nparm[i0] - 1) && (deep == 0)) || ((i2 < m_nparm[i0] - 1) && (deep < 0))) sys.error("macro: call nparm");
                        s1 = val.Substring(i3,i4-i3);
                        s0 = s0.Replace("#" + ((char)(i2 + '0')).ToString(), s1);
                        i3 = i4 + 1;
                    }
                    val = val.Substring(0, i1) + s0 + (i3 < val.Length ? val.Substring(i3, val.Length - i3): "");
                }
            }
            pos = 0; rnum.one(); name = get();
            return true;
        }

        public string get()
        {
            string r = "";
            char p;
            int start = pos;
            BigInteger b;
            bool data = false;
            if (pos >= val.Length) { more = false; return ""; }
            last = '\0';  while (true)
            {
                if (pos == val.Length) { last = '\n'; more = false; r = val.Substring(start); break; }
                if ((p = val[pos]) != ' ')
                {
                    if (delim.IndexOf(p) > -1)
                    {
                        if (data)
                        {
                            last = p; r = val.Substring(start, pos - start);
                            switch (p)
                            {
                                case '+': 
                                case '-': 
                                case '*': 
                                case '/': 
                                    break;
                                default:
                                    pos++; break;
                            }
                        } else {
                            switch (p)
                            {
                                case '+': rnum.set_sign(1); break;
                                case '-': rnum.set_sign(-1); break;
                                case '*': div = false; break;
                                case '/': div = true; break;
                                default:
                                    last = p; pos++; break;
                            }
                        }
                    } else if (!data) { data = true; start = pos; }
                }
                if (last != '\0') { more = (pos < val.Length); break; }
                pos++;
            }
            r = r.Replace(" ", "");
            thisnum = isnum(r);
            if (thisnum)
            {
                BigInteger.TryParse(r, out b);
                if (div)
                {
                    if (b == 0) sys.error("/0");
                    rnum.mul(new num(1,new BigInteger(1),b)); 
                } else {
                    rnum.mul(new num(1,b,new BigInteger(1)));
                }
            }
            return r;
        }

    }

    static class Program
    {
        static bool parseone(parse par, ids root, int i, List<one> data, bool hasone)
        {
          int fids;
          string val;
          val = par.get();
          if (val.Length > 0)
          {
              if (par.thisnum)
              {
                  if ((par.last == '+') || (par.last == '-') || (par.last == ')') || (par.last == '\n'))
                  {
                      if (hasone && (data.Count < 1)) par.sys.error("AZAZA");
                      if (!hasone) data.Add(new one(root.values[i], par.rnum));
                      else data[data.Count - 1].mult.mul(par.rnum);
                      hasone = false;  par.reset();
                  }
              } else {
                  if ((fids = root.find(val)) < 0) par.sys.error("no name");
                  if (hasone && (data.Count < 1)) par.sys.error("AZAZA");
                  if (!hasone) data.Add(new one(root.values[i], par.rnum));
                  else data[data.Count - 1].mult.mul(par.rnum);
                  if (par.div) {par.reset(); par.rnum.neg();} else par.reset();
                  if (par.last == '^')
                  {
                      val = par.get();
                      if (par.last == '(') { while (par.last != ')') val = par.get(); par.last = ' ';}
                  }
                  if ((data.Count < 1)) par.sys.error("AZAZA");
                  data[data.Count - 1].exps[fids].add(par.rnum);
                  par.reset();
                  if ((par.last == '+') || (par.last == '-') || (par.last == ')') || (par.last == '\n')) hasone = false; else hasone = true;
              }
          }
          return hasone;
        }

        static Form1 m0;
        static parse par;

        [STAThread]
        static int Main(string[] args)
        {
            int sx=0, sy=0;
            if (args.Length < 2) return 0;
            par = new parse(args[0], args[1], "#!@$+-=*/^(),~");
            par.next(); par.get(); sx = (int)par.rnum.get_up();
            par.rnum.one(); par.get(); sy = (int)par.rnum.get_up();
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
            par.next(); par.rnum.one(); par.get();
            ids root = new ids((int)par.rnum.get_up(),par.sys);
            while (par.sys.has)
            {
                if (par.next()) 
                {
                    if (par.thisnum) par.sys.error("num name");
                    switch (par.last) 
                    {
                     case '=':
                        if (root.find(par.name) > -1) par.sys.error("double name");
                        i = root.set_empty(par.name);
                        if (i < 0) par.sys.error("too many");
                        bool hasone,nowdiv;
                        string val;
                        if (par.more) root.values[i] = new many(root,i);
                        hasone = false; nowdiv = false; par.reset();
                        while (par.more)
                        {
                            hasone = parseone(par, root,i,(nowdiv ? root.values[i].data[1] : root.values[i].data[0]), hasone);
                            if (par.last == ')')
                            {
                                if (par.more)
                                {
                                    if ((par.val[par.pos] != '/') || (par.val[par.pos+1] != '(') || (nowdiv)) par.sys.error("wrong");
                                    nowdiv = true; root.values[i].data[1].RemoveAt(0);  par.pos += 2; par.reset();
                                }
                            }
                        }
                        if (root.values[i] != null) { root.values[i].simple(); par.sys.wline(root.names[i] + " = " + root.values[i].print()); }

                     break;
                     case '$':
                        if ((i=root.find(par.name)) < 0) par.sys.error("no name");
                        if (root.values[i] == null) par.sys.error("empty name");
                        {
                        many v = root.values[i];
                        List<int> _id = new List<int>();
                        bool _div=false;
                        while (par.more)
                        {
                            val = par.get(); if (val.Length == 0) break;
                            if ((i0 = root.find(val)) < 0) par.sys.error("no name");
                            if (i0 == i) par.sys.error("recursion - look recursion");
                            v.revert(i0);
                            _id.Add(i0);
                            if (par.last == '$') _div=true;
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
                        par.get();
                        mao_dict mdict = new mao_dict((int)par.rnum.get_up(),root.size,par.sys);
                        many_as_one[] mao_fr = new many_as_one[root.size];
                        List<int> _id = new List<int>();
                        bool _div = false;
                        while (par.more)
                        {
                            val = par.get(); if (val.Length == 0) break;
                            if ((i0 = root.find(val)) < 0) par.sys.error("no name");
                            if (i0 == i) par.sys.error("recursion - look recursion");
                            if (root.values[i0] != null) {
                                root.values[i].revert(i0);
                                _id.Add(i0);
                                mao_fr[i0] = new many_as_one(root.values[i0],mdict);
                            }
                            if (par.last == '$') _div = true;
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
                        if ((!par.more) || ((i0 = root.find(val = par.get())) < 0)) par.sys.error("must be defined many");
                        if ((par.last != '=') || (!par.more) || ((val = par.get()).Length < 1)) par.sys.error("must be equate");
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
                            one odv = new one(root.values[i0], (par.thisnum ? par.rnum : new num(1)));
                            if (!par.thisnum)
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
                                nn = root.names[i] + (e.get_up() != 0 ? ((e.get_sign() < 0 ? "_" : "") + e.get_up().ToString().Trim() + (e.get_down() > 1 ? ("_" + e.get_down().ToString().Trim()) : "")) : "0");
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
                        int x0,x1,f0,f1,c0,c1;
                        int[] xid = new int[6];
                        int _r0,_r1,_r2; double _d0,_d1,x2;
                        par.get(); BigInteger prec = par.rnum.get_up();
                        par.rnum.one(); par.get(); f0 = (int)(par.rnum.get_up());
                        par.rnum.one(); par.get(); f1 = (int)(par.rnum.get_up());
                        par.rnum.one(); par.get(); c0 = (int)(par.rnum.get_up());
                        par.rnum.one(); par.get(); c1 = (int)(par.rnum.get_up());
                        if ((f0 + c0 > m0.sx) || (f1 + c1 > m0.sy) || (c0 < 2) || (c1 < 2)) par.sys.error("wrong size");
                        i = 0; i0 = -1;
                        while (par.more && (i < 6))
                        {
                            val = par.get(); if (val.Length == 0) break;
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
                    }
                }
            }
            par.sys.close();
        }
    }
}
