import React, { useState, useEffect } from 'react';
import { getGroup } from '../../api/Api'
import SideBar from '../SideBar';
import { Outlet } from 'react-router-dom';
import PageBanner from '../PageBanner';

const list = [
    {
      id: 'a',
      firstname: 'Robin',
      lastname: 'Wieruch',
      year: 1988,
    },
    {
      id: 'b',
      firstname: 'Dave',
      lastname: 'Davidds',
      year: 1990,
    },
];

function InstructorPage() {
    const [title, setTitle] = useState("Instructor Page");

    async function loadData () {
        var id = localStorage.getItem('userId');
        var group = await getGroup(id);
        localStorage.setItem('grpId', group.id);
    }

    useEffect(() => {
        loadData();
    }, []);

    const NavLinks = [
        {   
            id: 1,
            title: 'Manage Jobs',
            path: "/instructor/jobs",
            func: () => setTitle("View Jobs")
        },
        {   
            id: 2,
            title: 'Manage Students',
            path: "/instructor/students",
            func: () => setTitle("Manage Students")
        },
        {   
            id: 3,
            title: 'Manage Registrations',
            path: "/instructor/registers",
            func: () => setTitle("Manage Registrations")
        }
    ]

    return ( 
        <div className="instructor-page">
            <PageBanner title={title}/>
            <SideBar NavLinks={NavLinks}/>
            <div className="right">
            <Outlet/>
            </div>
        </div>
    );
}

export default InstructorPage;