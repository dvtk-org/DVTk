<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<xsl:template match="DvtSummaryResultsFile">
		<xsl:for-each select="//Activity">
			<xsl:if test="@Level[.='Error']">
				<Result>
					<message>
						<xsl:value-of select="."/>
					</message>
					<type>Error</type>
									<id>
				<xsl:value-of select="@Index"/>
				</id>
				</Result>
			</xsl:if>
			<xsl:if test="@Level[.='Warning']">
				<Result>
					<message>
						<xsl:value-of select="."/>
					</message>
					<type>Warning</type>
									<id>
				<xsl:value-of select="@Index"/>
				</id>
				</Result>
			</xsl:if>
		</xsl:for-each>
		<xsl:for-each select="//Message">
			<xsl:sort select="@Message"/>
			<Result>
				<message>
					<xsl:value-of select="Meaning"/>
				</message>
				<type>
					<xsl:value-of select="Type"/>
				</type>
				<id>
				<xsl:value-of select="@Index"/>
				</id>
			</Result>
		</xsl:for-each>
		<xsl:for-each select="//UserActivity">
			<xsl:if test="not(contains(.,'The thread'))">
			<xsl:if test="not(contains(.,'The script'))">
			<xsl:if test="@Level[.='Error']">
				<Result>
					<message>
						
						<xsl:value-of select="."/>
					</message>
					<type>Error</type>
									<id>
				<xsl:value-of select="@Index"/>
				</id>
				</Result>
			</xsl:if>
			<xsl:if test="@Level[.='Warning']">
				<Result>
					<message>
						<xsl:value-of select="."/>
					</message>
					<type>Warning</type>
									<id>
				<xsl:value-of select="@Index"/>
				</id>
				</Result>
			</xsl:if>
			</xsl:if>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>
