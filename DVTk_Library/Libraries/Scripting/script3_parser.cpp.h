typedef union {
	DIMSE_CMD_ENUM	commandField;
	unsigned long   hex;
	char			identifier[MAX_ID_LEN];
	int				integer;
	IOM_LEVEL_ENUM	iomLevel;
	char			*string_ptr;
//	char			string[MAX_STRING_LEN];
	UP_ENUM			userProvider;
	ATTR_VR_ENUM	vr;
	VALIDATION_CONTROL_FLAG_ENUM validationFlag;
} YYSTYPE;
#define	T_LANGUAGE	258
#define	T_RESET	259
#define	T_ALL	260
#define	T_WAREHOUSE	261
#define	T_ASSOCIATION	262
#define	T_RELATION	263
#define	T_EXECUTION_CONTEXT	264
#define	T_COMPARE	265
#define	T_COMPARE_NOT	266
#define	T_CONFIRM	267
#define	T_COPY	268
#define	T_CREATE	269
#define	T_DELAY	270
#define	T_DELETE	271
#define	T_DISPLAY	272
#define	T_ECHO	273
#define	T_POPULATE	274
#define	T_READ	275
#define	T_RECEIVE	276
#define	T_ROLE	277
#define	T_SEND	278
#define	T_SET	279
#define	T_SYSTEM	280
#define	T_TIME	281
#define	T_VALIDATE	282
#define	T_VERBOSE	283
#define	T_WRITE	284
#define	T_IMPORT	285
#define	T_EXPORT	286
#define	T_VALIDATION	287
#define	T_DEF_SQ_LENGTH	288
#define	T_ADD_GROUP_LENGTH	289
#define	T_STRICT	290
#define	T_APPL_ENTITY	291
#define	T_ASSOCIATE_RQ	292
#define	T_ASSOCIATE_AC	293
#define	T_ASSOCIATE_RJ	294
#define	T_RELEASE_RQ	295
#define	T_RELEASE_RP	296
#define	T_ABORT_RQ	297
#define	T_PROT_VER	298
#define	T_CALLED_AE	299
#define	T_CALLING_AE	300
#define	T_APPL_CTX	301
#define	T_PRES_CTX	302
#define	T_MAX_LEN	303
#define	T_IMPL_CLASS	304
#define	T_IMPL_VER	305
#define	T_SOP_EXTEND_NEG	306
#define	T_SCPSCU_ROLE	307
#define	T_ASYNC_WINDOW	308
#define	T_USER_ID_NEG	309
#define	T_RESULT	310
#define	T_SOURCE	311
#define	T_REASON	312
#define	T_DEFINED_LENGTH	313
#define	T_AUTOSET	314
#define	T_FILEHEAD	315
#define	T_FILETAIL	316
#define	T_FILE_PREAMBLE	317
#define	T_DICOM_PREFIX	318
#define	T_TRANSFER_SYNTAX	319
#define	T_DATASET_TRAILING_PADDING	320
#define	T_SECTOR_SIZE	321
#define	T_PADDING_VALUE	322
#define	T_SQ	323
#define	T_OPEN_BRACKET	324
#define	T_CLOSE_BRACKET	325
#define	T_YES	326
#define	T_NO	327
#define	T_ON	328
#define	T_OFF	329
#define	T_OR	330
#define	T_AND	331
#define	COMMANDFIELD	332
#define	HEXADECIMAL	333
#define	IDENTIFIER	334
#define	INTEGER	335
#define	VALIDATIONFLAG	336
#define	IOMLEVEL	337
#define	STRING	338
#define	USERPROVIDER	339
#define	VR	340


extern YYSTYPE script3lval;
