typedef union {
	bool						boolean;
	DIMSE_CMD_ENUM				commandField;
    unsigned long				hex;
	int							integer;
    NAME_STRING  				string;
	ATTR_TYPE_ENUM				type;
	MOD_USAGE_ENUM				usage;
	ATTR_VAL_TYPE_ENUM			valueType;
	ATTR_VR_ENUM				vr;
	CONDITION_NODE_CLASS*		node_ptr;
	CONDITION_UNARY_NODE_CLASS*	unary_node_ptr;
	CONDITION_BINARY_NODE_CLASS* binary_node_ptr;
} YYSTYPE;
#define	T_SYSTEM	258
#define	T_DEFINE	259
#define	T_ENDDEFINE	260
#define	T_METASOPCLASS	261
#define	T_SOPCLASS	262
#define	T_MODULE	263
#define	T_ITEM	264
#define	T_MACRO	265
#define	T_INCLUDEITEM	266
#define	T_INCLUDEMACRO	267
#define	T_SQ	268
#define	T_AND	269
#define	T_OR	270
#define	T_NOT	271
#define	T_PRESENT	272
#define	T_VALUE	273
#define	T_EMPTY	274
#define	T_TRUE	275
#define	T_FALSE	276
#define	T_EQUAL	277
#define	T_LESS	278
#define	T_GREATER	279
#define	T_LESS_OR_EQUAL	280
#define	T_GREATER_OR_EQUAL	281
#define	T_WEAK	282
#define	T_WARN	283
#define	STRING	284
#define	INTEGER	285
#define	HEX	286
#define	COMMANDFIELD	287
#define	TYPE	288
#define	USAGE	289
#define	VALUETYPE	290
#define	VR	291


extern YYSTYPE definitionlval;
