format PE64 GUI 5.0
entry start

include 'win64ax.inc'
include '611_o.inc'
include 'medium.inc'
include 'medium_api.inc'


section '.text' code readable executable
start:
_l0:
_l1:
_l2:
_l3:
_l4:
_l5:
_l6:
_l7:
_l8:
_l9:

  and	rsp,-16
  jmp	rest
proc_med
proc_med_api
proc_med_sys
rest:
m_main_startup 10000000, 10, 200
jc eee
m_main_fileup 100
jc eee

f_open_append ftstw,1000,6
jc eee
f_open_read ftstr,1000,6
jc eee
m_thread_wait 1000
_oooo:
m_med_getpnt [file_medium]
m_task_sour_start rdi,loc_task_file_sour
jc eee
m_task_dest_start rdi,loc_task_file_dest
jc eee
or r11,r11
jz _o0
m_task_sour_ask rdi,loc_task_file_sour,rbx,-1
jc eee
or r11,r11
jz _oo1
push rbx
m_task_dest_ask rdi,rsi,loc_task_file_dest,rbx,r11
pop rdx
jc eee
or r11,r11
jz _oo1
mov rcx,r11
__tttt:
mov al,[rbx]
mov [rdx],al
inc rbx
inc rdx
loop __tttt
push r11
m_task_dest_roger rdi,rsi,loc_task_file_dest,r11
pop r11
jc eee
m_task_sour_roger rdi,loc_task_file_sour,r11
jc eee
m_task_dest_finish rdi,loc_task_file_dest
jc eee
m_task_sour_finish rdi,loc_task_file_sour
jc eee
jmp _oooo
_oo1:
m_task_dest_abort rdi,loc_task_file_dest
jc eee
m_task_sour_abort rdi,loc_task_file_sour
jc eee
m_thread_wait 1000
jc eee
jmp _oooo
_o0:
m_sour_unset rdi,loc_task_file_sour
jc eee
invoke Sleep,1000
invoke	ExitProcess,[msg.wParam]


eee:
db 0CCh
dq 0,0,0



section '.data' data readable writeable
med_data
med_api_data
med0 dq 0
dst0 dq 0
src0 dq 0
uuuu dq 0
section '.bss' readable writeable

;  hinstance dq ?
;  hwnd dq ?
  msg MSG
ftstr db "aaaaa.in",0
ftstw db "aaaaa.out",0

section '.idata' import data readable

  library kernel32,'KERNEL32.DLL',\
	  user32,'USER32.DLL'

  include 'api\kernel32.inc'
  include 'api\user32.inc'

