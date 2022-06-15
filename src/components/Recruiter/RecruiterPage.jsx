import React, { useState, useEffect } from 'react';
import {getRec} from '../../api/Api'
import SideBar from '../SideBar';
import { Outlet } from 'react-router-dom';
import PageBanner from '../PageBanner';



function RecruiterPage() {
    const [title, setTitle] = useState("Recruiter Page");

    async function loadData () {
        var id = localStorage.getItem('userId');
        var recruiter = await getRec(id);
        var comId = recruiter.companyId;
        localStorage.setItem('comId', comId);
    }

    useEffect(() => {
        loadData();
    }, []);

    const NavLinks = [
        {   
            id: 1,
            title: 'Jobs',
            path: "/recruiter/jobs",
            func: () => setTitle("Jobs")
        },
        {   
            id: 2,
            title: 'Applicants',
            path: "/recruiter/students",
            func: () => setTitle("Applicants")
        },
    ]

    return ( 
        <div className="recruiter-page">
            <PageBanner title={title}/>
            <SideBar NavLinks={NavLinks}/>
            <div className="right">
            <Outlet/>
            </div>
        </div>
    );
}

export default RecruiterPage;
