using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;

namespace alg
{
    class ids
    {
        public int size, last;
        public string[] names;
        public many[] values;
        public ids(int s)
        {
            size = s;
            last = 0;
            names = new string[s];
            values = new many[s];
        }
        public int set_empty(string nam)
        {
            if (last >= size) return -1;
            names[last] = nam;
            values[last] = null;
            last++; return last - 1;
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
        public BigInteger up, down;
        public int sign;
        public num(num n)
        {
            sign = n.sign;
            up = BigInteger.Abs(n.up);
            down = BigInteger.Abs(n.down);
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
        void setsign()
        {
            if (up < 0) { sign = -1; up = -up; } else sign = 1;
        }
        void init(int a, int b)
        {
            up = BigInteger.Abs(a);
            down = BigInteger.Abs(b);
            sign = (a < 0 ? -1 : 1);
            simple();
        }
        public void neg()
        {
            sign *= -1;
        }
        public bool great(num a)
        {
            if (sign == a.sign)
            {
                return (a.up*down > up*a.down);
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
            if (up == 0) { sign = 1; down = 1; } else do
            {
                a = BigInteger.GreatestCommonDivisor(up, down);
                up = BigInteger.Divide(up, a);
                down = BigInteger.Divide(down, a);
            } while (a != 1);
        }
        public void mul(num a)
        {
            sign *= a.sign;
            up *= a.up;
            down *= a.down;
            simple();
        }
        public void div(num a)
        {
            sign *= a.sign;
            up *= a.down;
            down *= a.up;
            simple();
        }
        public void add(num a)
        {
            if (sign * a.sign < 0)
            {
                up = up * a.down - a.up * down;
            }
            else
            {
                up = up * a.down + a.up * down;
            }
            if (up < 0) { up = -up; sign = -sign; }
            down *= a.down;
            simple();
        }
        BigInteger _sq(BigInteger a, int b)
        {
            BigInteger r0,r1;
            r1 = BigInteger.Pow((BigInteger)(b), (int)(BigInteger.Log(a, (double)b) / b + 0.5)); r0 = 0;
            while (r0 != r1) { r0 = r1; r1 = r0 - (BigInteger.Pow(r0, b) - a) / BigInteger.Pow(r0, b-1) / b; }
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
        public string print(string pos, string neg, string bord)
        {
            string s0;
            s0 = "";
            if (down > 1) s0 += bord.Substring(0,1);
            s0 += (sign < 0 ? neg : pos);
            s0 += up.ToString();
            if (down > 1) s0 += "/" + down.ToString() + bord.Substring(1, 1);
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
            int i;
            init(o.head);
            mult = new num(o.mult);
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
            if (head.head != a.head.head) return false;
            mult.mul(a.mult);
            for (i = 0; i < head.head.size; i++) exps[i].add(a.exps[i]);
            return true;
        }
        public bool div()
        {
            BigInteger t;
            int i0;
            if ((t = mult.up) == 0) return false; mult.up = mult.down; mult.down = t;
            for (i0 = 0; i0 < head.head.size; i0++) exps[i0].sign *= -1;
            return true;
        }

    }
    class many
    {
        public ids head;
        public List<one> up,down;
        public many(ids h)
        {
            head = h;
            up = new List<one>();
            down = new List<one>();
            down.Add(new one(this,1));
        }
        public many(many m)
        {
            int i0;
            head = m.head;
            up = new List<one>();
            down = new List<one>();
            for (i0 = 0; i0 < m.up.Count; i0++) up.Add(new one(m.up[i0]));
            for (i0 = 0; i0 < m.down.Count; i0++) down.Add(new one(m.down[i0]));
        }
        public void simple(List<one> data)
        {
            int i, j;
            for (i = 0; i < data.Count; i++)
            {
                for (j = i + 1; j < data.Count; j++)
                {
                    if (data[i].cmp(data[j]))
                    {
                        data[i].mult.add(data[j].mult);
                        data.RemoveAt(j);
                        j--;
                    }
                }
            }
        }
        public bool mul(List<one> multo, one mul0)
        {
            int i;
            if (head != mul0.head.head) return false;
            for (i = 0; i < multo.Count; i++) multo[i].mul(mul0);
            return true;
        }
        public bool muladd(List<one> addto, List<one> mul0, one mul1, int cn)
        {
            int i;
            if (head != mul1.head.head) return false;
            for (i = 0; i < cn; i++)
            {
                addto.Add(new one(mul0[i]));
                addto[addto.Count - 1].mul(mul1);
            }
            return true;
        }
        public bool muladd(List<one> addto, List<one> mul0, one mul1)
        {
            return muladd(addto,mul0,mul1,mul0.Count);
        }
        public bool add(List<one> addto, List<one> add0)
        {
            int i;
            for (i = 0; i < add0.Count; i++) addto.Add(new one(add0[i]));
            simple(addto);
            return true;
        }
        public bool mul(List<one> multo, List<one> mul0)
        {
            int i,cn;
            if (mul0.Count == 0) {
                multo.RemoveRange(0,multo.Count); return true;
            }
            for (cn = multo.Count, i = 0; i < mul0.Count; i++) muladd(multo,multo,mul0[i],cn);
            multo.RemoveRange(0,cn);
            simple(multo);
            return true;
        }
        public bool muladd(List<one> muladdto, List<one> mul0)
        {
            int i,cn;
            if (mul0.Count < 1) return true;
            cn = muladdto.Count;
            for (i = 0; i < mul0.Count; i++) muladd(muladdto, muladdto, mul0[i], cn);
            return true;
        }
        public void extract(one to, one from)
        {
            int i0;
            to.mult.up = BigInteger.GreatestCommonDivisor(to.mult.up, from.mult.up);
            to.mult.down = BigInteger.GreatestCommonDivisor(to.mult.down, from.mult.down);
            to.mult.sign = (((to.mult.sign < 0) && (from.mult.sign < 0)) ? -1 : 1);
            for (i0 = 0; i0 < head.size; i0++)
            {
                if (from.exps[i0].sign == to.exps[i0].sign)
                {
                    if (from.exps[i0].great(to.exps[i0])) to.exps[i0] = new num(from.exps[i0]);
                }
                else to.exps[i0].up = 0;
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
        public void simple()
        {
            one f_up, f_down, f_both;
            int i0;
            simple(up);
            simple(down);
            f_up = extract(up);
            f_down = extract(down);
            f_both = new one(f_up);
            extract(f_both, f_down);
            f_both.div();
            mul(up, f_both);
            mul(down, f_both);
            f_up.mult.one();
            f_down.mult.one();
            for (i0 = 0; i0 < head.size; i0++)
            {
                if (f_up.exps[i0].sign > 0) f_up.exps[i0].zero();
                if (f_down.exps[i0].sign > 0) f_down.exps[i0].zero();
            }
            f_up.div();
            mul(up, f_up);
            mul(down, f_up);
            f_down.div();
            mul(up, f_down);
            mul(down, f_down);
            simple(up);
            simple(down);
        }
        List<one> fup(num exp,many from)
        {
            return (exp.sign > 0 ? from.up : from.down);
        }
        List<one> fdw(num exp,many from)
        {
            return (exp.sign > 0 ? from.down : from.up);
        }

        public bool expand(List<one> _up, List<one> _down, int _id)
        {
            int i,j;
            num _exp;
            many id = head.values[_id];
            for (i = 0; i < _up.Count; i++)
            {
                if ((_exp = _up[i].exps[_id]).up != 0) {
                    if (_up[i].exps[_id].down == 1)
                    {
                        List<one> u, d;
                        u = new List<one>();
                        add(u, fup(_exp,id));
                        for (j = (int)_exp.up; j > 1; j--) mul(u, fup(_exp,id));
                        _up[i].exps[_id].up = 0;
                        mul(u, _up[i]);
                        _up.RemoveAt(i);

                        d = new List<one>();
                        add(d, fdw(_exp,id));
                        for (j = (int)_exp.up; j > 1; j--) mul(d, fdw(_exp,id));
                        mul(_up, d);
                        add(_up, u);
                        mul(_down, d);
                        return true;
                    }
                    else
                    {
                        if ((id.up.Count == 1) && (id.down.Count == 1))
                        {
                            one u, d, ru, rd;
                            u = new one(fup(_exp, id)[0]);
                            d = new one(fdw(_exp, id)[0]);
                            if (u.mult.root((int)_exp.down) && d.mult.root((int)_exp.down))
                            {
                                for (j = 0; j < head.size; j++)
                                {
                                    if (u.exps[j].up > 0) u.exps[j].down *= _exp.down;
                                    if (d.exps[j].up > 0) d.exps[j].down *= _exp.down;
                                }
                                ru = new one(u); rd = new one(d);
                                for (j = (int)_exp.up; j > 1; j--) ru.mul(u);
                                for (j = (int)_exp.up; j > 1; j--) rd.mul(d);
                                _up[i].exps[_id].up = 0;
                                d.div();
                                _up[i].mul(u);
                                _up[i].mul(d);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void expand(int _id)
        {
            bool f;
            if (head.values[_id] == null) return;
            f = true; while ( f )
            {
                f = expand(up,down,_id);
                if (!f) f = expand(down,up,_id);
                if (f) simple();
            }
        }
        public string print()
        {
            bool hasdiv;
            string s0;
            int i;
            hasdiv = ((down.Count > 1) || (!down[0].mult.isone()) || (down[0].mult.sign < 0));
            for (i = 0; i < head.size; i++) if (down[0].exps[i].up != 0) hasdiv = true;
            s0 = (hasdiv ? "(" + print(up) + ")/(" + print(down) + ")" : print(up));
            return s0;
        }
        public string print(List<one> data)
        {
            int i0, i1;
            bool oneout;
            string s0;
            for (s0 = "", i0 = 0; i0 < data.Count; i0++)
            {
                if (data[i0].mult.up != 0)
                {
                    oneout = false;
                    for (i1 = 0; i1 < head.size; i1++)
                    {
                        if (data[i0].exps[i1].up > 0)
                        {
                            if (!oneout) 
                            {
                                if (data[i0].mult.isone() && (data[i0].exps[i1].sign > 0))  {
                                    s0 += (i0 > 0 ? (data[i0].mult.sign < 0 ? "-" : "+") : (data[i0].mult.sign < 0 ? "-": ""));
                                } else {
                                    s0 += data[i0].mult.print((i0 > 0 ? "+" : ""), "-", "  ");
                                }
                            }
                            s0 += (data[i0].exps[i1].sign < 0 ? "/" : (oneout || (!data[i0].mult.isone()) ? "*" : "")) + head.names[i1] + (data[i0].exps[i1].isone() ? "" : "^" + data[i0].exps[i1].print("","","()"));
                            oneout = true;
                        }
                    }
                    if (!oneout) s0 += data[i0].mult.print((i0 > 0 ? "+" : ""), "-", "  ");
                }
            }
            return s0;
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
                                case '+': rnum.sign = 1; break;
                                case '-': rnum.sign = -1; break;
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
                    rnum.down *= b; 
                } else {
                    rnum.up *= b;
                }
            }
            return r;
        }

    }

    class Program
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
                      if (par.last == '(') while (par.last != ')') val = par.get();
                  }
                  if ((data.Count < 1)) par.sys.error("AZAZA");
                  data[data.Count - 1].exps[fids].add(par.rnum);
                  par.reset();
                  if ((par.last == '+') || (par.last == '-') || (par.last == ')') || (par.last == '\n')) hasone = false; else hasone = true;
              }
          }
          return hasone;
        }

        static int Main(string[] args)
        {
            int i,i0;
            parse par;
            par = new parse(args[0],args[1], "#$+-=*/^(),");
            par.next(); par.get();
            ids root = new ids((int)par.rnum.up);
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
                        bool hasone,nowdiv;
                        string val;
                        if (par.more) root.values[i] = new many(root);
                        hasone = false; nowdiv = false; par.reset();
                        while (par.more)
                        {
                            hasone = parseone(par, root,i,(nowdiv ? root.values[i].down : root.values[i].up), hasone);
                            if (par.last == ')')
                            {
                                if (par.more)
                                {
                                    if ((par.val[par.pos] != '/') || (par.val[par.pos+1] != '(') || (nowdiv)) par.sys.error("wrong");
                                    nowdiv = true; root.values[i].down.RemoveAt(0);  par.pos += 2; par.reset();
                                }
                            }
                        }
                        if (root.values[i] != null) { root.values[i].simple(); par.sys.wline(root.names[i] + " = " + root.values[i].print()); }

                     break;
                     case '$':
                        if ((i=root.find(par.name)) < 0) par.sys.error("no name");
                        if (root.values[i] == null) par.sys.error("empty name");
                        while (par.more)
                        {
                            val = par.get(); if (val.Length == 0) break;
                            if ((i0 = root.find(val)) < 0) par.sys.error("no name");
                            root.values[i].expand(i0);
                        }
                        par.sys.wline(root.names[i] + " = " + root.values[i].print());
                     break;
                    }
                }
            }
            par.sys.close();
            return 0;
        }
    }
}
