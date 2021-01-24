import React, { Component } from 'react';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import './cutMachineEdit.css';
import Sidebar from '../Sidebar';

export class CutMachineEdit extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },
        };
    }

    componentDidMount() {
        var table2 = [];
        fetch(`api/Machine/Return_All_Type`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json.length);
                console.log(json);
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        number: i + 1,
                        type: json[i],
                        edit:
                            <Link to="/machinetypeedit"><button className="info_t" id={i}
                                onClick={
                                    (e) => {
                                        console.log(e.target.id);
                                        sessionStorage.setItem('machinetype', json[e.target.id]);
                                    }
                                }>Edytuj</button>
                            </Link>
                    })
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Nr',
                                field: 'number',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Typ',
                                field: 'type',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Edycja',
                                field: 'edit',
                                width: 150
                            },
                        ],
                        rows: table2
                    }
                });
            })
    };

    backToHome = (event) => {
        this.props.history.push('/controlpanel');
    }

    addTypeMachine = (event) => {
        this.props.history.push('/add_typemachine')
    }

    editTypeMachine = (event) => {
        this.props.history.push('/machinetypeedit')
    }


    
    table() {
        return (
            <MDBDataTableV5


                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={15}
                pagesAmount={10}
                data={this.state.table}
                searchTop


                materialSearch
                searchBottom={false}
                // barReverse
                //  pagingTop
                // scrollX
                // scrollY
                responsive
                // maxHeight="35vh"
                bordered



                //   maxHeight="20vh"
                // borderless
                // btn
                // dark


                //maxHeight="400px"

                // paginationLabel={["<", ">"]}

                sortable


                // small
                // tego w ciemnym trybie nie ruszać/ striped/
                // theadColor="indigo"
                theadTextWhite
                // theadColor="indigo"
                theadTextWhite
                // barReverse
                className="User_table"
                // noBottomColumns
                sortable
            //info={false}


            //   autoWidth


            />
        )
    }


    render() {
        let typeTable = this.table();
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Zaloguj się, aby usyskać dostęp!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Logowanie</button>
                </div>
            );
        }
        else {
            return (
                <div className="cutMEdit">
                    <Sidebar />
                    <div className="cut_machine_edit">
                        <div className="nav_machine_e">
                        </div>
                        <div className="conceiner_machine_e">
                            <button type="button" className="success_cm_edit" onClick={this.addTypeMachine}>Dodaj typ</button>
                            {typeTable}
                        </div>
                    </div>
                </div>
            )
        }
    }


}