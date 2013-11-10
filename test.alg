xy=600,600
m=50,4,1000000
`screen xy
`max variable number, variable deep, precusion for noninteger exponent
##_fact#1*#0
`macro, 1 parm
##fact#1(1#_fact(2:(#0)))
`macro, loop
f=#fact(2+2)
a=
b=
c=
d=
`empty variable
x=a^(b+1)
y=2*a^(b+1)*a + x^2 + x
y$x$

t1= 2*a+b-3*c+d
t2= 6*d+c
t3= t1^3/t2^4
t4= t3^11 + t3^(-6)
t4!11,t3,t2,t1
`fast expand at t4, max vars in = 11
`t4$t3,t2,t1
`slow expand (but faster on small expression)

abc= a*b*c
abc@t4=0
`extract multiplier - may be single variable or monomial

e=
e1=/1000+e/10
`precusion control, max = 1000 decimal
exp=^e1+e1+e1^2/#fact(2)+e1^3/#fact(4)+e1^4/#fact(6)
`fast power serie on variable e 
`   use at graph and calc
exp_o=exp*1000000000
calc_exp&1,100,e,exp_o""1
`calculate exp(e), e=1:100, out to file 1 format ""
v=
v:4,3,2
dv=v+'v+''v+'''v
diff&0,10,v,dv""1

xa=
ya=
xb=100
yb=200
xc=500
yc=200
xba=xb-xa
yba=yb-ya
rba2=xba^2+yba^2
xca=xc-xa
yca=yc-ya
rca2=xca^2+yca^2
br2=xba^2+yba^2
br=250-br2^(1/4)*11
cr2=xca^2+yca^2
cr=250-cr2^2/6000000
gr=0
graph_2x2d~0,0,600,600,xa,ya,br,cr,gr
`graph(uper left(0,0), size(600,600)) (xa,ya) -> (br,cr)   input vars must be empty
t=
tt=t/600/3-3
xt=t/6
ytt=tt^6 + 2*tt^5 + tt^4 + 6*tt^3 + tt^2
yt=300+ytt*4
graph_1d~0,0,600,600,t,xt,yt
`graph (t) -> (xt,yt)
