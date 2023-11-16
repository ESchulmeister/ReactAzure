//eslint unicode-bom: ["error", "always"]

import React from 'react';

import { getUser } from './util/Utils'; 

import imgIndex from '../images/index.png';


export default function Home() {

    const user = getUser();

 
    if (user === undefined || user == null) {    //nothing to render; 
        return null;
    }

    return (
         


        <table className="homeCenter">
            <tbody>
              
                <tr>
                    <td>
                        <img src={imgIndex} alt="" />
                       
                    </td>
                </tr>
            </tbody>
        </table>

    );
};
