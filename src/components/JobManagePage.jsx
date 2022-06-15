import React from 'react';
import SideBar from './SideBar';
import ListItem from './StudentItem';
import PageBanner from './PageBanner';

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

function JobManagePage() {
    return ( 
        <div className="job-manage-page">
            <PageBanner title="Manage Job"/>
            <SideBar />
            <div className="job-list">
                <ul className="list">
                {list.map(item => (
                    <li key={item.id}>
                        <ListItem item={item}/>
                    </li>
                ))}
                </ul>
            </div>
        </div>
    );
}

export default JobManagePage;