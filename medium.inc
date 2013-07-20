;for Them, who my death and my life

_dsc mem,size,8,next,8,prev,8
_dsc main,size,8,med_qty,8,med_pnt,8,thrd_qty,8,thrd_api_pnt,8,thrd_event,8
_dsc main,med,8
_dsc main_thrd_api,stat,8,parm0,8,parm1,8,parm2,8,parm3,8,parm4,8

_dsc med,size,8,step_size,8,flags,8,dest_qty,8,dest_sour_pnt,8,dest_thrd_pnt,8,dest_hndl_pnt,8, \
		sour_qty,8,sours_pnt,8
;flag: 0b - has deleting sources
_dsc med_dest,sour,4 ; number of sour (for task)
_dsc med_dest,thrd,4 ;if setted != -1
_dsc med_dest,hndl,8
_dsc med_sour,size,8,step_qty,8,thrd,8,hndl,8,dist_wake,8,dist_dest,8,buf_pnt,8,dest_last,8,pos,8,dests_pos,8
_dsc_in med_sour_pos,task,4,set,4
_dsc_in med_sour_dests_pos,task,4,set,4
;pos: h32 - set position, l32 - task position
;last dest = min dist(sour -> dest), if -1 = not have linked dest

_loc task,thread,8

;write/read in step
;write: first free, always at least 1 (num to write = min dist from +1 to read)
;reader: point at used, if ==write then nothing read (num to read = dist to write)
;link_num = numread + write(1)
macro _mem_init _size {
local _end
invoke GetProcessHeap
      cmp	rax,1
      jc	_end
      mov	rdi,rax
      mov	rsi,_size
      push	rsi
invoke HeapAlloc,rdi,0,rsi
      pop	rsi
      cmp	rax,1
      jc	_end
      mov	[mem],rax
      mov	rdi,rax

      mov	rax,dsc_mem_b
      add	rax,rdi
      sub	rsi,rax
      mov	qword [rdi + dsc_mem_size],0
      mov	[rdi + dsc_mem_next],rax
      mov	[rdi + dsc_mem_prev],rax
      mov	qword [rax + dsc_mem_size],rsi
      mov	qword [rax + dsc_mem_next],rdi
      mov	qword [rax + dsc_mem_prev],rdi
_end:
}
macro _mem_chain_del _del {
      mov	rax,[_del + dsc_mem_prev]
      mov	rdx,[_del + dsc_mem_next]
      mov	[rax + dsc_mem_next],rdx
      mov	[rdx + dsc_mem_prev],rax
}
macro _mem_chain_ins_after _ins,_after {
      mov	rax,[_after + dsc_mem_next]
      mov	[_ins + dsc_mem_next],rax
      mov	qword [_ins + dsc_mem_prev],_after
      mov	qword [_after + dsc_mem_next],_ins
      mov	qword [rax + dsc_mem_prev],_ins
}
macro _mem_chain_move _from,_to {
      mov	rax,[_from + dsc_mem_prev]
      mov	rdx,[_from + dsc_mem_next]
      mov	[_to + dsc_mem_prev],rax
      mov	[_to + dsc_mem_next],rdx
      mov	qword [rax + dsc_mem_next],_to
      mov	qword [rdx + dsc_mem_prev],_to
}
;out:rbx
macro _mem_get _pnt,_size { ;def rdi, r8
local _l0,_l1,_l2,_l3,_l4,_l5,_err,_end
_regs rax,rbx,rcx,rdx,rdi
_parm_set _size
      push	rsi
      push	rdi
      add	_size,8
_allign _size
      mov	rdi,[mem]
      mov	rdi,[rdi + dsc_mem_next]
      mov	rbx,-1
      mov	rcx,-1
_l0:
      cmp	rdi,[mem]
      jz	_l2
      mov	rax,[rdi + dsc_mem_size]
      cmp	rax,rcx
      jnc	_l1
      cmp	rax,_size
      jc	_l1
      mov	rcx,rax
      mov	rbx,rdi
      jz	_l2
_l1:
      mov	rdi,[rdi + dsc_mem_next]
      jmp	_l0
_l2:
;size rcx pnt rbx
      cmp	rbx,-1
      jz	_err
      mov	rax,_size
      cmp	rcx,rax
      jz	_l4
      add	rax,dsc_mem_b
      cmp	rcx,rax
      jnc	_l3
      mov	rcx,rax
      jmp	_l4
_l3:
;cut from block
      mov	[rbx + dsc_mem_size],_size
      mov	rdx,rcx
      sub	rdx,_size
      mov	rcx,rbx
      add	rcx,_size
      mov	[rcx + dsc_mem_size],rdx
_mem_chain_move rbx,rcx
      jmp	_l5
_l4:
;take all
      mov	[rbx + dsc_mem_size],rcx
_mem_chain_del rbx
_l5:
_e _err,_end
      pop	rdi
      pop	rsi
      _mov	_pnt,rbx
}
macro _mem_del _del { ;def rsi
local _l0,_l1,_l2,_l3,_l4,_l5,_l6,_end
_regs rax,rbx,rcx,rdx,rdi,r8,r9
_parm_set _del
      push	rsi
      push	rdi
      mov	rbx,_del
      add	rbx,[_del + dsc_mem_size]
      mov	rdi,[mem]
      mov	rdi,[rdi + dsc_mem_next]
      mov	r8,-1
      mov	r9,-1
_l0:
      cmp	rdi,[mem]
      jz	_l3
      cmp	rdi,rbx
      jnz	_l1
      mov	r9,rdi ;next adj block
      cmp	r8,-1
      jnz	_l3
      jmp	_l2
_l1:
      mov	rax,[rdi + dsc_mem_size]
      add	rax,rdi
      cmp	rax,_del
      jnz	_l2
      mov	r8,rdi ;prev adj block
      cmp	r9,-1
      jnz	_l3
_l2:
      mov	rdi,[rdi + dsc_mem_next]
      jmp	_l0
_l3:
      cmp	r8,-1
      jnz	_l4
      cmp	r9,-1
      jnz	_l5
;- -  ins at freechain
      mov	rdi,[mem]
_mem_chain_ins_after _del,rdi
      jmp	_end
_l4:
      cmp	r9,-1
      jnz	_l6
;+ -  concat at prev free block
      mov	rax,[_del + dsc_mem_size]
      add	[r8 + dsc_mem_size],rax
      jmp	_end
_l5:
;- +  move and concat next free block
_mem_chain_move r9,_del
      mov	rax,[r9 + dsc_mem_size]
      add	[_del + dsc_mem_size],rax
      jmp	_end
_l6:
;+ +  concat 2 blocks to prev free
_mem_chain_del r9
      mov	rax,[r9 + dsc_mem_size]
      add	rax,[_del + dsc_mem_size]
      add	[r8 + dsc_mem_size],rax
_end:
      pop	rdi
      pop	rsi
}



macro _main_calcsize _size,_med_qty,_thrd_qty {
      mov	_size,dsc_main_thrd_event
      mov	rax,dsc_main_thrd_event_b
      mov	rdx,_thrd_qty
      mul	rdx
      add	_size,rax
_allign _size
      mov	rax,dsc_main_med_b
      mov	rdx,_med_qty
      mul	rdx
      add	_size,rax
_allign _size
      mov	rax,dsc_main_thrd_api_b
      mov	rdx,_thrd_qty
      mul	rdx
      add	_size,rax
_allign _size
}
macro _main_fillparm _main,_med_qty,_thrd_qty {
      mov	qword [_main + dsc_main_med_qty],_med_qty
      mov	rax,dsc_main_thrd_event_b
      mov	rdx,_thrd_qty
      mov	[_main + dsc_main_thrd_qty],rdx
      mul	rdx
      add	rax,dsc_main_thrd_event
      add	rax,_main
_allign rax
      mov	[_main + dsc_main_med_pnt],rax
      mov	rax,dsc_main_thrd_api_b
      mov	rdx,_thrd_qty
      mul	rdx
      add	rax,[_main + dsc_main_med_pnt]
_allign rax
      mov	[_main + dsc_main_thrd_api_pnt],rax
}
;---------create main
macro _main_create _med_qty,_thrd_qty {
local _l0,_l1,_err,_end
      _neg8	[main]
      jnc	_err
      mov	[thread],-1
_main_calcsize r8,_med_qty,_thrd_qty
_mem_get r10,r8
      jc	_err
      mov	[main],r10
_main_fillparm r10,_med_qty,_thrd_qty
      mov	rax,-1
      mov	rcx,[r10 + dsc_main_med_qty]
_index_c0 main_med,rdx,,r10
_l0:
      mov	_ic0,rax
      loop	_l0
      mov	rcx,[r10 + dsc_main_thrd_qty]
_index_c0 main_thrd_event,r10
      mov	rbx,[r10 + dsc_main_thrd_api_pnt]
_l1:
      mov	_ic0,rax
      mov	[rbx + dsc_main_thrd_api_stat],rax
      add	rbx,dsc_main_thrd_api_b
      loop	_l1
_e _err,_end
}

macro _main_thread_set _thread_num, _thread_proc, _from_thread {
local _err,_err1,_end,_l0,_l1,_l2,_l3,_l4
_regs rax,rbx,rcx,rdx,r8,r9
_parm_set _thread_num,rcx
_parm_set _thread_proc,rbx
_parm_set _from_thread
      mov	rdx,[main]
      _neg	rcx
      jnc	_l4
      mov	rcx,[rdx + dsc_main_thrd_qty]
_index_c0 main_thrd_event,rdx
_l2:
      _neg	_ic0,_sc0
      jc	_l3
      loop	_l2
      jmp	_err
_l3:
      dec	rcx
_l4:
      mov	rdx,[main]
_index0 main_thrd_event,rdx,rcx
      _neg	_i0,_s0
      jnc	_err
      mov	r8,[rdx + dsc_main_thrd_api_pnt]
      mov	rax,dsc_main_thrd_api_b
      mul	rcx
      add	rax,r8
      mov	r9,rax ; to
      mov	rax,dsc_main_thrd_api_b
      mov	rdx,_from_thread
      mul	rdx
      add	r8,rax ; from
      mov	qword [r9 + dsc_main_thrd_api_stat],-1
      mov	rax,[r8 + dsc_main_thrd_api_parm0]
      mov	[r9 + dsc_main_thrd_api_parm0],rax
      mov	rax,[r8 + dsc_main_thrd_api_parm1]
      mov	[r9 + dsc_main_thrd_api_parm1],rax
      mov	rax,[r8 + dsc_main_thrd_api_parm2]
      mov	[r9 + dsc_main_thrd_api_parm2],rax
      mov	rax,[r8 + dsc_main_thrd_api_parm3]
      mov	[r9 + dsc_main_thrd_api_parm3],rax
      _push	rbx,rcx
invoke CreateEvent, NULL, TRUE, FALSE, NULL
      or	rax,rax
      jz       _err1
      _push    rax
      _neg8    [thread]
      jc       _l1
invoke Sleep,1
      _neg8    [thread]
      jc       _l1
invoke Sleep,10
      _neg8    [thread]
      jc       _l1
invoke Sleep,1000
      _neg8    [thread]
      jc       _l1
      _pop     rax
_err1:
      _pop     rbx,rcx
      jmp      _err
_l1:
      _pop     rbx,rcx,rax
      mov      rdx,[main]
      mov      _i0,rax
      mov      [thread],rcx
invoke CreateThread, NULL, 10000h, rbx, NULL, NULL, NULL
      test     rax, rax
      jz       _err
invoke CloseHandle, rax
_e _err,_end
}


;------------------------------------------
macro _med_calcsize _size,_sour_qty,_dest_qty {
      mov	_size,dsc_med_sours_pnt
      mov	rax,dsc_med_sours_pnt_b
      mov	rdx,_sour_qty
      mul	rdx
      add	_size,rax
_allign _size
      mov	rax,dsc_med_dest_sour_b
      mov	rdx,_dest_qty
      mul	rdx
      add	_size,rax
_allign _size
      mov	rax,dsc_med_dest_thrd_b
      mov	rdx,_dest_qty
      mul	rdx
      add	_size,rax
_allign _size
      mov	rax,dsc_med_dest_hndl_b
      mov	rdx,_dest_qty
      mul	rdx
      add	_size,rax
_allign _size
}
macro _med_fillparm _med, _sour_qty,_dest_qty,_step_size {
      mov	rdx,_step_size
      mov	[_med + dsc_med_step_size],rdx
      mov	rdx,_sour_qty
      mov	[_med + dsc_med_sour_qty],rdx
      mov	rax,dsc_med_sours_pnt_b
      mul	rdx
      add	rax,dsc_med_sours_pnt
      add	rax,_med
_allign rax
      mov	[_med + dsc_med_dest_sour_pnt],rax
      mov	rdx,_dest_qty
      mov	[_med + dsc_med_dest_qty],rdx

      mov	rax,dsc_med_dest_sour_b
      mul	rdx
      add	rax,[_med + dsc_med_dest_sour_pnt]
_allign rax
      mov	[_med + dsc_med_dest_thrd_pnt],rax

      mov	rax,dsc_med_dest_thrd_b
      mov	rdx,_dest_qty
      mul	rdx
      add	rax,[_med + dsc_med_dest_thrd_pnt]
_allign rax
      mov	[_med + dsc_med_dest_hndl_pnt],rax
}
;---------create/del medium
macro _med_create _med_num,_sour_qty,_dest_qty,_step_size {
;api
local _l0,_l1,_l2,_l3,_l4,_err,_end
_regs rax,rbx,rcx,rdx,rsi,rdi,r8,r10
_parm_set _med_num
_parm_set _sour_qty
_parm_set _dest_qty
_parm_set _step_size
      mov	rax,[main]
      _neg	_med_num
      jnc	_l2
      mov	rcx,[rax + dsc_main_med_qty]
_index_c0 main_med,rdx,,rax
_l3:
      _neg	_ic0,_sc0
      jc	_l4
      loop	_l3
      jmp	_err
_l4:
      dec	rcx
      mov	_med_num,rcx
_l2:
_index0 main_med,r10,_med_num,,rax
      _neg	_i0,_s0
      jnc	_err ;medium alredy exist
_med_calcsize r8,_sour_qty,_dest_qty
_mem_get rdi,r8
      jc	_err
      mov	_i0,rdi
_med_fillparm rdi, _sour_qty,_dest_qty,_step_size
      mov	qword [rdi + dsc_med_flags],0
      mov	rax,-1
      mov	rcx,[rdi + dsc_med_sour_qty]
_index_c0 med_sours_pnt,rdi
_l0:
      mov	_ic0,rax
      loop	_l0

      mov	rcx,[rdi + dsc_med_dest_qty]
_index_c0 med_dest_sour,rdx,,rdi
_index_c1 med_dest_thrd,rbx,,rdi
_index_c2 med_dest_hndl,rsi,,rdi
_l1:
      mov	_ic0,eax
      mov	_ic1,eax
      mov	_ic2,rax
      loop	_l1
_e _err,_end
}
macro _med_del _med_num {
;api, remove empty
local _err,_end,_l0
_regs rax,rdx,rcx,rsi
_parm_set _med_num
      mov	rax,[main]
_index0 main_med,rdx,_med_num,,rax
      mov	rsi,_i0
      _neg	rsi
      jc	_err ;medium non exist
      mov	rcx,[rsi + dsc_med_sour_qty]
_index_c0 med_sours_pnt,rsi
_l0:
      _neg	_ic0,_sc0
      jnc	_err
      loop	_l0
      mov	_i0,-1
_mem_del rsi
_e _err,_end
}

macro _med_parm_set_sournum _med, _sour_pnt,_sour_num {
local _l0
      mov	rax,_sour_num
      cmp	rax,[_med + dsc_med_sour_qty]
      cmc
      jc	_l0
_index0 med_sours_pnt,_med,rax
      mov	_sour_pnt,_i0
      _neg8	_sour_pnt
_l0:
}
macro _med_sour_calcsize _med,_size,_step_qty {
      mov	_size,dsc_med_sour_dests_pos
      mov	rdx,dsc_med_sour_dests_pos_b
      mov	rax,[_med + dsc_med_dest_qty]
      mul	rdx
      add	_size,rax
_allign _size
      mov	rax,_step_qty
      mul	qword [_med + dsc_med_step_size]
      add	_size,rax
_allign _size
}
macro _med_sour_fillparm _med,_med_sour,_step_qty {
      mov	rdx,dsc_med_sour_dests_pos_b
      mov	rax,[_med + dsc_med_dest_qty]
      mul	rdx
      add	rax,dsc_med_sour_dests_pos
      add	rax,_med_sour
_allign rax
      mov	[_med_sour + dsc_med_sour_buf_pnt],rax
      mov	rdx,_step_qty
      mov	[_med_sour + dsc_med_sour_step_qty],rdx
}
;------------create/del sourc
macro _med_sour_create _med, _sour_num,_step_qty,_thread,_handler,_dist_wake,_dist_dest {
;api, -num take next free
local _l0,_l1,_l2,_l3,_end,_err
_regs rax,rdx,rbx,rcx,rsi,r8
_parm_set _med
_parm_set _sour_num
_parm_set _step_qty
_parm_set _flag
_parm_set _thread
_parm_set _handler
_parm_set _dist_wake
_parm_set _dist_dest
      _neg	_sour_num
      jnc	_l1
      mov	rcx,[_med + dsc_med_sour_qty]
_index_c0 med_sours_pnt,_med
_l2:
      _neg	_ic0,_sc0
      jc	_l3
      loop	_l2
      jmp	_err
_l3:
      dec	rcx
      mov	_sour_num,rcx
_l1:
_med_sour_calcsize _med,r8,_step_qty
_mem_get rsi,r8
      jc	_err
_med_sour_fillparm _med,rsi, _step_qty
      mov	rax,_thread
      mov	[rsi + dsc_med_sour_thrd],rax
      mov	rax,_handler
      mov	[rsi + dsc_med_sour_hndl],rax
      mov	rax,_dist_wake
      mov	[rsi + dsc_med_sour_dist_wake],rax
      mov	rax,_dist_dest
      mov	[rsi + dsc_med_sour_dist_dest],rax
      xor	rax,rax
      mov	eax,-1
      mov	[rsi + dsc_med_sour_pos],rax
      mov	rcx,[_med + dsc_med_dest_qty]
      mov	rax,-1
_index_c1 med_sour_dests_pos,rsi
;fill dests pos (flag: -1 - not set, 0 - set all dest at this sour)
_l0:
      mov	_ic1,rax
      loop	_l0

      mov	rax,_sour_num
      cmp	rax,[_med + dsc_med_sour_qty]
      jnc	_err
_index0 med_sours_pnt,_med,rax
      _neg	_i0,_s0
      jnc	_err
      mov	_i0,rsi
_e _err,_end
}

macro _med_sour_trans _med, _sour_num, _tothread {
;direct, local_thread,wake
local _err,_end,_l0,_l1,_l2,_l3
_regs rax,rdx,rbx,rsi
_parm_set _med
_parm_set _dest_num
_parm_set _tothread,rdx
_med_parm_set_sournum _med, rsi,_sour_num
      jc	_err
      mov	rax,[rsi + dsc_med_sour_thrd]
      cmp	rax,loc_task_thread
      jnz	_err
      mov	rax,[main]
_index1 main_thrd_event,rax,rdx
      mov	rbx,_i1
      _neg	rbx
      jc	_err
      mov	[rsi + dsc_med_sour_thrd],rdx
invoke SetEvent,rbx
_e _err,_end
}
macro _med_sour_unset _med, _sour_num {
;direct, local_thread, wake linked dest, medium mark as deleted sour
local _err,_end,_l0,_l00,_l1
_regs rax,rdx,rcx,rsi
_parm_set _med
_parm_set _dest_num
_med_parm_set_sournum _med, rsi,_sour_num
      jc	_err
      mov	rax,loc_task_thread
      cmp	rax,[rsi + dsc_med_sour_thrd]
      jnz	_err
      xor	rax,rax
      mov	[rsi + dsc_med_sour_dist_dest],rax
      dec	rax
      mov	[rsi + dsc_med_sour_thrd],rax
      mov	[rsi + dsc_med_sour_dist_wake],rax

      mov	rcx,[_med + dsc_med_dest_qty]
_index_c0 med_sour_dests_pos,rsi
_l0:
      mov	rax,_ic0
      _neg	rax
      jc	_l1
      _neg	eax
      jnc	_l1
      mov	rax,rcx
      dec	rax
_index1 med_dest_thrd,rdx,_med,,rax
      xor	rax,rax
      mov	eax,_i1 ;thread of dest
      mov	rdx,[main]
_index1 main_thrd_event,rdx,rax
      _neg	_i1,_s1
      jc	_l1
      push	rcx
      push	rsi
      push	_med
      mov	rcx,_i1
invoke SetEvent,rcx
      pop	_med
      pop	rsi
      pop	rcx
      jmp      _l1
_l00:
      jmp      _l0
_l1:
      loop     _l00
      or       qword [_med + dsc_med_flags],1
_e _err,_end
}

macro _med_dest_set _med, _dest_num,_thread,_handler {
;api, -num take next free
local _l0,_l1,_l2,_err,_end
_regs rax,rdx,rcx
_parm_set _med
_parm_set _dest_num
_parm_set _thread
_parm_set _handler
      _neg	_dest_num
      jnc	_l0
      mov	rcx,[_med + dsc_med_dest_qty]
_index_c0 med_dest_thrd,rdx,,_med
_l1:
      _neg	_ic0,_sc0
      jc	_l2
      loop	_l1
      jmp	_err
_l2:
      dec	rcx
      mov	_dest_num,rcx
_l0:
_index0 med_dest_thrd,rdx,_dest_num,,_med
      _neg	_i0,_s0
      jnc	_err ;already exist dest
      mov	rax,_thread
      mov	_i0,eax
_index0 med_dest_hndl,rdx,_dest_num,,_med
      mov	rax,_handler
      mov	_i0,rax
_index0 med_dest_sour,rdx,_dest_num,,_med
      mov	_i0,-1
_e _err,_end
}

macro _med_dest_trans _med, _dest_num, _tothread {
;direct, local_thread, wake
local _err,_end
_regs rax,rdx,rbx,rcx
_parm_set _med
_parm_set _dest_num
_parm_set _tothread,rdx
_index0 med_dest_thrd,rbx,_dest_num,,_med
      xor	rax,rax
      mov	eax,_i0
      cmp	rax,loc_task_thread
      jnz	_err
      mov	rax,[main]
_index1 main_thrd_event,rax,rdx
      mov	rcx,_i1
      _neg	rcx
      jc	_err
      mov	_i0,edx
invoke SetEvent,rcx
_e _err,_end
}
macro _med_dest_unset _med, _dest_num {
;direct, local_thread, wake at linked sour
local _err,_end,_l0,_l1,_l2,_l3
_regs rax,rdx,rcx,rbx,rsi,r8
_parm_set _med
_parm_set _dest_num
_index0 med_dest_thrd,rdx,_dest_num,,_med
      xor	rax,rax
      mov	eax,_i0
      cmp	rax,loc_task_thread
      jnz	_err
_index_c0 med_sours_pnt,_med
_index0 med_sour_dests_pos,rsi,_dest_num
      mov	rcx,[_med + dsc_med_sour_qty]
_l0:
      mov	rsi,_ic0
      _neg	rsi
      jc	_l1
      _neg	_i0,_s0
      jc	_l1
      mov	_i0,-1
_med_task_dest_testwake _med, rsi, _dest_num
_l1:
      jmp	_l2
_l3:
      jmp	_l0
_l2:
      loop	_l3
_index0 med_dest_thrd,rdx,_dest_num,,_med
      mov	_i0,-1
_e _err,_end
}


;---------------------------------------------------------
macro _med_dist_dest _med_sour, _dist, _dest_num, _err, _prepared, _off {
;prepared: rdx = sour_pos, rbx = sour_step_num
local _l0,_l1
_regs rax,rdx,rbx
_parm_set _dest_num
if (_prepared eq 0)
      xor      rdx,rdx
      mov      edx,[_med_sour + dsc_med_sour_pos_set]
      _neg     edx
      jc       _err
      mov      rbx,[_med_sour + dsc_med_sour_step_qty]
end if
_index0 med_sour_dests_pos,_med_sour,_dest_num,_off
      xor      rax,rax
      mov      eax,_i0
      _neg     eax
      jc       _err
      inc      rax ; dest_pos+1
      cmp      rax,rbx
      jc       _l0
      sub      rax,rbx
_l0:
      neg      rax
      add      rax,rdx
      jns	_l1
      add      rax,rbx
_l1:
      _mov     _dist,rax
}


macro _med_dist_sour_findlast _med,_med_sour, _dist {
;prepared: rdx = sour_pos, rbx = sour_step_qty
local _l0,_l1,_l2
_regs rax,rdx,rbx,rcx,r8,r9
      mov      r8,-1
      mov      r9,-1
      mov      rcx,[_med + dsc_med_dest_qty]
_index_c0 med_sour_dests_pos,_med_sour,_set
_l0:
      xor      rax,rax
      mov      eax,_ic0
      _neg     eax
      jc       _l2 ;dest not set
      sub      rax,rdx
      jnc	_l1
      add      rax,rbx
_l1:
      cmp      rax,r8
      jnc      _l2
      mov      r8,rax
      mov      r9,rcx
      dec      r9
_l2:
      loop     _l0
      mov      [_med_sour + dsc_med_sour_dest_last],r9
      _neg     r8
      _mov     _dist,r8
}
macro _med_dist_sour _med,_med_sour, _dist, _err, _prepared {
;prepared: rdx = sour_pos, rbx = sour_step_num
local _l0,_l1
_regs rax,rdx,rbx,rcx,r8,r9
if (_prepared eq )
      xor      rdx,rdx
      mov      edx,[_med_sour + dsc_med_sour_pos_set]
      _neg     edx
      jc       _err
      mov      rbx,[_med_sour + dsc_med_sour_step_qty]
end if
      mov      rax,[_med_sour + dsc_med_sour_dest_last]
      _neg     rax
      jc       _l0
_index0 med_sour_dests_pos,_med_sour,rax,_set
      xor      rcx,rcx
      mov      ecx,_i0
      _neg     ecx
      jc       _l0
      mov      r8,rcx
      sub      r8,rdx
      jnc      _l1
      add      r8,rbx
      jmp      _l1
_l0:
_med_dist_sour_findlast _med,_med_sour, r8
_l1:
      _neg     r8
      _mov     _dist,r8
}


macro _med_link_set _med, _dest_num,_sour_num {
;direct, local_thread == (dest | sour), wake at sour
local _err,_end,_l0
_regs rax,rdx,rcx,rsi
_parm_set _med
_parm_set _dest_num
_parm_set _sour_num
_index0 med_dest_thrd,rdx,_dest_num,,_med
      _neg	_i0,_s0
      jc	_err ;non exist dest
_med_parm_set_sournum _med, rsi,_sour_num
      jc	_err ;not exist sourc
      mov	rdx,[rsi + dsc_med_sour_thrd]
      _neg	rdx
      jc	_err
      cmp	rdx,loc_task_thread
      jz	_l0
_index0 med_dest_thrd,rax,_dest_num,,_med
      xor	rax,rax
      mov	eax,_i0
      cmp	rax,loc_task_thread
      jnz	_err
_l0:
_index0 med_sour_dests_pos,rsi,_dest_num
      _neg	_i0,_s0
      jnc	_err ;already setted sour->dest
      mov	rax,[rsi + dsc_med_sour_pos]
      mov	eax,-1
      mov	_i0,rax ;set dest_pos = sour_pos
      cmp	rdx,loc_task_thread
      jz	_end ;no wake self sour
_med_task_dest_testwake _med, rsi, _dest_num
_e _err,_end
}

macro _med_link_unset _med, _dest_num,_sour_num {
local _l0,_err,_end
_regs rax,rcx,rdx,rsi
_parm_set _med
_parm_set _dest_num
_parm_set _sour_num
_index0 med_dest_thrd,rdx,_dest_num,,_med
      _neg	_i0,_s0
      jc	_err ;dest non exist
_med_parm_set_sournum _med, rsi,_sour_num
      jc	_err ;non exist sour
      mov	rdx,[rsi + dsc_med_sour_thrd]
      _neg	rdx
      jc	_err
      cmp	rdx,loc_task_thread
      jz	_l0
_index0 med_dest_thrd,rax,_dest_num,,_med
      xor	rax,rax
      mov	eax,_i0
      cmp	rax,loc_task_thread
      jnz	_err
_l0:
_index0 med_sour_dests_pos,rsi,_dest_num
      mov	rax,_i0
      _neg	rax
      jc	_err ;not setted link
      _neg	eax
      jnc	_err ;link at task
      mov	_i0,-1
      cmp	rdx,loc_task_thread
      jz	_end
_med_task_dest_testwake _med, rsi, _dest_num
_e _err,_end
}

;-------------------------------------------------
;max area ->dest, border(sour_pos)
;max area sour->, border(dests_pos[]-1)
macro _med_task_sour_prep _med, _med_sour, _sour_num, _err {
_med_parm_set_sournum _med, _med_sour,_sour_num
      jc       _err
      mov	rax,[_med_sour + dsc_med_sour_thrd]
      cmp	rax,loc_task_thread
      jnz	_err
      mov	rdx,[_med_sour + dsc_med_sour_pos]
      _neg	rdx
      jc	_err
      _neg	edx
}

macro _med_task_sour_start _med,_sour_num {
;direct, local_thread
local _err,_end
_regs rax,rdx
_parm_set _med
_parm_set _sour_num
_med_task_sour_prep _med, rsi, _sour_num, _err
      jnc      _err ;already in task
      mov      rax,rdx
      shr      rax,32
      mov      [rsi + dsc_med_sour_pos_task],eax
_e _err,_end
}
macro _med_task_dest_start _med,_dest_num {
;direct, local_thread
local _err,_end,_l0,_l1,_l2,_l3
_regs rax,rdx,rcx,rbx
_parm_set _med
_parm_set _dest_num
_index0 med_dest_thrd,rdx,_dest_num,,_med
      xor	rax,rax
      mov	eax,_i0
      cmp	rax,loc_task_thread
      jnz	_err
_index_c0 med_sours_pnt,_med
_index0 med_sour_dests_pos,rdx,_dest_num
_index1 med_sour_dests_pos,rdx,_dest_num,_task
      mov	rcx,[_med + dsc_med_sour_qty]
;find link already in task
_l0:
      mov	rdx,_ic0
      _neg	rdx
      jc	_l1
      mov	rax,_i0
      _neg	rax
      jc	_l1
      _neg	eax
      jnc	_err ;already in task
_l1:
      loop	_l0
      xor	rbx,rbx
      mov	rcx,[_med + dsc_med_sour_qty]
;set links to task
_l2:
      mov	rdx,_ic0
      _neg	rdx
      jc	_l3
      mov	rax,_i0
      _neg	rax
      jc	_l3
      _neg	rax
      jnc	_l3
      shr	rax,32
      mov	_i1,eax
      inc	rbx
_l3:
      loop	_l2
      _mov	_dest_num,rbx
_e _err,_end
}

;-----
macro _med_task_posok _pos, _newpos, _zero {
      mov	rax,r#_newpos
      shr	r#_newpos,32
      cmp	e#_newpos,eax
      jz	_zero
      mov	e#_newpos,eax
      shl	rax,32
      mov	eax,-1
      mov	_pos,rax
}
macro _med_task_sour_fin _med, _sour_num, _flag {
;direct, local_thread, wake all link(dest) aviable > dist_wake
local _err,_end,_l0,_l00,_l1,_l2
_regs rax,rbx,rcx,rdx,rsi,r8,r9
_parm_set _med
_parm_set _sour_num
_med_task_sour_prep _med, rsi, _sour_num, _err
      jc	_err ;non task
if _flag eq 0
_med_task_posok [rsi + dsc_med_sour_pos],dx,_end
;wake up
      mov	r8,[rsi + dsc_med_sour_dist_wake]
      _neg	r8
      jc	_l2
      mov	rbx,[rsi + dsc_med_sour_step_qty]
      mov	rcx,[_med + dsc_med_dest_qty]
; dest[all] ->:-> sour_pos
_l0:
      mov	r9,rcx
      dec	r9
_med_dist_dest rsi, rax, r9, _l1, 1, _set ;rdx - sour_pos
      cmp	rax,r8
      jc	_l1
_index1 med_dest_thrd,r9,_med,,r9
      xor	rax,rax
      mov	eax,_i1 ;thread of dest
      mov	r9,[main]
_index1 main_thrd_event,r9,rax
      _neg	_i1,_s1
      jc	_l1
      _push	rdx,rcx,rbx,r8,rsi,_med
      mov	rbx,_i1
invoke SetEvent,rbx
      _pop	rdx,rcx,rbx,r8,rsi,_med
      jmp      _l1
_l00:
      jmp      _l0
_l1:
      loop     _l00
_l2:
else
      mov	dword [rsi + dsc_med_sour_pos_task],-1
end if
_e _err,_end
}
macro _med_task_sour_finish _med, _dest_num {
_med_task_sour_fin _med, _dest_num, 0
}
macro _med_task_sour_abort _med, _dest_num {
_med_task_sour_fin _med, _dest_num, 1
}

macro _med_task_dest_testwake _med, _med_sour, _dest_num {
;direct, local_thread, wake all link(dest) aviable > dist_wake
local _end
_regs rax,rbx,rcx,rdx,r8
_parm_set _med
_parm_set _med_sour
_parm_set _dest_num
      cmp	[_med_sour + dsc_med_sour_dest_last],_dest_num
      stc
      jnz	_end
;wake up
      mov	rdx,[_med_sour + dsc_med_sour_pos]
      _neg	rdx
      jc	_end
      _neg	edx
      jnc	_end ;already task
      mov	rbx,[_med_sour + dsc_med_sour_step_qty]
_med_dist_sour_findlast _med,rsi, r8
      jc	_end
      cmp	r8,[_med_sour + dsc_med_sour_dist_wake]
      jc	_l1
      mov	rax,[_med_sour + dsc_med_sour_thrd] ;thread of sour
      _neg	rax
      jc	_end ;non thread
      mov	rdx,[main]
_index1 main_thrd_event,rdx,rax
      _neg	_i1,_s1
      jc	_end
      _push	 rcx,_dest_num,_med
      mov	rbx,_i1
invoke SetEvent,rbx
      _pop	 rcx,_dest_num,_med
      clc
_end:
}

macro _med_task_dest_fin _med, _dest_num, _flag {
;direct, local_thread, retest and wake all sour on link
local _err,_end,_l0,_l00,_l1
_regs rax,rdx,rcx,rbx,rsi,r8
_parm_set _med
_parm_set _dest_num
_index0 med_dest_thrd,rdx,_dest_num,,_med
      xor	rax,rax
      mov	eax,_i0
      cmp	rax,loc_task_thread
      jnz	_err
_index_c0 med_sours_pnt,_med
_index0 med_sour_dests_pos,rsi,_dest_num
_index1 med_sour_dests_pos,rsi,_dest_num,_task
      mov	rcx,[_med + dsc_med_sour_qty]
_l0:
      mov	rsi,_ic0
      _neg	rsi
      jc	_l1
      mov	rdx,_i0
      _neg	rdx
      jc	_l1
      _neg	edx
      jc	_l1 ;non task
if _flag eq 0
_med_task_posok _i0,dx,_l1
_med_task_dest_testwake _med, rsi, _dest_num
else
      mov	_i1,-1
end if
      jmp	_l1
_l00:
      jmp	_l0
_l1:
      loop	_l00
_e _err,_end
}

macro _med_task_dest_finish _med, _dest_num {
_med_task_dest_fin _med, _dest_num, 0
}
macro _med_task_dest_abort _med, _dest_num {
_med_task_dest_fin _med, _dest_num, 1
}

;-----------------------------------------------
macro _med_task_sour_ask _med,_sour_num,_adr,_size {
local _err,_end,_l0
_regs rax,rcx,rbx,rdx,rsi,r8,r9
_parm_set _sour_num
_med_task_sour_prep _med, rsi, _sour_num, _err
      jc	_err ;non task
      xor	rax,rax
      mov	eax,edx
      mov	rdx,rax
      mov	rbx,[rsi + dsc_med_sour_step_qty]
_med_dist_sour _med,rsi, r8, _err, 1
      jc	_err
      or	r8,r8
      jz	_end
      sub	rbx,rdx
      cmp	r8,rbx
      jc	_l0
      mov	r8,rbx
_l0:
      mov	rax,[_med + dsc_med_step_size]
      mul	rdx
      add	rax,[rsi + dsc_med_sour_buf_pnt]
      _mov	_adr,rax
_e _err,_end
      _mov	_size,r8
}
macro _med_task_sour_roger _med,_sour_num, _size {
local _err,_end,_l0
_regs rax,rcx,rbx,rdx,rsi
_parm_set _sour_num
_med_task_sour_prep _med, rsi, _sour_num, _err
      jc	_err ;non task
      mov	rbx,[rsi + dsc_med_sour_step_qty]
      _mov	rax,_size
      add	edx,eax
      cmp	edx,ebx
      jc	_l0
      sub	edx,ebx
_l0:
      mov	[rsi + dsc_med_sour_pos_task],edx
_e _err,_end
}

macro _med_task_dest_ask _med, _med_sour, _dest_num,_adr,_size {
local _err,_end,_l0,_l1,_l2,_l3,_l4,_l5
_regs rax,rcx,rbx,rdx,r8,r9
_parm_set _dest_num
_parm_set _med_sour
_parm_set _med
_index0 med_dest_thrd,rdx,_dest_num,,_med
      xor	rax,rax
      mov	eax,_i0
      cmp	rax,loc_task_thread
      jnz	_err

      mov	rcx,[_med + dsc_med_sour_qty]
_index0 med_dest_sour,rdx,_med,,_dest_num
      xor	r8,r8
      mov	r9,r8 ;r9==0 - sour is current
      mov	r8d,_i0
      _neg	r8d
      jnc	_l0
;non current sour - begin again
      xor	r8,r8
      inc	r9
_index0 med_sours_pnt,_med,r8
_l0:
      mov	_med_sour,_i0
      _neg	_med_sour
      jc	_l5
_med_dist_dest _med_sour, rax,_dest_num, _l5,0,_task
      or	rax,rax
      jz	_l1
      or	r9,r9
      jz	_l3 ;remain same sour
      cmp	rax,[_med_sour + dsc_med_sour_dist_dest]
      jnc	_l3
_l1:
      _neg8	[_med_sour + dsc_med_sour_thrd]
      jnc	_l5
_index1 med_sour_dests_pos,_med_sour,_dest_num
      mov	_i1,-1 ;remove empty dest:sour if sour non threaded
_l5:
      inc	r8
      cmp	r8,[_med + dsc_med_sour_qty]
      jc	_l2
      xor	r8,r8
_l2:
      inc	r9
      loop	_l0
      xor	_size,_size
      jmp	_end
_l3:
_index0 med_dest_sour,rcx,_med,,_dest_num
      mov	_i0,r8d
      mov	rcx,[_med_sour + dsc_med_sour_step_qty]
      sub	rcx,rdx
      cmp	rax,rcx
      jc	_l4
      mov	rax,rcx
_l4:
      mov	_size,rax
      mov	rax,[_med + dsc_med_step_size]
      mul	rdx
      add	rax,[_med_sour + dsc_med_sour_buf_pnt]
      _mov	_adr,rax
_e _err,_end
}
macro _med_task_dest_roger _med, _med_sour, _dest_num,_size {
local _err,_end,_l0,_l1,_l2,_l3,_l4
_regs rax,rcx,rbx,rdx
_parm_set _dest_num
_parm_set _med_sour
_parm_set _med
_parm_set _size
_index0 med_dest_thrd,rdx,_dest_num,,_med
      xor	rax,rax
      mov	eax,_i0
      cmp	rax,loc_task_thread
      jnz	_err
_index0 med_sour_dests_pos,_med_sour,_dest_num
_index1 med_sour_dests_pos,_med_sour,_dest_num,_task
      mov      rdx,_i0
      _neg     rdx
      jc       _err
      _neg     edx
      jc       _err
      mov	rbx,[_med_sour + dsc_med_sour_step_qty]
      _mov	rax,_size
      add	edx,eax
      cmp	edx,ebx
      jc	_l0
      sub	edx,ebx
_l0:
      mov	_i1,edx
_e _err,_end
}

;ask:
;(ret:parm0 == -1) -> error
med_api_thread_create	 equ 0
med_api_thread_proc	 equ dsc_main_thrd_api_parm0
med_api_thread_num	 equ dsc_main_thrd_api_parm0
med_api_thread_destroy	 equ 1

med_api_medium_create	 equ 2
med_api_medium_step_size equ dsc_main_thrd_api_parm0
med_api_medium_sours_qty equ dsc_main_thrd_api_parm1
med_api_medium_dests_qty equ dsc_main_thrd_api_parm2
med_api_medium_destroy	 equ 3
med_api_medium_num	 equ dsc_main_thrd_api_parm0

med_api_source_create	 equ 4
med_api_source_med_num	 equ dsc_main_thrd_api_parm0
med_api_source_step_num  equ dsc_main_thrd_api_parm1
med_api_source_handl	 equ dsc_main_thrd_api_parm2
med_api_source_dist_wake equ dsc_main_thrd_api_parm3
med_api_source_dist_dest equ dsc_main_thrd_api_parm4
med_api_source_num	 equ dsc_main_thrd_api_parm0

med_api_destination_set     equ 5
med_api_destination_med_num equ dsc_main_thrd_api_parm0
med_api_destination_handl   equ dsc_main_thrd_api_parm1

macro proc_med_sys {
local _ls,_l0,_l1,_l_1,_l2,_l3,_l4,_l5,_l6,_l7,_l_7,_l8,_l9,_l_9,_l9_,_l10,_l11
local _la00,_la01,_la02,_la03,_la04,_la1,_la2,_la3,_la4
_med_startup:
;r11 - memsize, r12 - med_num, r13 - thrd_num
      push	r12
      push	r13
_mem_init r11
      pop	r13
      pop	r12
      jc	_ls
_main_create r12,r13
      jc	_ls
m_main_thread_set 0,_med_0thread,0
      jc	_ls

invoke CreateEvent, NULL, TRUE, FALSE, NULL
      or	rax,rax
      stc
      jz       _ls
      mov      rdx,[main]
_index0 main_thrd_event,rdx,1
      mov	_i0,rax
      mov	loc_task_thread,1
      clc
_ls:
      retn


_med_0thread:
__stack equ 0
      mov	[thread],-1


_l0:
      mov	rbx,[main]
      mov	rbx,[rbx + dsc_main_thrd_event]
invoke ResetEvent,rbx
      mov	rbx,[main]
      mov	rcx,[rbx + dsc_main_thrd_qty]
      mov	rbx,[rbx + dsc_main_thrd_api_pnt]
      mov	rax,dsc_main_thrd_api_b
      mul	rcx
      add	rbx,rax
      mov	[med_sys0],0
_l1:
      sub	rbx,dsc_main_thrd_api_b
      mov	rax,[rbx + dsc_main_thrd_api_stat]
      _neg	rax
      jc	_l2
      _push	rcx,rbx
      mov	rdx,rcx
      dec	rdx
;-------------------
      cmp	rax,med_api_thread_create
      jnz	_la00
m_main_thread_set -1,[rbx + med_api_thread_proc],rdx
      mov	rax,rcx
      jmp	_l3
_la00:
      cmp	rax,med_api_thread_destroy
      jnz	_la01
      mov	rbx,[main]
_index_c0 main_thrd_event,rbx
      mov	_ic0,-1
      mov	rcx,[rbx + dsc_main_med_qty]
_index_c0 main_med,rbx,,rbx
_la1:
      mov	rdi,_ic0
      _neg	rdi
      jc	_la2
;on this medium
      _push	rcx,rbx
;all dest
      mov	rcx,[rdi + dsc_med_dest_qty]
_index_c1 med_dest_thrd,rbx,,rdi
_la3:
      cmp	edx,_ic1
      jnz	_la4
      _push	rcx,rbx,rdx
      dec	rcx
m_dest_unset rdi,rcx
      _pop	rcx,rbx,rdx
_la4:
      loop	_la3
;all sour
      mov	rcx,[rdi + dsc_med_sour_qty]
_index_c1 med_sours_pnt,rdi
_la5:
      mov	rsi,_ic1
      _neg	rsi
      jc	_la6
      cmp	rdx,[rsi + dsc_med_sour_thrd]
      jnz	_la6
      _push	rcx,rdx
      dec	rcx
m_sour_unset rdi,rcx
      _pop	rcx,rdx
_la6:
      loop	_la5
      _pop	rcx,rbx
_la2:
      loop	_la1
      _pop	rcx,rbx
      xor	rax,rax
      jmp	_l4

_la01:
      cmp	rax,med_api_medium_create
      jnz	_la02
m_med_create  -1, [rbx + med_api_medium_step_size], [rbx + med_api_medium_sours_qty], [rbx + med_api_medium_dests_qty]
      mov	rax,r11
      jmp	_l3

_la02:
      cmp	rax,med_api_medium_destroy
      jnz	_la03
m_med_del [rbx + med_api_medium_num]
      jmp	_l3

_la03:
      cmp	rax,med_api_source_create
      jnz	_la04
      mov	rax,[rbx + dsc_main_thrd_api_parm0]
      mov	rdi,[main]
_index0 main_med,r11,rax,,rdi
      mov	rdi,_i0
      _neg	rdi
      jc	_l3
m_sour_create rdi,-1,[rbx + med_api_source_step_num],rdx,[rbx + med_api_source_handl],[rbx + med_api_source_dist_wake],[rbx + med_api_source_dist_dest]
      mov	rax,r11
      jmp	_l3

_la04:
      cmp	rax,med_api_destination_set
      stc
      jnz	_l3
      mov	rax,[rbx + dsc_main_thrd_api_parm0]
      mov	rdi,[main]
_index0 main_med,r11,rax,,rdi
      mov	rdi,_i0
      _neg	rdi
      jc	_l3
m_dest_set rdi,-1,rdx,[rbx + med_api_destination_handl]
      mov	rax,r11
_l3:
      _pop	rcx,rbx
      jnc	_l4
      mov	rax,-1
_l4:
      mov	[rbx + dsc_main_thrd_api_parm0],rax
      mov	qword [rbx + dsc_main_thrd_api_stat],-1
      inc	[med_sys0]
      mov	rdx,[main]
_index_c0 main_thrd_event,rdx
      _push	rbx,rcx
      mov	rbx,_ic0
      _neg	rbx
      jc	_l5
invoke SetEvent,rbx
_l5:
      _pop	rbx,rcx
      jmp	_l2
_l_1:
      jmp	_l1
_l2:
      loop	_l_1

      cmp	[med_sys0],0
      jz	_l6
      jmp	_l0


_l6:
      mov	rbx,[main]
      mov	rcx,[rbx + dsc_main_med_qty]
_index_c0 main_med,rbx,,rbx
_l7:
      mov	rdi,_ic0
      _neg	rdi
      jc	_l8
      bt	qword [rdi + dsc_med_flags],1
      jnc	_l8
      _push	rbx,rcx
      mov	rcx,[rdi + dsc_med_sour_qty]
      xor	r11,r11
_index_c1 med_sours_pnt,rdi
_l9:
      _push	rcx
      mov	rsi,_ic1
      _neg	rsi
      jc	_l10
      _neg8	[rsi + dsc_med_sour_thrd]
      jnc	_l10
      inc	r11
      mov	rcx,[rdi + dsc_med_dest_qty]
_index_c2 med_sour_dests_pos,rsi
_l11:
      _neg	_ic2,_sc2
      jnc	_l10
      loop	_l11
      _pop	rcx
      _push	rcx
      mov	_ic1,-1
_mem_del rsi
      dec	r11
_l10:
      _pop	rcx
      jmp	_l_9
_l9_:
      jmp	_l9
_l_9:
      loop	_l9_
      _pop	rbx,rcx
      or	r11,r11
      jnz	_l8
      btr	qword [rdi + dsc_med_flags],1
      jmp	_l8
_l_7:
      jmp	_l7
_l8:
      loop	_l_7

      mov	rbx,[main]
      mov	rbx,[rbx + dsc_main_thrd_event]
invoke WaitForSingleObject,rbx,100
      jmp	_l0

}



macro proc_med {
_call m_main_thread_set, _main_thread_set,rcx,r11,r12
;               _thread_num,_thread_proc,_from_thread (thread_num == -1) -> next
;----
_call m_med_create,_med_create,r11,  r12,    r13,    r14
;             _med_num,_sour_num,_dest_num,_step_size
_call m_med_del,_med_del,r11
;             _med_num
;----
_call m_sour_create,_med_sour_create,rdi, r15,	    r11,      r12,    r13,	r9,	 r10
;ask (mem)                          _med, _sour_num,_step_num,_thread,_handler,_dist_wake,_dist_dest,
_call m_dest_set,_med_dest_set,rdi, r14, r12, r13
;m_dest_set    medium_pnt, dest_num, thread, handler
}





macro med_data {
mem	      dq    -1
main	      dq    -1
thread	      dq    -1
med_sys0      dq    -1
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
