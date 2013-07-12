macro _dsc _pref,[_name,_size] {
common
_dsc_last equ 0
forward
dsc_#_pref#_#_name equ _dsc_last
dsc_#_pref#_#_name#_b equ _size
_dsc_last equ dsc_#_pref#_#_name + dsc_#_pref#_#_name#_b
common
dsc_#_pref#_b equ _dsc_last
}
macro _dsc_in _pref,[_name,_size] {
common
if dsc_#_pref eq
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
macro _index _to,_st,_type,_base,_ind {
_st equ dsc_#_type
if (dsc_#_type#_b) eq 8
_to equ _base + (_ind) * 8 + dsc_#_type
else if (dsc_#_type#_b) eq 4
_to equ _base + (_ind) * 4 + dsc_#_type
else if (dsc_#_type#_b) eq 2
_to equ _base + (_ind) * 2 + dsc_#_type
else if (dsc_#_type#_b) eq 1
_to equ _base + (_ind) + dsc_#_type
else
UFUFUFUFU
end if
}
macro _index_c _type,_base {
_index _ic,_sc,_type,_base,rcx - 1
}
macro _index_c1 _type,_base {
_index _ic1,_sc1,_type,_base,rcx - 1
}
macro _index0 _type,_base,_ind {
_index _i0,_s0,_type,_base,_ind
}
macro _index1 _type,_base,_ind {
_index _i1,_s0,_type,_base,_ind
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
macro _end {
      clc
      jmp    _end
_err:
      stc
_end:
}
macro _local_set _type {
_loc_off equ 0
_loc_typ equ _type
      sub	rsp,dsc_#_type#_b
_il equ rsp + _loc_off + dsc_#_type
_sl equ dsc_#_type
}
macro _push _p {
_loc_off equ _loc_off + 8
      push   _p
}
macro _pop  _p {
_loc_off equ _loc_off - 8
      pop    _p
}
macro _local_unset {
      add	rsp,dsc_#_loc_type#_b
}
