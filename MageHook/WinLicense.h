#pragma once

#define CODEREPLACE_START \
	__asm _emit 0xEB \
	__asm _emit 0x10 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \

#define CODEREPLACE_END \
	__asm _emit 0xEB \
	__asm _emit 0x10 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \
	__asm _emit 0x01 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \

#define ENCODE_START \
	__asm _emit 0xEB \
	__asm _emit 0x10 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \
	__asm _emit 0x04 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \

#define ENCODE_END \
	__asm _emit 0xEB \
	__asm _emit 0x10 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \
	__asm _emit 0x05 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \

#define CLEAR_START \
	__asm _emit 0xEB \
	__asm _emit 0x10 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \
	__asm _emit 0x06 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \

#define CLEAR_END \
	__asm _emit 0xEB \
	__asm _emit 0x15 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \
	__asm _emit 0x07 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x57 \
	__asm _emit 0x4C \
	__asm _emit 0x20 \
	__asm _emit 0x20 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00 \
	__asm _emit 0x00
