<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:variable name="skipped">
		<xsl:value-of select="document(/Collection/FileName)/Nodes/skipped"/>
	</xsl:variable>
	<xsl:variable name="filenameresultsxml">
		<xsl:value-of select="/Collection/FileName"/>
	</xsl:variable>
	<xsl:template match="Collection">
	<xsl:param name="resultfile"/>
	<collection>
	<Directory><xsl:value-of select="/Collection/Directory"/></Directory>
<skipped><xsl:value-of select="$skipped"/></skipped>
<xsl:for-each select="Role">
<Role>	
<Rolename><xsl:value-of select="Rolename"/></Rolename>
<xsl:for-each select="Sop">
<Sop>
<Sopname><xsl:value-of select="Sopname"/></Sopname>
<xsl:for-each select="file">
<file>
<filename><xsl:value-of select="filename"/></filename>
<xsl:if test="count(result)>0">
<Executed>Y</Executed>
<isexecuted/>
<xsl:for-each select="result">
				<xsl:call-template name="allresultsfiles">
				<xsl:with-param name="resultfile"><xsl:value-of select="."/></xsl:with-param>
				</xsl:call-template>
</xsl:for-each>
</xsl:if>
<xsl:if test="count(result)=0">
<Executed>N</Executed>
</xsl:if>
	
</file>
</xsl:for-each>
</Sop>	
</xsl:for-each>
</Role>	
</xsl:for-each>

</collection>
</xsl:template>

		<xsl:template name="allresultsfiles">
<xsl:param name="resultfile"/>
<xsl:param name="result"/>
<xsl:for-each select="document(concat(substring-before($filenameresultsxml,'.pdvt.xml'),'3.xml'))//Resultfile[.=$resultfile]">
				<xsl:call-template name="allresults">
				<xsl:with-param name="result"><xsl:value-of select="../../@Name"/></xsl:with-param>
				<xsl:with-param name="resultcheck1">ALL * <xsl:value-of select="../../@Name"/></xsl:with-param>
				<xsl:with-param name="resultcheck2"><xsl:value-of select="$resultfile"/> * <xsl:value-of select="../../@Name"/></xsl:with-param>
				</xsl:call-template>
		</xsl:for-each>

<xsl:for-each select="document($filenameresultsxml)/Nodes/added/Message">
		<xsl:if test="substring-before(Name, ' * ')=$resultfile">
		<problem><xsl:value-of select="substring-after(Name, ' * ')"/></problem>
		<xsl:if test="not(PRnr='')"><prnr><xsl:value-of select="PRnr"/><br/></prnr></xsl:if>
					<pr>
		<prnr><xsl:value-of select="PRnr"/></prnr>
		<com><xsl:value-of select="Comment"/></com>
		</pr>
		</xsl:if>
		</xsl:for-each>
		
	</xsl:template>
	
	<xsl:template name="allresults">
<xsl:param name="resultcheck1"/>
<xsl:param name="resultcheck2"/>
<xsl:param name="result"/>	
		<xsl:if test="not(contains($skipped,$resultcheck1))"><xsl:if test="not(contains($skipped,$resultcheck2))"><problem><xsl:value-of select="$result"/></problem>

<xsl:for-each select="document($filenameresultsxml)/Nodes/Problem/Message[Name=$result]">
		<xsl:if test="not(PRnr='')"><prnr><xsl:value-of select="PRnr"/><br/></prnr></xsl:if>
	      <xsl:if test="not(contains($skipped,$resultcheck1))">	  <xsl:if test="not(contains($skipped,$resultcheck2))">	<pr>
		<prnr><xsl:value-of select="PRnr"/></prnr>
<com><xsl:value-of select="Comment"/></com>
		</pr></xsl:if></xsl:if>
		</xsl:for-each>

<xsl:for-each select="document($filenameresultsxml)/Nodes/Problem/Message[Name=$resultcheck1]">
		<xsl:if test="not(PRnr='')"><prnr><xsl:value-of select="PRnr"/><br/></prnr></xsl:if>
	      <xsl:if test="not(contains($skipped,$resultcheck1))">		<pr>
		<prnr><xsl:value-of select="PRnr"/></prnr>
<com><xsl:value-of select="Comment"/></com>
		</pr></xsl:if>
		</xsl:for-each>

<xsl:for-each select="document($filenameresultsxml)/Nodes/Problem/Message[Name=$resultcheck2]">
		<xsl:if test="not(PRnr='')"><prnr><xsl:value-of select="PRnr"/><br/></prnr></xsl:if>
	      <xsl:if test="not(contains($skipped,$resultcheck2))">		<pr>
		<prnr><xsl:value-of select="PRnr"/></prnr>
<com><xsl:value-of select="Comment"/></com>
		</pr></xsl:if>
		</xsl:for-each>

<xsl:for-each select="document($filenameresultsxml)/Nodes/skipped/Message[Name=$resultcheck1]">
		<xsl:if test="not(PRnr='')"><prnr><xsl:value-of select="PRnr"/><br/></prnr></xsl:if>
		</xsl:for-each>
		<xsl:for-each select="document($filenameresultsxml)/Nodes/skipped/Message[Name=$resultcheck2]">
		<xsl:if test="not(PRnr='')"><prnr><xsl:value-of select="PRnr"/><br/></prnr></xsl:if>
		</xsl:for-each>
</xsl:if></xsl:if>

</xsl:template>

	<xsl:template name="allresultsproblems">
<xsl:param name="result"/>	
<xsl:for-each select="document($filenameresultsxml)/Nodes/Problem/Message[Name=$result]">
		<prnr><xsl:value-of select="PRnr"/><br/></prnr>
		</xsl:for-each>
</xsl:template>

</xsl:stylesheet>
