import React from 'react';
import {Link} from "react-router-dom";

function SideBar({NavLinks}) {
    return ( 
        <div className="side-bar">
            <ul className="side-bar-list">
                
                {NavLinks.map((link, index) =>(
                    <li className="side-bar-item" key={index}><Link to={link.path} onClick={link.func}>{link.title}</Link></li>
                ))}
            </ul>
        </div>
     );
}

export default SideBar;
