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
