macro _api_prep {
      mov	rbx,loc_task_thread
      mov	rax,dsc_main_thrd_api_b
      mul	rbx
      mov	rdx,[main]
      add	rax,[rdx + dsc_main_thrd_api_pnt]
_index0 main_thrd_event,rdx,rbx
}

macro proc_med_api {
local _l0,_l1
_m_api_res:
_api_prep
      mov	rdx,_i0
      _push	r10,r11,r12,r13,r14,r15
_l0:
      _neg8	[rax + dsc_main_thrd_api_stat]
      jc	_l1
      _push	rax,rdx
invoke WaitForSingleObject,rdx,100
      _pop	rax,rdx
      jmp	_l0
_l1:
      _push	rax,rdx
invoke ResetEvent,rdx
      _pop	rax,rdx
      _pop	r10,r11,r12,r13,r14,r15
      mov	rdx,[rax + dsc_main_thrd_api_result]
      _neg	rdx
      retn
_m_api_call5:
      mov	[rax + dsc_main_thrd_api_parm4],r14
_m_api_call4:
      mov	[rax + dsc_main_thrd_api_parm3],r13
_m_api_call3:
      mov	[rax + dsc_main_thrd_api_parm2],r12
_m_api_call2:
      mov	[rax + dsc_main_thrd_api_parm1],r11
_m_api_call1:
      mov	[rax + dsc_main_thrd_api_parm0],r10
_m_api_call0:
      mov	[rax + dsc_main_thrd_api_stat],r8
      mov	rdx,[main]
      mov	rdx,[rdx + dsc_main_thrd_event]
invoke SetEvent,rdx
      retn
; ------ direct call ------
_call m_thread_clear,_med_thrd_c
_call m_thread_wait,_med_thrd_w,r11
;                              timout
_call m_thread_sournext,_med_thrd_sournext,rdi,rsi, r11
_call m_thread_destnext,_med_thrd_destnext,rdi, r11

_call m_med_count,_med_count,rdi,r8,r9
;out:r8 sour,r9 dest
_call m_med_getpnt,_med_getpnt,rdi
;m_med_getpnt _med_num
;out: rdi
_call m_sour_unset,_med_sour_unset,rdi, r11
;m_sour_unset  medium_pnt,_sour_num
_call m_sour_trans,_med_sour_trans,rdi, r11, r15
;m_dest_unset  medium_pnt, sour_num, thread_num

_call m_dest_unset,_med_dest_unset,rdi,  r11
;m_dest_unset  medium_pnt, dest_num
_call m_dest_trans,_med_dest_trans,rdi,  r11, r15
;m_dest_unset  medium_pnt, dest_num, thread_num
;----
_call m_link_set,_med_link_set,rdi,r14,  r15
;m_link_set    medium_pnt, dest_num, sour_num
_call m_link_unset,_med_link_unset,rdi,r14, r15
;m_link_unset  medium_pnt, dest_num, sour_num
;change last_dest, wake sour
;----
_call m_task_sour_start,_med_task_sour_start,rdi,r11
;out: r11 - links at sour
_call m_task_sour_finish,_med_task_sour_finish,rdi,r11
_call m_task_sour_abort,_med_task_sour_abort,rdi,r11
;m_task_sour_...    medium_pnt, sour_num
_call m_task_dest_start,_med_task_dest_start,rdi,r11
;m_task_dest_start    medium_pnt, dest_num
;out: r11 - tasked links
_call m_task_dest_finish,_med_task_dest_finish,rdi,r11
_call m_task_dest_abort,_med_task_dest_abort,rdi,r11
;m_task_dest_...    medium_pnt, dest_num
;----
_call m_task_sour_ask,_med_task_sour_ask,rdi,r12,rbx,r11
;m_task_sour_ask medium_pnt, sour_num, adr, size
;in: adr - ignored
;out: rbx, r11
_call m_task_dest_ask,_med_task_dest_ask,rdi,rsi,r12,rbx,r11
;m_task_dest_ask medium_pnt,  sour_pnt, dest_num, adr, size
;in: sour_pnt,adr - ignored
;out: rsi, rbx, r11
_call m_task_sour_roger,_med_task_sour_roger,rdi,r10,r11
;m_task_sour_roger  medium_pnt, sour_num, size
_call m_task_dest_roger,_med_task_dest_roger,rdi,rsi,r10,r11
;m_task_dest_roger medium_pnt, sour_pnt, dest_num, size




_med_file_thrd:
local _f0,_f1,_f2,_f3,_f4,_f5,_f6,_f7,_f8,_f9
local _f10,_f11,_f12,_f13,_f14,_f15,_f16,_f17,_f18,_f19
m_thread_head

_f0:
	mov	rdi,loc_task_medium
	mov	loc_task_flag,0
m_thread_destnext rdi,-1
_f1:
	_neg	r11
	jc	_f2
	mov	loc_task_dest,r11
_index0 med_dest_hndl,rdx,r11,,rdi ;??????????????????????????????
	mov	rax,_i0
	_neg	rax
	jc	_f2
	mov	loc_task_handler,rax
m_task_dest_start rdi, r11
	jnc	_f6
m_task_dest_abort rdi, loc_task_dest
	jmp	_f3
_f6:
	or	r11,r11
	jnz	_f4
m_dest_unset  rdi, loc_task_dest
	mov	rbx,loc_task_handler
invoke	CloseHandle, rbx
	jmp	_f3
_f4:
m_task_dest_ask rdi, rsi, loc_task_dest, rbx, -1
	jc	_f5
	or	r11,r11
	jz	_f5
	mov	r10,loc_task_handler
	lea	r9,loc_task_tmp
	push	rdi
	push	rsi
invoke	WriteFile, r10,rbx,r11,r9, NULL
	pop	rsi
	pop	rdi
	or	rax,rax
	jz	_f5
	inc	loc_task_flag
m_task_dest_roger rdi, rsi, loc_task_dest, loc_task_tmp
	jmp	_f4
_f5:
m_task_dest_finish rdi, loc_task_dest
_f3:
m_thread_destnext rdi,loc_task_dest
	or	r11,r11
	jnz	_f1
_f2:


m_thread_sournext rdi,rsi,-1
_f11:
	_neg	r11
	jc	_f12
	mov	loc_task_sour,r11

	mov	rax,[rsi + dsc_med_sour_hndl] ;??????????????????????????
	_neg	rax
	jc	_f12
	mov	loc_task_handler,rax
m_task_sour_start rdi, r11
	jnc	_f16
m_task_sour_abort rdi,loc_task_sour
	jmp	_f13
_f16:
	or	r11,r11
	jnz	_f14
m_sour_unset rdi,loc_task_sour
	mov	rbx,loc_task_handler
invoke	CloseHandle, rbx
	jmp	_f13
_f14:
m_task_sour_ask rdi, loc_task_sour, rbx, -1
	jc	_f15
	or	r11,r11
	jz	_f15
	mov	r10,loc_task_handler
	lea	r9,loc_task_tmp
	push	rdi
invoke	ReadFile, r10,rbx,r11,r9, NULL
	pop	rdi
	or	rax,rax
	jz	_f15
	inc	loc_task_flag
m_task_sour_roger rdi, loc_task_sour, loc_task_tmp
	jmp	_f14
_f15:
m_task_sour_finish rdi, loc_task_sour
_f13:
m_thread_sournext rdi,rsi,loc_task_dest
	or	r11,r11
	jnz	_f11
_f12:

	cmp	loc_task_flag,0
	jnz	_f0
m_thread_wait 100
	jmp	_f0
}

macro m_thread_head {
local _l0
      mov	rax,[api_thread]
      mov	[api_thread],-1
      xor	rbp,rbp
      enter	loc_task,0
      mov	loc_task_thread,rax
      mov	rdx,dsc_main_thrd_api_b
      mul	rdx
      mov	rbx,[main]
      add	rax,[rbx + dsc_main_thrd_api_pnt]
      mov	rdx,[rax + dsc_main_thrd_api_parm1]
      mov	rcx,-1
      _neg	rdx
      jc	_l0
_index0 main_med,rax,rdx,,rbx
      mov	rcx,_i0
_l0:
      mov	loc_task_medium,rcx
}
; ------ api call  ----------

macro m_api_result {
call near _m_api_res
}

macro m_api_thread_create _thread_proc, _medium, _parm0, _parm1 {
_regs r8,r10,r11
_parm_set _thread_proc,r10
_parm_set _medium,r11
_parm_set _parm0,r12
_parm_set _parm1,r13
	  call near _m_api_res
	  mov	    r8,med_api_thread_create
	  call near _m_api_call4
}
macro m_api_thread_destroy {
_regs r8
	  call near _m_api_res
	  mov	    r8,med_api_thread_destroy
	  call near _m_api_call0
	  leave
invoke ExitThread
}

macro m_api_medium_create _medium_step_size, _medium_sours_qty, _medium_dests_qty {
_regs r8,r10,r11,r12
_parm_set _medium_step_size,r10
_parm_set _medium_sours_qty,r11
_parm_set _medium_dests_qty,r12
	  call near _m_api_res
	  mov	    r8,med_api_medium_create
	  call near _m_api_call3
}


macro m_api_medium_destroy _medium_num {
_regs r8,r10
_parm_set _medium_num,r10
	  call near _m_api_res
	  mov	    r8,med_api_medium_destroy
	  call near _m_api_call1
}

macro m_api_source_create _source_med_num, _source_step_qty, _source_handl, _source_dist_wake, _source_dist_dest {
_regs r8,r10,r11,r12,r13,r14
_parm_set _source_med_num,r10
_parm_set _source_step_qty,r11
_parm_set _source_handl,r12
_parm_set _source_dist_wake,r13
_parm_set _source_dist_dest,r14
	  call near _m_api_res
	  mov	    r8,med_api_source_create
	  call near _m_api_call5
}
macro m_api_destination_set _destination_med_num, _destination_handl {
_regs r8,r10,r11
_parm_set _destination_med_num,r10
_parm_set _destination_handl,r11
	  call near _m_api_res
	  mov	    r8,med_api_destination_set
	  call near _m_api_call2
}

;-----------------------------
macro m_main_startup _memory_size, _mediums_qty, _threads_qty {
_regs r11,r12,r13
_parm_set _memory_size,r11
_parm_set _mediums_qty,r12
_parm_set _threads_qty,r13
      enter	loc_task,0
      call	near _med_startup
}

macro m_main_fileup _files_qty {
local _err
m_api_medium_create 1, _files_qty, _files_qty
m_api_result
jc _err
mov [med_file],rdx
m_api_thread_create _med_file_thrd,[med_file],0,0
m_api_result
_err:
}

macro med_api_data {
med_file      dq    -1
iotab_todec   db    '0123456789'
iotab_tohex   db    '0123456789ABCDEF'
iotab_dec     db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,0C0h,0A0h,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,0C0h,0A0h,255
	      db      0,  1,  2,  3,  4,  5,  6,  7,   8,  9,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255

	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
iotab_acc     db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db      0,  1,  2,  3,  4,  5,  6,  7,   8,  9,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255

	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255

iotab_hex     db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,0C0h,0A0h,255
	      db      0,  1,  2,  3,  4,  5,  6,  7,   8,  9,255,255,255,255,255,255
	      db    255, 10, 11, 12, 13, 14, 15,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255, 10, 11, 12, 13, 14, 15,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255

	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
	      db    255,255,255,255,255,255,255,255, 255,255,255,255,255,255,255,255
}
