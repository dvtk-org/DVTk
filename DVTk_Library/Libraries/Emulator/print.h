// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2009 DVTk
// ------------------------------------------------------
// This file is part of DVTk.
//
// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License as published by the Free Software Foundation; either version 3.0
// of the License, or (at your option) any later version. 
// 
// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
// General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// library; if not, see <http://www.gnu.org/licenses/>

//*****************************************************************************
//  DESCRIPTION     :	Printer emulation classes.
//*****************************************************************************
#ifndef PRINT_H
#define PRINT_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Idicom.h"			// Dicom component interface


#define MYPRINTER		PRINTER_CLASS::instance()
#define MYPRINTQUEUE	PRINT_QUEUE_CLASS::instance()

//*****************************************************************************
//  FORWARD DECLARATIONS
//*****************************************************************************
class BASIC_FILM_SESSION_CLASS;
class EMULATOR_SESSION_CLASS;
class LOG_CLASS;

//>>***************************************************************************

class BASE_SOP_CLASS 

//  DESCRIPTION     : Abstract base SOP class for various "Print" objects
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
protected:
	string					sopClassUidM;
	string					sopInstanceUidM;
	DCM_DATASET_CLASS		*datasetM_ptr;
	LOG_CLASS				*loggerM_ptr;

public:
	virtual ~BASE_SOP_CLASS() = 0;

	string getSopClassUid() 
		{ return sopClassUidM; }

	string getSopInstanceUid() 
		{ return sopInstanceUidM; }

	DCM_DATASET_CLASS *get() 
		{ return datasetM_ptr; }

	virtual UINT16 set(DCM_DATASET_CLASS*);

	void setLogger(LOG_CLASS *logger_ptr)
		{ loggerM_ptr = logger_ptr; }

	LOG_CLASS *getLogger()
		{ return loggerM_ptr; }
};


//>>***************************************************************************

class IMAGE_BOX_CLASS : public BASE_SOP_CLASS 

//  DESCRIPTION     : Image box class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	IMAGE_BOX_CLASS(string, string);

	~IMAGE_BOX_CLASS();

	UINT16 set(DCM_DATASET_CLASS*);
};


//>>***************************************************************************

class IMAGE_OVERLAY_CLASS : public BASE_SOP_CLASS 

//  DESCRIPTION     : Image overlay class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	IMAGE_OVERLAY_CLASS(string, DCM_DATASET_CLASS*);

	~IMAGE_OVERLAY_CLASS();
};


//>>***************************************************************************

class ANNOTATION_BOX_CLASS : public BASE_SOP_CLASS 

//  DESCRIPTION     : Annotation box class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	ANNOTATION_BOX_CLASS(string);

	~ANNOTATION_BOX_CLASS();
};


//>>***************************************************************************

class PRESENTATION_LUT_CLASS : public BASE_SOP_CLASS 

//  DESCRIPTION     : Presentation LUT class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	PRESENTATION_LUT_CLASS(string, DCM_DATASET_CLASS*);

	~PRESENTATION_LUT_CLASS();
};


//>>***************************************************************************

class VOI_LUT_BOX_CLASS : public BASE_SOP_CLASS 

//  DESCRIPTION     : Voi LUT box class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	VOI_LUT_BOX_CLASS(string, DCM_DATASET_CLASS*);

	~VOI_LUT_BOX_CLASS();
};


//>>***************************************************************************

class BASIC_FILM_BOX_CLASS : public BASE_SOP_CLASS 

//  DESCRIPTION     : Basic film box class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	EMULATOR_SESSION_CLASS			*sessionM_ptr;
	string							imageSopInstanceUidM;
	int								filmNumberM;
	int								imageNumberM;
	ARRAY<IMAGE_BOX_CLASS*>			imageBoxM;
	ARRAY<ANNOTATION_BOX_CLASS*>	annotationBoxM;

	void makeImageSopInstanceUid();

public:
	BASIC_FILM_BOX_CLASS(EMULATOR_SESSION_CLASS*, LOG_CLASS*, int, string);
	BASIC_FILM_BOX_CLASS(EMULATOR_SESSION_CLASS*, LOG_CLASS*, int, string, DCM_DATASET_CLASS*);

	~BASIC_FILM_BOX_CLASS();

	int noImageBoxes() 
		{ return imageBoxM.getSize(); }

	void addImageBox(IMAGE_BOX_CLASS *imageBox_ptr)
		{ imageBoxM.add(imageBox_ptr); }

	int isImageBox(string);

	bool removeImageBox(UINT);

	IMAGE_BOX_CLASS *getImageBox(UINT);

	int noAnnotationBoxes()
		{ return annotationBoxM.getSize(); }

	void addAnnotationBox(ANNOTATION_BOX_CLASS *annotationBox_ptr)
		{ annotationBoxM.add(annotationBox_ptr); }

	int isAnnotationBox(string);

	bool removeAnnotationBox(UINT);

	ANNOTATION_BOX_CLASS *getAnnotationBox(UINT);

	UINT16 create(char*, DCM_DATASET_CLASS**);

	UINT16 action(BASIC_FILM_SESSION_CLASS*, int, bool, DCM_DATASET_CLASS**);
};


//>>***************************************************************************

class BASIC_FILM_SESSION_CLASS : public BASE_SOP_CLASS 

//  DESCRIPTION     : Basic film session class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	EMULATOR_SESSION_CLASS			*sessionM_ptr;
	ARRAY<BASIC_FILM_BOX_CLASS*>	filmBoxM;
	ARRAY<IMAGE_OVERLAY_CLASS*>		overlayM;
	ARRAY<VOI_LUT_BOX_CLASS*>		voiLutM;

public:
	BASIC_FILM_SESSION_CLASS(EMULATOR_SESSION_CLASS*, LOG_CLASS*, string);
	BASIC_FILM_SESSION_CLASS(EMULATOR_SESSION_CLASS*, LOG_CLASS*, string, DCM_DATASET_CLASS*);

	~BASIC_FILM_SESSION_CLASS();

	int noFilmBoxes()
		{ return filmBoxM.getSize(); }

	void addFilmBox(BASIC_FILM_BOX_CLASS *filmBox_ptr)
		{ filmBoxM.add(filmBox_ptr); }

	int isFilmBox(string);

	bool removeFilmBox(UINT);

	BASIC_FILM_BOX_CLASS *getFilmBox(UINT);

	int noImageOverlays()
		{ return overlayM.getSize(); }

	void addImageOverlay(IMAGE_OVERLAY_CLASS *overlay_ptr)
		{ overlayM.add(overlay_ptr); }

	int isImageOverlay(string);

	bool removeImageOverlay(UINT);

	IMAGE_OVERLAY_CLASS *getImageOverlay(UINT);

	int noVoiLutBoxes()
		{ return voiLutM.getSize(); }

	void addVoiLutBox(VOI_LUT_BOX_CLASS *voiLut_ptr)
		{ voiLutM.add(voiLut_ptr); }

	int isVoiLutBox(string);

	bool removeVoiLutBox(UINT);

	VOI_LUT_BOX_CLASS *getVoiLutBox(UINT);

	UINT16 action(bool, DCM_DATASET_CLASS**);
};


//>>***************************************************************************

class PRINT_JOB_CLASS : public BASE_SOP_CLASS 

//  DESCRIPTION     : Print job class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	EMULATOR_SESSION_CLASS	*sessionM_ptr;

public:
	PRINT_JOB_CLASS(EMULATOR_SESSION_CLASS*, LOG_CLASS*);

	~PRINT_JOB_CLASS();

	void makeSopInstanceUid();
};


//>>***************************************************************************

class PRINT_QUEUE_CLASS

//  DESCRIPTION     : Print queue (Singleton) class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static PRINT_QUEUE_CLASS	*instanceM_ptr;		// Singleton
	int							printJobNumberM;
	ARRAY<PRINT_JOB_CLASS*>		printJobM;

	void initialise();

protected:
	PRINT_QUEUE_CLASS();

public:
	static PRINT_QUEUE_CLASS *instance();

	void cleanup();

	int nextPrintJobNo()
		{ return printJobNumberM++; }

	int noPrintJobs()
		{ return printJobM.getSize(); }

	void addPrintJob(PRINT_JOB_CLASS *printJob_ptr)
		{ printJobM.add(printJob_ptr); }

	int isPrintJob(string);

	bool removePrintJob(UINT);

	PRINT_JOB_CLASS *getPrintJob(UINT);

	UINT16 get(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS**);
};


//>>***************************************************************************

class FORMAT_DESC_CLASS

//  DESCRIPTION     : Format descriptor class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	char	*formatM_ptr;
	int		noBoxesM;

public:
	FORMAT_DESC_CLASS(char*, int);

	~FORMAT_DESC_CLASS();

	char *getFormat()
		{ return formatM_ptr; }

	int	getNoBoxes()
		{ return noBoxesM; }
};


//>>***************************************************************************

class PRINTER_CLASS

//  DESCRIPTION     : Printer (Singleton) class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static PRINTER_CLASS			*instanceM_ptr; // Singleton
    bool                            imageDisplayFormatDataLoadedM;
	char							statusM[CS_LENGTH + 1];
	char							statusInfoM[CS_LENGTH + 1];
	ARRAY<FORMAT_DESC_CLASS*>		imageDisplayFormatM;
	ARRAY<FORMAT_DESC_CLASS*>		annotationDisplayFormatIdM;
	ARRAY<PRESENTATION_LUT_CLASS*>	presentationLutM;

	void initialise();

	int addNumbers(char*);

	int multiplyNumbers(char*);

	int getNoImages(char*);

protected:
	PRINTER_CLASS();

public:
	static PRINTER_CLASS *instance();

	void cleanup();

	UINT16 get(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS**);

	void setStatus(char*);

	void setStatusInfo(char *);

	char *getStatus()
		{ return statusM; }

	char *getStatusInfo()
		{ return statusInfoM; }

	UINT getNoStatusInfoDTs();

	string getStatusInfoDT(UINT);

	char *getName()
		{ return "Emulator"; }

	char *getManufacturer()
		{ return MANUFACTURER; }

	char *getModelName()
		{ return MODEL_NAME; }

	char *getSerialNumber()
		{ return "123456"; }

	char *getSoftwareVersion()
		{ return IMPLEMENTATION_VERSION_NAME; }

	char *getCalibrationDate() 
		{ return DATE; }

	char *getCalibrationTime()
		{ return "141000"; }

	bool loadImageDisplayFormats(string filename);

	void addImageDisplayFormat(char*, int);

	void addAnnotationDisplayFormatId(char*, int);

	int getImageDisplayFormat(char*);

	int	getAnnotationDisplayFormatId(char*);

	void addPresentationLut(PRESENTATION_LUT_CLASS *presentationLut_ptr)
		{ presentationLutM.add(presentationLut_ptr); }

	int isPresentationLut(string);

	bool removePresentationLut(UINT);

	void removeAllPresentationLuts();
};

#endif /* PRINT_H */
