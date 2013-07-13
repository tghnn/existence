macro _loc _pref,[_name,_size] {
common
macro _loc_#_pref _off \{
_loc_last equ _off
forward
if (_size) = 1
loc_#_pref#_#_name equ byte [rsp + _loc_last]
else if (_size) = 2
loc_#_pref#_#_name equ word [rsp + _loc_last]
else if (_size) = 4
loc_#_pref#_#_name equ dword [rsp + _loc_last]
else if (_size) = 8
loc_#_pref#_#_name equ qword [rsp + _loc_last]
else
loc_#_pref#_#_name equ [rsp + _loc_last]
end if
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
macro _index _to,_st,_type,_base,_ind,_tmp {
_st equ dsc_#_type#_b
_aa equ _base
if defined dsc_#_type#_pnt
if (_tmp eq )
UFUFUFUFU
end if
    mov    _tmp,[_base + dsc_#_type#_pnt]
_aa equ _tmp
else
if ~ (_tmp eq )
UFUFUFUFU
end if
end if
_to equ _aa + (_ind) * (dsc_#_type#_b) + dsc_#_type
}
macro _index_c0 _type,_base,_tmp {
_index _ic0,_sc0,_type,_base,rcx - 1,_tmp
}
macro _index_c1 _type,_base,_tmp {
_index _ic1,_sc1,_type,_base,rcx - 1,_tmp
}
macro _index_c2 _type,_base,_tmp {
_index _ic2,_sc2,_type,_base,rcx - 1,_tmp
}
macro _index_c3 _type,_base,_tmp {
_index _ic3,_sc3,_type,_base,rcx - 1,_tmp
}
macro _index0 _type,_base,_ind,_pnt,_tmp {
_index _i0,_s0,_type,_base,_ind,_pnt,_tmp
}
macro _index1 _type,_base,_ind,_pnt,_tmp {
_index _i1,_s1,_type,_base,_ind,_pnt,_tmp
}
macro _index2 _type,_base,_ind,_pnt,_tmp {
_index _i2,_s2,_type,_base,_ind,_pnt,_tmp
}
macro _index3 _type,_base,_ind,_pnt,_tmp {
_index _i3,_s3,_type,_base,_ind,_pnt,_tmp
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
_neg8 _fr
else if _fr in <eax,ebx,ecx,edx,r8d,r9d,r10d,r11d,r12d,r13d,r14d,r15d>
_neg4 _fr
else if (_bt) eq 8
_neg8 _fr
else if (_bt) eq 4
_neg4 _fr
else if (_bt) eq 2
_neg2 _fr
else if (_bt) eq 1
_neg1 _fr
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
_loc_off equ 0
_loc_b equ loc_#_type#_b
macro _loc_c _off \{
_loc_#_type _off
\}
_loc_#_type 0
      sub	rsp,_loc_last
}
macro _push _p {
_loc_off equ _loc_off + 8
_loc_c _loc_off
      push   _p
}
macro _pop  _p {
_loc_off equ _loc_off - 8
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
