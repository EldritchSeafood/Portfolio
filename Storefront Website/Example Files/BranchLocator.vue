<template>
    <v-container>
        <v-row align="center" justify="center">
            <v-col class="title text-center">Find Branches Near You</v-col>
        </v-row>
        <v-row justify="center">
            <v-col>
            <v-text-field color="primary" label="Enter address" v-model="address" required></v-text-field>
            </v-col>
        </v-row>
        <v-row justify="center">
            <v-btn
            @click="popDialog()"
            :class=" { 'purple darken-4 white--text' :valid, disabled: !valid }"
            >LOCATE</v-btn>
        </v-row>
        <v-row
        class="display-1"
        justify="center"
        align="center"
        style="margin-top:2vh;color:purple"
        >{{status}}</v-row>
        <v-dialog v-model="dialog" justify="center" align="center">
            <v-card>
                <v-row>
                    <v-spacer></v-spacer>
                    <v-btn text @click="dialog = false" style="font-size:XX-large;margin:2vw;">X</v-btn>
                </v-row>
                <v-row
                class="headline"
                justify="center"
                align="center"
                style="margin-left:3vw;margin-right:3vw;color:purple"
                >3 Closest Branches</v-row>
                <v-row justify="center">
                    <div
                    id="map"
                    ref="map"
                    class="googlemap"
                    style="margin-bottom:5vw;margin-top:3vw"
                    ></div>
                </v-row>
            </v-card>
        </v-dialog>
    </v-container>
</template>

<script>
    import fetcher from "../mixins/fetcher";
    export default {
        data() {
            return {
                status: "",
                details: [],
                address: "N5Y-5R6",
                valid: true,
                map: null,
                dialog: false,
                dialogStatus: {}
            };
        },
        mixins: [fetcher],
        methods: {
            getBranchLocations: async function() {
                try {
                    // A service for converting between an address to LatLng
                    let geocoder = new window.google.maps.Geocoder();
                    geocoder.geocode({ address: this.address }, async (results, status) => {
                        if (status === window.google.maps.GeocoderStatus.OK) {
                            // only if google gives us the OK
                            let lat = results[0].geometry.location.lat();
                            let lng = results[0].geometry.location.lng();
                            let myLatLng = new window.google.maps.LatLng(lat, lng);
                            let options = {
                                zoom: 10,
                                center: myLatLng,
                                mapTypeId: window.google.maps.MapTypeId.ROADMAP
                            };
                            this.map = new window.google.maps.Map(this.$refs["map"], options);
                            let center = this.map.getCenter();
                            this.map.setCenter(center);
                            window.google.maps.event.trigger(this.map, "resize");
                            let infowindow = new window.google.maps.InfoWindow({ content: "" });
                            // add the markers, event handlers, infowindows for each location
                            let i2 = 0;
                            let route = this.$_buildRouteWithParams("branch", lat, lng);
                            this.details = await this.$_getdata(route);
                            this.details.map(detail => {
                                i2++;
                                let img = `/img/marker${i2}.png`;
                                let marker = new window.google.maps.Marker({
                                    position: new window.google.maps.LatLng(
                                    detail.latitude,
                                    detail.longitude
                                    ),
                                    animation: window.google.maps.Animation.DROP,
                                    icon: img,
                                    title: `Branch#${detail.id} ${detail.street}, ${detail.city},${detail.region}`,
                                    html: `<div>Branch#${detail.id}<br/>${detail.street}, ${
                                    detail.city
                                    }<br/>${detail.distance.toFixed(2)} mi</div>`,
                                    visible: true
                                });
                                window.google.maps.event.addListener(marker, "click", () => {
                                    infowindow.setContent(marker.html); // added .html to the marker object.
                                    infowindow.close();
                                    infowindow.open(this.map, marker);
                                });
                                marker.setMap(this.map);
                            });
                            this.map.setCenter(center);
                            window.google.maps.event.trigger(this.map, "resize");
                        }
                    });
                } catch (err) {
                    console.log(err);
                }
            },
            popDialog: async function() {
                this.dialogStatus = "";
                this.dialog = !this.dialog;
                await this.getBranchLocations();
            }
        }
    };
</script>