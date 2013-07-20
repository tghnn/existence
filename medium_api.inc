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
      mov	rdx,[rax + dsc_main_thrd_api_parm0]
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
_call m_sour_unset,_med_sour_unset,rdi,r15
;m_sour_unset  medium_pnt,_sour_num
_call m_sour_trans,_med_sour_trans,rdi,  r15, r11
;m_dest_unset  medium_pnt, sour_num, thread_num

_call m_dest_unset,_med_dest_unset,rdi,  r14
;m_dest_unset  medium_pnt, dest_num
_call m_dest_trans,_med_dest_trans,rdi,  r14, r11
;m_dest_unset  medium_pnt, dest_num, thread_num
;----
_call m_link_set,_med_link_set,rdi,r14,  r15
;m_link_set    medium_pnt, dest_num, sour_num
_call m_link_unset,_med_link_unset,rdi,r14, r15
;m_link_unset  medium_pnt, dest_num, sour_num
;change last_dest, wake sour
;----
_call m_task_sour_start,_med_task_sour_start,rdi,r11
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
_call m_task_sour_ask,_med_task_sour_ask,rdi,r10,rbx,r11
;m_task_sour_ask medium_pnt, sour_num, adr, size
;in: adr - ignored
;out: rbx, r11
_call m_task_dest_ask,_med_task_dest_ask,rdi,rsi,r10,rbx,r11
;m_task_dest_ask medium_pnt,  sour_pnt, dest_num, adr, size
;in: sour_pnt,adr - ignored
;out: rsi, rbx, r11
_call m_task_sour_roger,_med_task_sour_roger,rdi,r10,r11
;m_task_sour_roger  medium_pnt, sour_num, size
_call m_task_dest_roger,_med_task_dest_roger,rdi,rsi,r10,r11
;m_task_dest_roger medium_pnt, sour_pnt, dest_num, size
}

macro m_thread_head {
      mov      rax,[thread]
      mov      [thread],-1
      xor      rbp,rbp
      enter    loc_task
      mov      loc_task_thread,rax
}
; ------ api call  ----------

macro m_api_result {
call near _m_api_res
}

macro m_api_thread_create _thread_proc {
_regs r8,r10
_parm_set _thread_proc,r10
	  call near _m_api_res
	  mov	    r8,med_api_thread_create
	  call near _m_api_call1
}
macro m_api_thread_destroy {
_regs r8
	  call near _m_api_res
	  mov	    r8,med_api_thread_destroy
	  call near _m_api_call0
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
macro m_api_destination_set _destination_med_num, _destionation_handl {
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
      call	near _med_startup
}
