<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:template match="/Contratos">
        <xsl:apply-templates select="Contrato" />
    </xsl:template>

    <xsl:template match="Contrato">
        <table width="100%" border="1">
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
        </table>
    </xsl:template>
    
</xsl:stylesheet>