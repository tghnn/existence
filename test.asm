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
m_main_startup 10000000, 10, 1000
jc eee
m_api_medium_create 1, 100, 200
m_api_result
jc eee
mov [med0],rdx
m_api_destination_set [med0], 0
m_api_result
jc eee
mov [dst0],rdx

m_api_thread_create _tst_
m_api_result
jc eee
m_thread_wait 10000
m_med_getpnt [med0]
m_task_dest_start rdi,[dst0]
jc eee
m_task_dest_ask rdi,rsi,[dst0],rbx,10000
jc eee
m_task_dest_roger rdi,rsi,[dst0],r11
jc eee
m_task_dest_finish rdi,[dst0]
jc eee
m_thread_wait 10000
m_task_dest_start rdi,[dst0]
jc eee
m_task_dest_ask rdi,rsi,[dst0],rbx,10000
jc eee
m_task_dest_roger rdi,rsi,[dst0],r11
jc eee
m_task_dest_ask rdi,rsi,[dst0],rbx,10000
jc eee
m_task_dest_roger rdi,rsi,[dst0],r11
jc eee
m_task_dest_finish rdi,[dst0]
jc eee
m_api_result
invoke Sleep,10
m_med_getpnt [med0]
m_dest_unset rdi,[dst0]
m_med_count rdi,r8,r9
invoke	ExitProcess,[msg.wParam]
eee:
db 0CCh
dq 0,0,0
eee1:
db 0CCh
dq 4,4,4


proc _tst_
m_thread_head
m_api_source_create [med0], 1000, 0, 611, 6
m_api_result
jc eee1
mov	[src0],rdx
m_med_getpnt [med0]
m_link_set rdi,[dst0], [src0]
jc eee1
m_task_sour_start rdi,[src0]
jc eee1
m_task_sour_ask rdi,[src0],rbx,800
m_task_sour_roger rdi,[src0],r11
m_task_sour_finish rdi,[src0]
jc eee1
m_thread_wait 10000
m_task_sour_start rdi,[src0]
jc eee1
m_task_sour_ask rdi,[src0],rbx,300
jc eee1
m_task_sour_roger rdi,[src0],r11
jc eee1
add r11,400
m_task_sour_ask rdi,[src0],rbx,r11
jc eee1
m_task_sour_roger rdi,[src0],r11
jc eee1
m_task_sour_finish rdi,[src0]
jc eee1
m_api_thread_destroy
endp

section '.data' data readable writeable
med_data
med0 dq 0
dst0 dq 0
src0 dq 0
section '.bss' readable writeable

;  hinstance dq ?
;  hwnd dq ?
  msg MSG


section '.idata' import data readable

  library kernel32,'KERNEL32.DLL',\
	  user32,'USER32.DLL'

  include 'api\kernel32.inc'
  include 'api\user32.inc'

