<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/Contratos">
    <xsl:for-each select="/Contratos/Contrato">
      <tr>
        <td>
          <b>
            <xsl:value-of select="CodigoContrato" />

          </b>
        </td>
        <td>
          <b>
            <xsl:value-of select="NombreContrato" />
          </b>
        </td>
      </tr>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
