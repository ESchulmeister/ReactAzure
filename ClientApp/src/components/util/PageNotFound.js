/*eslint unicode-bom: ["error", "always"]*/
import React from 'react';
import { getUser } from '../../components/util/Utils'; 


import imgQuestion from '../../images/question.png';

export default function PageNotFound() {


    const user = getUser();

    if (user === undefined || user == null) {    //nothing to render; 
        return null;
    }

    return (
        <div className="homeCenter">
            <tabble>
                <tbody>
                    <tr>
                        <td align="center">
                          <img id="imgQuestion" src={imgQuestion}
                                alt=""
                                className="questImg"
                            ></img>
                        </td>
                    </tr>

                    <tr>
                        <td align="center">
                            <p className="oLabel">404 - Page Not Found</p>
                        </td>
                    </tr>

                </tbody>


            </tabble>
  
            
        </div>
    )
}
