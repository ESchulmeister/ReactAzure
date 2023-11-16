import React from 'react';


export const Footer = () => {

    let year = new Date().getFullYear();
  
    return (
	// eslint-disable-next-line
        <div className="footer cFooter">
            <footer >
                <hr className="custHr" />
                <p>Copyright  &copy; {year} - Dayton T. Brown Inc. All Rights Reserved.</p>
            </footer>
        </div>
    )
}


