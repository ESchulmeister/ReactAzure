
/*eslint unicode-bom: ["error", "always"]*/

import React from 'react';

export default function ThumbnailCell(cellData) {
    return (

        <div>
            <img src={cellData.data.ThumbnailUri} alt="" className="cellThumb" width="90" height="90"/>
        </div>
      
    );
}