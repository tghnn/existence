macro _loc _pref,[_name,_size] {
common
_loc_last equ 0
forward
loc_#_pref#_#_name equ [rbp + _loc_last]
match =8, _size
\{
loc_#_pref#_#_name equ qword [rbp + _loc_last]
\}
match =4, _size
\{
loc_#_pref#_#_name equ dword [rbp + _loc_last]
\}
match =2, _size
\{
loc_#_pref#_#_name equ word [rbp + _loc_last]
\}
match =1, _size
\{
loc_#_pref#_#_name equ byte [rbp + _loc_last]
\}
_loc_last equ _loc_last + _size
common
loc_#_pref equ _loc_last
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
macro _index _to,_st,_type,_base,_ind,_off,_basebase {
_st equ dsc_#_type#_b
if ((defined dsc_#_type#_pnt) & (~ (_basebase eq )))
    mov    _base,[_basebase + dsc_#_type#_pnt]
else if ((~ (defined dsc_#_type#_pnt)) & (_basebase eq ))
else
UFUFUFUFU
end if
_t equ
match =8, dsc_#_type#_off#_b
\{
_t equ qword
\}
match =4, dsc_#_type#_off#_b
\{
_t equ dword
\}
match =2, dsc_#_type#_off#_b
\{
_t equ word
\}
match =1, dsc_#_type#_off#_b
\{
_t equ byte
\}
_to equ _t [_base + (_ind) * (dsc_#_type#_b) + dsc_#_type#_off]
}
macro _index_c0 _type,_base,_off,_basebase {
_index _ic0,_sc0,_type,_base,rcx - 1,_off,_basebase
}
macro _index_c1 _type,_base,_off,_basebase {
_index _ic1,_sc1,_type,_base,rcx - 1,_off,_basebase
}
macro _index_c2 _type,_base,_off,_basebase {
_index _ic2,_sc2,_type,_base,rcx - 1,_off,_basebase
}
macro _index_c3 _type,_base,_off,_basebase {
_index _ic3,_sc3,_type,_base,rcx - 1,_off,_basebase
}

macro _index0 _type,_base,_ind,_off,_basebase {
_index _i0,_s0,_type,_base,_ind,_off,_basebase
}
macro _index1 _type,_base,_ind,_off,_basebase {
_index _i1,_s1,_type,_base,_ind,_off,_basebase
}
macro _index2 _type,_base,_ind,_off,_basebase {
_index _i2,_s2,_type,_base,_ind,_off,_basebase
}
macro _index3 _type,_base,_ind,_off,_basebase {
_index _i3,_s3,_type,_base,_ind,_off,_basebase
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

macro _push [_aaa] {
forward
      push   _aaa
__stack equ __stack + 1
}
macro _pop  [_aaa] {
reverse
      pop    _aaa
__stack equ __stack - 1
}

macro _regs [_name] {
common
macro _parm_set _fr,_to \{
forward
if (_fr eq _name) & (~(_fr eq _to))
!WRONG CALL PARM!
end if
common
if _to in <eax,ebx,ecx,edx,r8d,r9d,r10d,r11d,r12d,r13d,r14d,r15d,rax,rbx,rcx,rdx,rsi,rdi,r8,r9,r10,r11,r12,r13,r14,r15>
_mov _to,_fr
end if
if _to in <xmm0,xmm1,xmm2,xmm3,xmm4,xmm5,xmm6,xmm7,xmm8,xmm9,xmm10,xmm11,xmm12,xmm13,xmm14,xmm15>
movdqa _to,_fr
end if
\}
}


macro _call _cname,_mname, [_parm] {
common
macro _cname _p0,_p1,_p2,_p3,_p4,_p5,_p6,_p7,_p8,_p9,_p10 \{
_cc equ 0
_regs _parm
forward
match =0,_cc \\{
_pp equ _p0
\\}
match =0=+=1,_cc \\{
_pp equ _p1
\\}
match =0=+=1=+=1,_cc \\{
_pp equ _p2
\\}
match =0=+=1=+=1=+=1,_cc \\{
_pp equ _p3
\\}
match =0=+=1=+=1=+=1=+=1,_cc \\{
_pp equ _p4
\\}
match =0=+=1=+=1=+=1=+=1=+=1,_cc \\{
_pp equ _p5
\\}
match =0=+=1=+=1=+=1=+=1=+=1=+=1,_cc \\{
_pp equ _p6
\\}
match =0=+=1=+=1=+=1=+=1=+=1=+=1=+=1,_cc \\{
_pp equ _p7
\\}
match =0=+=1=+=1=+=1=+=1=+=1=+=1=+=1=+=1,_cc \\{
_pp equ _p8
\\}
match =0=+=1=+=1=+=1=+=1=+=1=+=1=+=1=+=1=+=1,_cc \\{
_pp equ _p9
\\}
match =0=+=1=+=1=+=1=+=1=+=1=+=1=+=1=+=1=+=1=+=1,_cc \\{
_pp equ _p10
\\}
_parm_set _pp,_parm
_cc equ _cc + 1
common
 call near _c#_mname#_
 if defined _#_mname#_
_#_mname#_
 end if
\}
_c#_mname#_:
_mname _parm
retn
}
