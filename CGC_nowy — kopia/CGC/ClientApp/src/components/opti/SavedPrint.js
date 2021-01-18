import React, { Component } from "react";
import './test.css';
import { ItemsTable } from '../saveProjectTables/ItemsTable'
import { GlassTableProject } from '../saveProjectTables/glassTableProject'
import Sidebar from '../Sidebar';

export class SavedPrint extends Component {
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            value2: '',
            table: [],
            pieces: [],
            glass_ids: []

        }
    }

    componentDidMount() {
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('orderId2'),
            },
            id: sessionStorage.getItem('cutId2'),
        }
           fetch(`api/Cut/Return_Porject`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json)
                var table2 = [];
                table2 = json
                for (var i = 0; i < json.length; i++) {
                    //table2.push({id:json[i].glass_info[0].id})
                    var canvas = document.getElementById('canvas');
                    if (canvas.getContext) {
                        var ctx = canvas.getContext('2d');
                        if (json[i].width > 999 || json[i].length > 999) {
                            ctx.fillStyle = 'rgba(9, 157, 215, 0.7)';
                            ctx.fillRect(0, i * 2000, (json[i].width), (json[i].length)); ///tafla

                        }
                        else {
                            ctx.fillStyle = "rgba(9, 157, 215, 0.7)";
                            ctx.fillRect(0, 2000 * i, json[i].width, json[i].length); ///tafla

                        }

                        if (json[i].width === 0) {
                            sessionStorage.setItem('uncat', json[i].error_Messege)

                        }
                        else {
                            for (var j = 0; j < json[i].glass_info[0].pieces.length; j++) {
                                ctx.strokeRect(json[i].glass_info[0].pieces[j].x, json[i].glass_info[0].pieces[j].y + (2000 * i), json[i].glass_info[0].pieces[j].widht, json[i].glass_info[0].pieces[j].lenght);///itemy
                            }
                        }
                    }
                    console.log('beka z MimeTypeArray')
                };

                this.setState({
                    glass_ids: table2
                })
                //return (json)
            })
    }


    cutOrder = (event) => {
        event.preventDefault();
       /* const receiver = {
            glasses:
                this.state.glass_ids
            ,
            order: {
                id_order: sessionStorage.getItem('idOpti')
            }

        }


        fetch(`api/Cut/Save_Project`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(receiver)
                console.log(json)
                return (json)
            })
            .then(json => {
                sessionStorage.setItem('id_order', json);
            })
        this.props.history.push('/pick_machine');

        console.log(receiver)*/
    }

    //api/Cut/Save_Project
    saveProject = (event) => {
        event.preventDefault();
        this.props.history.push('/saved_projects')

    }


    render() {
        return (
            <div>
                <Sidebar />
                <div className="test_c">
                    <div className="canva">
                        <canvas className="canvas" id="canvas" width="10000" height="10000" ></canvas>
                    </div>
                    <div className="table2">
                        <h2>Tafle</h2>
                        <GlassTableProject />
                    </div>
                    <div className="table3">
                        <h2>Produkty</h2>
                        <ItemsTable />
                    </div>
                        <div>
                            <button className="prim_test" onClick={this.saveProject}>Powrót</button>
                            <button className="success_test" onClick={this.cutOrder}>Wytnij</button>

                        </div>
                    

                </div>
            </div>
        );
    }
}