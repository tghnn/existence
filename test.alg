xy=600,600
m=40
`screen xy, max variable number
sqr#1#0^(1/2)
`macro, 1 parm
sum#2#0+#1
`macro, 2 parms
`sum(sqr(a),4)
a=
b=
c=
d=
`empty variable
t1= 2*a+b-3*c+d
t2= 6*d+c
t3= t1^3/t2^4
t4= t3^11 + t3^-6
t4!11,t3,t2,t1
`fast expand at t4, max vars in = 11
`t4$t3,t2,t1
`slow expand (but faster on small expression)

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
a~1000000,xa,ya,br,cr
`graph (xa,ya) -> (br,cr)   input vars must be empty
t=
tt=t/600/3-3
xt=t/6
ytt=tt^6 + 2*tt^5 + tt^4 + 6*tt^3 + tt^2
yt=300+ytt*4
a~10000,t,xt,yt
`graph (t) -> (xt,yt)
