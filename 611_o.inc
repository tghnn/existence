macro _loc _pref,[_name,_size] {
common
macro _loc_#_pref _off \{
_loc_last equ _off
forward
loc_#_pref#_#_name equ [rsp + _loc_last]
match =8, _size
\\{
loc_#_pref#_#_name equ qword [rsp + _loc_last]
\\}
match =4, _size
\\{
loc_#_pref#_#_name equ dword [rsp + _loc_last]
\\}
match =2, _size
\\{
loc_#_pref#_#_name equ word [rsp + _loc_last]
\\}
match =1, _size
\\{
loc_#_pref#_#_name equ byte [rsp + _loc_last]
\\}
_loc_last equ _loc_last + _size
common
\}
}


macro _dsc _pref,[_name,_size] {
common
_dsc_last equ 0
forward
if defined dsc_#_pref#_#_name#_b
!WRONG!
end if
dsc_#_pref#_#_name equ _dsc_last
dsc_#_pref#_#_name#_b equ _size
_dsc_last equ dsc_#_pref#_#_name + dsc_#_pref#_#_name#_b
common
dsc_#_pref#_b equ _dsc_last
}
macro _dsc_in _pref,[_name,_size] {
common
if ~ defined dsc_#_pref
!WRONG!
end if
_dsc_in equ dsc_#_pref
forward
dsc_#_pref#_#_name equ _dsc_in
dsc_#_pref#_#_name#_b equ _size
_dsc_in equ dsc_#_pref#_#_name + dsc_#_pref#_#_name#_b
common
if (dsc_#_pref + dsc_#_pref#_b) <> _dsc_in
!WRONG LEN!
end if
}
macro _alias _pref,_base,[_al] {
forward
dsc_#_pref#_#_al equ dsc_#_pref#_#_base
}
macro _seq _pref,_start,[_name] {
common
_seq_last equ _start
forward
seq_#_pref#_#_name equ _seq_last
_seq_last equ _seq_last + 1
}
macro _buf _pref,_n {
if _n eq
align 10h
_pref#_buf:
  rb _#_pref#_size
else
_#_pref#_num  equ _n
_#_pref#_size equ ( #_n + 1 ) * ( dsc_#_pref#_end )
end if
}
macro _switch _j, _pref,[_name] {
common
      shl   _j,3
      add   _j,_pref
      jmp   _j
align 8
_pref:
forward
align 8
      jmp _pref#_#_name
}
macro _allign _a {
      add	_a,15
      and	_a,-16
}
macro __index _to,_type,_base,_ind {
}
macro _index _to,_st,_type,_base,_ind,_basebase {
_st equ dsc_#_type#_b
if ((defined dsc_#_type#_pnt) & (~ (_basebase eq )))
    mov    _base,[_basebase + dsc_#_type#_pnt]
else if ((~ (defined dsc_#_type#_pnt)) & (_basebase eq ))
else
UFUFUFUFU
end if
_to equ [_base + (_ind) * (dsc_#_type#_b) + dsc_#_type]
match =8, dsc_#_type#_b
\{
_to equ qword [_base + (_ind) * (dsc_#_type#_b) + dsc_#_type]
\}
match =4, dsc_#_type#_b
\{
_to equ dword [_base + (_ind) * (dsc_#_type#_b) + dsc_#_type]
\}
match =2, dsc_#_type#_b
\{
_to equ word [_base + (_ind) * (dsc_#_type#_b) + dsc_#_type]
\}
match =1, dsc_#_type#_b
\{
_to equ byte [_base + (_ind) * (dsc_#_type#_b) + dsc_#_type]
\}
}
macro _index_c0 _type,_base,_basebase {
_index _ic0,_sc0,_type,_base,rcx - 1,_basebase
}
macro _index_c1 _type,_base,_basebase {
_index _ic1,_sc1,_type,_base,rcx - 1,_basebase
}
macro _index_c2 _type,_base,_basebase {
_index _ic2,_sc2,_type,_base,rcx - 1,_basebase
}
macro _index_c3 _type,_base,_basebase {
_index _ic3,_sc3,_type,_base,rcx - 1,_basebase
}
macro _index0 _type,_base,_ind,_basebase {
_index _i0,_s0,_type,_base,_ind,_basebase
}
macro _index1 _type,_base,_ind,_basebase {
_index _i1,_s1,_type,_base,_ind,_basebase
}
macro _index2 _type,_base,_ind,_basebase {
_index _i2,_s2,_type,_base,_ind,_basebase
}
macro _index3 _type,_base,_ind,_basebase {
_index _i3,_s3,_type,_base,_ind,_basebase
}
macro _neg8 _fr {
      bt qword _fr,63
}
macro _neg4 _fr {
      bt dword _fr,31
}
macro _neg2 _fr {
      bt word _fr,15
}
macro _neg1 _fr {
      bt byte _fr,7
}
macro _neg _fr,_bt {
if _fr in <rax,rbx,rcx,rdx,rsi,rdi,r8,r9,r10,r11,r12,r13,r14,r15>
      bt _fr,63
else if _fr in <eax,ebx,ecx,edx,r8d,r9d,r10d,r11d,r12d,r13d,r14d,r15d>
      bt _fr,31
else if (_bt) eq 8
      bt _fr,63
else if (_bt) eq 4
      bt _fr,31
else if (_bt) eq 2
      bt _fr,15
else if (_bt) eq 1
      bt _fr,7
else
UFUFUFUFU
end if
}
macro _mov _to,_fr {
if ~(_to in <_fr, >)
      mov	_to,_fr
end if
}
macro _e _a,_b {
      clc
      jmp    _b
_a:
      stc
_b:
}
macro _local_set _type {
_loc_off = 0
macro _loc_c _off \{
_loc_#_type _off
\}
_loc_#_type 0
      sub	rsp,_loc_last
}
macro _local_at _type, _at {
_loc_off = _at
macro _loc_c _off \{
_loc_#_type _off
\}
_loc_#_type _at
}
macro _push _p {
_loc_off = _loc_off + 8
_loc_c _loc_off
      push   _p
}
macro _pop  _p {
_loc_off = _loc_off - 8
_loc_c _loc_off
      pop    _p
}
macro _local_unset {
_loc_c _loc_off
      add	rsp,_loc_last
}

macro _regs [_name] {
common
macro _check _reg \{
forward
if _reg eq _name
!WRONG CALL PARM!
end if
common
\}
}


macro _call _name, [_num,_parm] {
common
macro _check_#_name _reg \{
forward
if _reg eq _parm
!WRONG CALL PARM!
end if
common
\}
forward
macro _p_#_name#_#_num _reg \{
if _parm in <eax,ebx,ecx,edx,r8d,r9d,r10d,r11d,r12d,r13d,r14d,r15d,rax,rbx,rcx,rdx,rsi,rdi,r8,r9,r10,r11,r12,r13,r14,r15>
 if (~(_reg eq _parm)) & (~(_reg eq ))
_check_#_name _reg
 mov _parm,_reg
 end if
end if
if _parm in <xmm0,xmm1,xmm2,xmm3,xmm4,xmm5,xmm6,xmm7,xmm8,xmm9,xmm10,xmm11,xmm12,xmm13,xmm14,xmm15>
 if (~(_reg eq _parm)) & (~(_reg eq ))
_check_#_name _reg
 movdqa _parm,_reg
 end if
end if
\}
__p#_num equ _parm
__num equ _num
common
_c#_name#_:
match =0,__num \{
_name __p0
macro call#_name _p0 \\{
 _p_#_name#_0 _p0
 call near _c#_name#_
\\}
\}
match =1,__num \{
_name __p0,__p1
macro call#_name _p0,_p1 \\{
 _p_#_name#_0 _p0
 _p_#_name#_1 _p1
 call near _c#_name#_
\\}
\}
match =2,__num \{
_name __p0,__p1,__p2
macro call#_name _p0,_p1,_p2 \\{
 _p_#_name#_0 _p0
 _p_#_name#_1 _p1
 _p_#_name#_2 _p2
 call near _c#_name#_
\\}
\}
match =3,__num \{
_name __p0,__p1,__p2,__p3
macro call#_name _p0,_p1,_p2,_p3 \\{
 _p_#_name#_0 _p0
 _p_#_name#_1 _p1
 _p_#_name#_2 _p2
 _p_#_name#_3 _p3
 call near _c#_name#_
\\}
\}
match =4,__num \{
_name __p0,__p1,__p2,__p3,__p4
macro call#_name _p0,_p1,_p2,_p3,_p4 \\{
 _p_#_name#_0 _p0
 _p_#_name#_1 _p1
 _p_#_name#_2 _p2
 _p_#_name#_3 _p3
 _p_#_name#_4 _p4
 call near _c#_name#_
\\}
\}
match =5,__num \{
_name __p0,__p1,__p2,__p3,__p4,__p5
macro call#_name _p0,_p1,_p2,_p3,_p4,_p5 \\{
 _p_#_name#_0 _p0
 _p_#_name#_1 _p1
 _p_#_name#_2 _p2
 _p_#_name#_3 _p3
 _p_#_name#_4 _p4
 _p_#_name#_5 _p5
 call near _c#_name#_
\\}
\}
match =6,__num \{
_name __p0,__p1,__p2,__p3,__p4,__p5,__p6
macro call#_name _p0,_p1,_p2,_p3,_p4,_p5,_p6 \\{
 _p_#_name#_0 _p0
 _p_#_name#_1 _p1
 _p_#_name#_2 _p2
 _p_#_name#_3 _p3
 _p_#_name#_4 _p4
 _p_#_name#_5 _p5
 _p_#_name#_6 _p6
 call near _c#_name#_
\\}
\}
match =7,__num \{
_name __p0,__p1,__p2,__p3,__p4,__p5,__p6,__p7
macro call#_name _p0,_p1,_p2,_p3,_p4,_p5,_p6,_p7 \\{
 _p_#_name#_0 _p0
 _p_#_name#_1 _p1
 _p_#_name#_2 _p2
 _p_#_name#_3 _p3
 _p_#_name#_4 _p4
 _p_#_name#_5 _p5
 _p_#_name#_6 _p6
 _p_#_name#_7 _p7
 call near _c#_name#_
\\}
\}
match =8,__num \{
_name __p0,__p1,__p2,__p3,__p4,__p5,__p6,__p7,__p8
macro call#_name _p0,_p1,_p2,_p3,_p4,_p5,_p6,_p7,_p8 \\{
 _p_#_name#_0 _p0
 _p_#_name#_1 _p1
 _p_#_name#_2 _p2
 _p_#_name#_3 _p3
 _p_#_name#_4 _p4
 _p_#_name#_5 _p5
 _p_#_name#_6 _p6
 _p_#_name#_7 _p7
 _p_#_name#_8 _p8
 call near _c#_name#_
\\}
\}
match =9,__num \{
_name __p0,__p1,__p2,__p3,__p4,__p5,__p6,__p7,__p8,__p9
macro call#_name _p0,_p1,_p2,_p3,_p4,_p5,_p6,_p7,_p8,_p9 \\{
 _p_#_name#_0 _p0
 _p_#_name#_1 _p1
 _p_#_name#_2 _p2
 _p_#_name#_3 _p3
 _p_#_name#_4 _p4
 _p_#_name#_5 _p5
 _p_#_name#_6 _p6
 _p_#_name#_7 _p7
 _p_#_name#_8 _p8
 _p_#_name#_9 _p9
 call near _c#_name#_
\\}
\}
match =10,__num \{
_name __p0,__p1,__p2,__p3,__p4,__p5,__p6,__p7,__p8,__p9,__p10
macro call#_name _p0,_p1,_p2,_p3,_p4,_p5,_p6,_p7,_p8,_p9,_p10 \\{
 _p_#_name#_0 _p0
 _p_#_name#_1 _p1
 _p_#_name#_2 _p2
 _p_#_name#_3 _p3
 _p_#_name#_4 _p4
 _p_#_name#_5 _p5
 _p_#_name#_6 _p6
 _p_#_name#_7 _p7
 _p_#_name#_8 _p8
 _p_#_name#_9 _p9
 _p_#_name#_10 _p10
 call near _c#_name#_
\\}
\}
retn
}
