import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import Sidebar from '../Sidebar';
import './GlassPicker.css'


export class GlassPicker extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },
            ids: '',
            send: [],
        };
    }



    componentDidMount() {
        var table2 = [];
        var tableIds = [];
        const receiver = {
            id: sessionStorage.getItem('idOpti'),
            package: {
                color: sessionStorage.getItem('colorpick'),
                type: sessionStorage.getItem('typepick'),
                thickness2: sessionStorage.getItem('thicknesspick'),
                owner: sessionStorage.getItem('ownerpick'),
            } ,
            user: {
                company: sessionStorage.getItem('company'),
            }

        }
        fetch(`api/Cut/Return_Glass_To_Cut`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {

                for (var i = 0; i < json.length; i++) {
                    if (sessionStorage.getItem('magazineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
                        table2.push({
                            length: json[i].length,
                            width: json[i].width,
                            thickness: json[i].hight,
                            color: json[i].color,
                            type: json[i].type,
                            owner: json[i].owner,
                            desk: json[i].desk,
                            choice: <input type="checkbox" id={'check' + i} className={i} onClick={(e) => { this.check(e.target.id, table2[e.target.className].id, i) }} />,
                            id: '',
                           
                        })
                    }
                    else {
                        table2.push({
                            length: json[i].length,
                            width: json[i].width,
                            thickness: json[i].hight,
                            color: json[i].color,
                            type: json[i].type,
                            owner: json[i].owner,
                            desk: json[i].desk,
                            id: '',
                        })

                    }
                };

                for (var k = 0; k < table2.length; k++) {
                    var amount = json[k].length;
                    for (var j = 0; j < amount; j++) {
                        table2[k].id = json[k].glass_Id
                    };
                }

                if (sessionStorage.getItem('magazineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
                    this.setState({
                        table: {
                            columns: [
                                {
                                    label: 'Length',
                                    field: 'length',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Width',
                                    field: 'width',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Thickness',
                                    field: 'thickness',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Color',
                                    field: 'color',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Type',
                                    field: 'type',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Owner',
                                    field: 'owner',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Ref number',
                                    field: 'id',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: '',
                                    field: 'choice',
                                   sort: 'asc',
                                    width: 150
                                },

                            ],
                            rows: table2
                        }
                    });
                }
                else {
                    this.setState({
                        table: {
                            columns: [
                                {
                                    label: 'Length',
                                    field: 'length',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Width',
                                    field: 'width',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Thickness',
                                    field: 'thickness',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Color',
                                    field: 'color',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Type',
                                    field: 'type',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Owner',
                                    field: 'owner',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'ref number',
                                    field: 'id',
                                    sort: 'asc',
                                    width: 150
                                },
                            ],
                            rows: table2
                        }
                    });
                }
            })
    };

    check(number, id, miejsce) {
        var checkBox = document.getElementById(number);
        var arr = this.state.send
        if (checkBox.checked == true) {
            //alert('dodane' + '' + number)
            arr.push(id)
            this.setState.send = arr


        } else {
            //alert('usunięte' + '' + number)
            const index = arr.indexOf(id);
            if (index > -1) {
                arr.splice(index, 1);
            }
            this.setState.send = arr

        }
    };


    pick = (event) => {
        event.preventDefault();
        sessionStorage.setItem('glass_id', this.state.send)
        this.props.history.push('/test')



       
        
    }

    render() {
        let xd = this.table();
        if (sessionStorage.getItem('cutManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div className="Glass_Picker">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Pick glass to cut </h1>
                    </div>
                    <div className="Glass_Picker_c">
                    
                        <button className="success_glass_p " onClick={this.pick}> Pick selected </button>
                    
                        { xd }
                    </div>
                </div>
            )

        }
        else {
            return (
                <div className="HomePageFail">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
    }






    table() {
        return (
            <MDBDataTableV5
                data={this.state.table}
                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={10}
                pagesAmount={10}
                searchTop
                materialSearch
                searchBottom={false}
                responsive
                bordered
                paginationLabel={["Previous", "Next"]}
                sortable
                // small
                theadTextWhite
                theadTextWhite
                className="table_corection"
            />



        )
    }



}
